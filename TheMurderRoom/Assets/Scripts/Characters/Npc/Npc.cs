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
    public float rotateSpeed = 100;
    private Quaternion rotationBeforeDialogue;
    private Quaternion dialogueRotation;

    [Tooltip("Flowchart linked to this npc.")]
    public Flowchart flowchart;
    private Interact flowchartInteract;

    [HideInInspector] public NpcIKController ikController;

	void Start () {
        if(flowchart == null) {
            Debug.Log("this npc should have a flowchart linked to it.", this);
        }
        else {
            flowchartInteract = flowchart.GetComponent<Interact>();
            flowchart.npc = this;
        }
        rotationBeforeDialogue = transform.rotation;
        dialogueRotation = transform.rotation;
        ikController = GetComponentInChildren<NpcIKController>();
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
             transform.rotation = Quaternion.RotateTowards(transform.rotation, dialogueRotation, rotateSpeed * Time.deltaTime);
        }
        else if(state == State.Idle) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationBeforeDialogue, rotateSpeed * Time.deltaTime);
        }
	}

    public void SetState(State newState) {
        state = newState;

        if(newState == State.Dialogue) {
            // Save rotation it had before player talked to it
            rotationBeforeDialogue = transform.rotation;

            // Rotate npc towards player
            Vector3 playerDistance = Player.instance.transform.position - transform.position;
            dialogueRotation = Quaternion.LookRotation(playerDistance.normalized, Vector3.up);
            // Only rotate in Y axis
            dialogueRotation = Quaternion.Euler(0, dialogueRotation.eulerAngles.y, 0);
        }
    }

    public void EndDialogue() {
        SetState(State.Idle);
    }

    public State GetState() {
        return state;
    }
}
