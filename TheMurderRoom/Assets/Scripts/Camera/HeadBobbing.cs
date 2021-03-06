﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour {

    [Header("HeadBobbing")]
    public float intensity = 0.1f;
    public float bobbingSpeed = 2f;
    [Tooltip("Speed to return to neutral when standing still.")]
    public float bobbingReturnSpeed = 0.2f;
    private float bobOscillate = 0;
    private float timer = 0;

    [FMODUnity.EventRef]
    public string playerFootstepEvent;
    FMOD.Studio.EventInstance player_Footstep;

    // limit footstep sound to not trigger multiple times at almost the same time
    private Timer timeSinceLastFootstep;

    void Start() {
        timeSinceLastFootstep = new Timer(0.2f);
        timeSinceLastFootstep.Start();
        timeSinceLastFootstep.InstantFinish();
    }

    void Update() {
        // If standing still return to neutral position
        if (Mathf.Abs(Player.instance.speed) < Mathf.Epsilon) {
            float timeLeftUntilNeutral = timer % 0.5f;
            // if not back to neutral, go back to neutral
            if (!(timeLeftUntilNeutral < Mathf.Epsilon)) {
                if (timeLeftUntilNeutral < bobbingReturnSpeed) {
                    timer = 0;
                }
                else {
                    timer += bobbingReturnSpeed * Time.deltaTime;
                }
            }
        }
        else {
            timer += Time.deltaTime * bobbingSpeed * (Player.instance.speed / Player.instance.maxSpeed);
        }

        bobOscillate = Mathf.Sin(timer * (2 * Mathf.PI));

        transform.localPosition = new Vector3(0, bobOscillate * intensity, 0);

        if(bobOscillate < -0.9f) {
            PlayFootstep();
        }
    }

    private void PlayFootstep() {
        if (timeSinceLastFootstep.IsDone()) {
            player_Footstep = FMODUnity.RuntimeManager.CreateInstance(playerFootstepEvent);
            player_Footstep.start();
            timeSinceLastFootstep.Start();
        }
    }
}
