﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Gramophone : MonoBehaviour {

    public static Gramophone instance;

    public float part1Duration = 108;
    public FMODUnity.StudioEventEmitter part1;
    public float part2Duration = 178;
    public FMODUnity.StudioEventEmitter part2;
    public FMODUnity.StudioEventEmitter failstate;

    //[FMODUnity.EventRef]
    //public string Failstate;


    private Timer timer;
    bool part1Active = true;

    // Use this for initialization
    void Start () {
        instance = this;
        part1Active = true;
        part1.Play();
        timer = new Timer(part1Duration);
        timer.Start();
        
        failstate.Stop();
        StartCoroutine(WaitOneFrameAfterStart());
    }
	
	// Update is called once per frame
	void Update () {
        if (timer.IsDone()) {
            if (part1Active) {
                part1Active = false;
                part2.Play();
                part1.Stop();
                timer = new Timer(part2Duration);
                timer.Start();
            }
            else {
                part1Active = true;
                part1.Play();
                part2.Stop();
                timer = new Timer(part1Duration);
                timer.Start();
            }
        }
	}

    public void SetParameter(float p) {
        part1.SetParameter("Conversation", p);
        part2.SetParameter("Conversation", p);
    }

    IEnumerator WaitOneFrameAfterStart()
    {
        yield return new WaitForEndOfFrame();
        Game.instance.timeLeftChanged.AddListener(OneTickLeft);
    }

    public void OneTickLeft(){
        if (Game.instance.timeLeft == 1){
            failstate.Play();
        }
    }
}
