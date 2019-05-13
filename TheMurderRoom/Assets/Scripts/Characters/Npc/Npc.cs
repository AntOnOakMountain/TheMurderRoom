using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Fungus;


public class Npc : MonoBehaviour {

    [Tooltip("For testing NavMesh Pathfinding")]
    public Transform navTarget;

    [Tooltip("Flowchart linked to this npc.")]
    public Flowchart flowchart;
    private Interact flowchartInteract;

    [Tooltip("Set the head of the model as this transform.")]
    public Transform head;
    [Tooltip("Set the root of the model as this transform.")]
    public Transform body;

    private Animator animator;
    private AnimationPlayOnceManager apom;

	void Start () {
        // Testing NavMesh Pathfinding
        if(navTarget != null) {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = navTarget.position;
        }

        if(flowchart == null) {
            Debug.Log("this npc should have a flowchart linked to it.", this);
        }
        else {
            flowchartInteract = flowchart.GetComponent<Interact>();
        }



	}

    public bool Interact() {
        Journal.journal.Clear();
        Journal.journal.ToggleJournal(true);
        if(Player.instance.roomEmitter != null)
            Player.instance.roomEmitter.SetParameter("Conversation", 1.0f);
        return flowchartInteract.InteractWith();
    }
	
	void Update () {
		
	}
}
