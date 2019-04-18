using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Player : MonoBehaviour {

    // Player singleton(ich) for easy access for other scripts
    private static Player player;
    public static Player Instance{
        get { return player; }
    }

    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    [HideInInspector]
    public float speed = 0;
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float gravity = 9.81f;
    private float previousForward = 0;
    private float previousSideways = 0;
    private CharacterController characterController;
    private OpenPad pad;

    // Variables for interacting with Npc/objects
    private LayerMask interactableMask;
    private Interact lookingAt = null;

    /// <summary> Used for the basic camera controls </summary>
    private FPCamera cameraFixture;

    // State variables
    public enum State {
        Play, Dialogue
    }
    private State state = State.Play;

    void Start () {
        // Init Player singleton
        if(player == null) {
            player = this;
        }
        else {
            Debug.Log("Only one player instance should exist per scene", this);
        }
        
        cameraFixture = transform.Find("CameraFixture").GetComponent<FPCamera>();
        pad = transform.Find("Pad").GetComponent<OpenPad>();

        characterController = GetComponent<CharacterController>();

        // Create the interact layermask
        int npcLayer = 1 << LayerMask.NameToLayer("Npc");
        int interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
        interactableMask.value = npcLayer | interactableLayer;
    }

    void Update() {
        if (state == State.Play) {
            // Interact with NPC/Objects
            ScanForInteractables();
            if (lookingAt != null && Input.GetButtonDown("Interact")) {
                if (lookingAt.InteractWith()) {
                    state = State.Dialogue;
                    speed = 0;
                }
            }
            else if(Input.GetButtonDown("OpenPad")){
                pad.Open();
                state = State.Dialogue;
                speed = 0;
            }
        }
    }

    void FixedUpdate () {
        if (state == State.Play) {
            Movement();
        }
    }

    private void Movement() {
        // Get movement input
        float moveForward = Input.GetAxis("Vertical");
        float moveSideways = Input.GetAxis("Horizontal");

        // Get forward and right direction vectors. No direction in Y since you can't move up
        Vector3 forward = new Vector3(cameraFixture.transform.forward.x, 0, cameraFixture.transform.forward.z).normalized;
        Vector3 right = new Vector3(cameraFixture.transform.right.x, 0, cameraFixture.transform.right.z).normalized;

        // No input, decelerate
        if (Mathf.Abs(moveForward) < Mathf.Epsilon && Mathf.Abs(moveSideways) < Mathf.Epsilon) {
            if(speed < deceleration * Time.fixedDeltaTime) {
                speed = 0;
            }
            else {
                speed -= deceleration * Time.fixedDeltaTime;
            }
            // If you still have speed you need a direction to move in when decelerating, so set it as the previous direction
            moveForward = previousForward;
            moveSideways = previousSideways;
        }
        // Input, accelerate
        else {
            speed += acceleration * Time.fixedDeltaTime;
            if(speed > maxSpeed) {
                speed = maxSpeed;
            }
        }

        // Calculate directions weight
        Vector3 forwardWeight = forward * moveForward;
        Vector3 sidewaysWeight = right * moveSideways;
        Vector3 movement = (forwardWeight + sidewaysWeight).normalized;

        // Calculate full movement vector
        movement = movement * speed * Time.fixedDeltaTime;

        // Add fake gravity
        movement.y = -gravity * Time.fixedDeltaTime;

        // Save input directions as previous input directions
        previousForward = moveForward;
        previousSideways = moveSideways;

        // Move
        characterController.Move(movement);
    }


    private RaycastHit hit;
    private void ScanForInteractables() {
        // Raycast (what do you look at)
        Physics.Raycast(cameraFixture.transform.position, cameraFixture.transform.forward, out hit, 5f, interactableMask);
        if (Physics.Raycast(cameraFixture.transform.position, cameraFixture.transform.forward, out hit, 5f, interactableMask)) {
            // If looking at a Npc or interactable, save it in lookingAt
            if (hit.transform.tag == "Npc" || hit.transform.tag == "Interactable") {
                lookingAt = hit.transform.GetComponent<Interact>();
            }
        }
        else {
            // When no longer looking at a Npc/Interactable null lookingAt
            lookingAt = null;
        }
    }

    /// <summary>
    /// Called to from fungus to signal when an conversation have ended
    /// </summary>
    public void EndDialogue() {
        state = State.Play;
    }

    public State GetState() {
        return state;
    }

    public bool IsInState(State state) {
        return this.state == state;
    }
}
