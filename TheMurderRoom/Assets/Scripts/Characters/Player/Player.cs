using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Player : MonoBehaviour {

    // Player singleton(ich) for easy access for other scripts
    private static Player player;
    public static Player instance{
        get { return player; }
    }

    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    [HideInInspector] public float speed = 0;
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float gravity = 9.81f;
    private float previousForward = 0;
    private float previousSideways = 0;
    private CharacterController characterController;

    public FMODUnity.StudioEventEmitter roomEmitter;
   
    // Variables for interacting with Npc/objects
    private LayerMask interactableMask;
    private Npc lookingAt = null;
    [Tooltip("Max distance which you can interact with an npc from.")]
    public float interactDistance = 2f;

    /// <summary> Used for the basic camera controls </summary>
    [HideInInspector] public FPCamera fpCamera;

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
        
        fpCamera = transform.Find("CameraFixture").GetComponent<FPCamera>();

        characterController = GetComponent<CharacterController>();

        // Create the interact layermask
        int npcLayer = 1 << LayerMask.NameToLayer("Npc");
        int interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
        interactableMask.value = npcLayer | interactableLayer;

        this.SetState(State.Play);
    }

    void Update() {
        if (state == State.Play) {
            ScanForInteractables();
            // Interact with NPC/Objects
            if (lookingAt != null && Input.GetButtonDown("Interact")) {
                if (lookingAt.Interact()) {
                    Game.instance.SetState(Game.State.Dialogue); // also sets player ti dialogue state
                    lookingAt.SetState(Npc.State.Dialogue);
                    if (lookingAt.ikController != null) {
                        fpCamera.dialogueCamera.StartDialogueFocusOn(lookingAt.ikController.head);
                    }
                }
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
        Vector3 forward = new Vector3(fpCamera.transform.forward.x, 0, fpCamera.transform.forward.z).normalized;
        Vector3 right = new Vector3(fpCamera.transform.right.x, 0, fpCamera.transform.right.z).normalized;

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
        bool found = false;

        // Raycast (what do you look at)
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, interactDistance, interactableMask)) {
            // If looking at a Npc or interactable, save it in lookingAt
            if (hit.transform.tag == "Npc" || hit.transform.tag == "Interactable") {
                Npc npc = hit.transform.GetComponent<Npc>();
                Vector3 npcDistance = transform.position - npc.transform.position;

                Vector3 nForward = npc.transform.forward;

                // only interactable from front
                if (Vector3.Angle(nForward, npcDistance) < npc.maxInteractAngle) {
                    lookingAt = hit.transform.GetComponent<Npc>();
                    UIManager.Instance.interactPrompt.gameObject.SetActive(true);
                    found = true;
                }
            }
        }
        if(!found) {
            // When no longer looking at a Npc/Interactable null lookingAt
            lookingAt = null;
            UIManager.Instance.interactPrompt.gameObject.SetActive(false);
        }
    }

    public void SetState(State newState) {
        state = newState;
        
        switch (newState) {
            case State.Dialogue:
                UIManager.Instance.interactPrompt.gameObject.SetActive(false);
                speed = 0;
                break;
            case State.Play:
                if (roomEmitter != null)
                    roomEmitter.SetParameter("Conversation", 0.0f);
                break;
        }
    }

    public State GetState() {
        return state;
    }

    public bool IsInState(State state) {
        return this.state == state;
    }
}
