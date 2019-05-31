using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Gramophone : MonoBehaviour {

    public static Gramophone instance;

    public float part1Duration = 3;
    public FMODUnity.StudioEventEmitter part1;
    public float part2Duration = 3;
    public FMODUnity.StudioEventEmitter part2;

    private Timer timer;
    bool part1Active = true;

    // Use this for initialization
    void Start () {
        instance = this;
        part1Active = true;
        part1.Play();
        timer = new Timer(part1Duration);
        timer.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (timer.IsDone()) {
            if (part1Active) {
                part1Active = false;
                part2.Play();
                timer = new Timer(part2Duration);
                timer.Start();
            }
            else {
                part1Active = true;
                part1.Play();
                timer = new Timer(part1Duration);
                timer.Start();
            }
        }
	}

    public void SetParameter(float p) {
        part1.SetParameter("Conversation", p);
        part2.SetParameter("Conversation", p);
    }
}
