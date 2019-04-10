using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Player : MonoBehaviour {

    // Player singleton
    private static Player player;
    public static Player GetPlayer() {
        return player;
    }

    public float speed = 10;
    public float gravity = 9.81f;

    public LayerMask interactableMask;
    private Interact lookingAt = null;
    
    private Camera FPCamera;
    private CharacterController characterController;

	void Start () {
        if(player == null) {
            player = this;
        }
        else {
            Debug.Log("Only one player instance should exist per scene", this);
        }
        
        FPCamera = transform.Find("Main Camera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        // Interact with NPC/Objects
        if(lookingAt != null && Input.GetButtonDown("Interact")) {
            lookingAt.InteractWith();
        }
    }

    void FixedUpdate () {
        Movement();
        ScanForInteractables();
    }

    private void Movement() {
        // Movement
        float moveForward = Input.GetAxis("Vertical");
        float moveSideways = Input.GetAxis("Horizontal");

        // Get forward and right direction vectors. Remove direction in Y to prevent flying
        Vector3 forward = new Vector3(FPCamera.transform.forward.x, 0, FPCamera.transform.forward.z).normalized;
        Vector3 right = new Vector3(FPCamera.transform.right.x, 0, FPCamera.transform.right.z).normalized;

        // Get how much in each direction to move in
        Vector3 forwardMovement = forward * moveForward;
        Vector3 sidewaysMovement = right * moveSideways;
        Vector3 movement = (forwardMovement + sidewaysMovement).normalized;

        // Multiply by scalar
        movement = movement * speed * Time.fixedDeltaTime;

        // Add fake gravity
        movement.y = -gravity * Time.fixedDeltaTime;

        // Move
        characterController.Move(movement);
    }

    private void ScanForInteractables() {
        // Raycast (what do you look at)
        
        RaycastHit hit;
        Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, 5f, interactableMask);
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, 5f, interactableMask)) {
            if (hit.transform.tag == "Npc") {
                lookingAt = hit.transform.GetComponent<Interact>();
            }
        }
        else {
            lookingAt = null;
        }
    }
}
