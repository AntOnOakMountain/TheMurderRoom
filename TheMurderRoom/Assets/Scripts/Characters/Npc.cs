using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Fungus;


public class Npc : MonoBehaviour {


    public Transform navTarget;

	void Start () {
        if(navTarget != null) {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = navTarget.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
