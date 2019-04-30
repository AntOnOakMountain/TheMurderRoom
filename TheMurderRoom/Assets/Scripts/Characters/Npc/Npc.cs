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

    private Transform head;
    public Transform GetHead() {
        return head;
    }

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

        head = transform.Find("Head");
	}

    public bool Interact() {
        Journal.journal.Clear();
        Journal.journal.ToggleJournal(true);
        return flowchartInteract.InteractWith();
    }
	
	void Update () {
		
	}
}
