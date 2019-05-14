using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Fungus;


public class Npc : MonoBehaviour {

    private State state;
    public enum State {
        Idle, Dialogue
    }

    [Tooltip("How fast the character will rotate it's body when talking to the player.")]
    public float rotateSpeed = 100f;

    [Tooltip("Flowchart linked to this npc.")]
    public Flowchart flowchart;
    private Interact flowchartInteract;

    private Quaternion rotationBeforeDialogue;

    [HideInInspector] public IKController ikController;

	void Start () {
        if(flowchart == null) {
            Debug.Log("this npc should have a flowchart linked to it.", this);
        }
        else {
            flowchartInteract = flowchart.GetComponent<Interact>();
            flowchart.npc = this;
        }
        rotationBeforeDialogue = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        ikController = GetComponentInChildren<IKController>();
    }

    public bool Interact() {
        Journal.journal.Clear();
        Journal.journal.ToggleJournal(true);
        if(Player.instance.roomEmitter != null)
            Player.instance.roomEmitter.SetParameter("Conversation", 1.0f);
        return flowchartInteract.InteractWith();
    }
	
	void Update () {
        if(state == State.Dialogue) {

            // Rotate npc towards player
            Vector3 playerDistance =  Player.instance.transform.position- transform.position;
            Quaternion goalRotation = Quaternion.LookRotation(playerDistance.normalized, Vector3.up);
            // Only rotate in Y axis
            goalRotation =  Quaternion.Euler(0, goalRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, rotateSpeed * Time.deltaTime);
        }
        else {
            // Rotate back to default rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationBeforeDialogue, rotateSpeed * Time.deltaTime);
        }
        
        if(ikController != null) {
            Vector3 playerDistance = Player.instance.GetCameraPosition() - transform.position;
            float lookAtDistance = 3f;
            float lookAtMaxAngle = 60f;

            Vector3 nForward = transform.forward.normalized;
            Vector3 nDistance = playerDistance.normalized;

            if (Vector3.Angle(nForward, nDistance) < lookAtMaxAngle && 
                playerDistance.sqrMagnitude < Mathf.Pow(lookAtDistance, 2)) {
                ikController.LookAtPlayer();
            }
            else {
                ikController.StopLookingAtPlayer();
            }
        }
	}

    public void SetState(State newState) {
        state = newState;

        if(newState == State.Dialogue) {
            // Save rotation it had before player talked to it
            rotationBeforeDialogue = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
    }

    public void EndDialogue() {
        SetState(State.Idle);
    }

    public State GetState() {
        return state;
    }
}
