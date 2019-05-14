using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Fungus;


public class Npc : MonoBehaviour {

    [Tooltip("Flowchart linked to this npc.")]
    public Flowchart flowchart;
    private Interact flowchartInteract;

    [HideInInspector] public IKController ikController;


	void Start () {
        if(flowchart == null) {
            Debug.Log("this npc should have a flowchart linked to it.", this);
        }
        else {
            flowchartInteract = flowchart.GetComponent<Interact>();
        }

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
}
