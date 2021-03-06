﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignettePoisonEffect : MonoBehaviour, ICameraEffect {

    private static int INFINITE = -1;

    private PostProcessVolume volume;
    private Vignette vignette;

    private float normalIntesity;

    public float intensity = 0.3f;
    public float speed = 2;

    private bool active = false;
    private float startTime = 0;

    // intensity when effect is deactivated
    private float endIntensity = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    private int loopAmount = INFINITE;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        vignette = volume.profile.GetSetting<Vignette>();

        normalIntesity = vignette.intensity;
        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed;
            float value = Mathf.Sin(sinValue);
            value *= intensity;
            // only positive for better effect
            value = Mathf.Abs(value);
            vignette.intensity.value = normalIntesity + value;

            if (loopAmount != INFINITE) {
                // each loop is half a loop so, * 2 to get actual number of loops
                if (2 * ((Time.realtimeSinceStartup - startTime) * speed) / (2 * Mathf.PI) >= loopAmount) {
                    Deactivate();
                }
            }
        }
        // go back to normal
        else if(!active && !Mathf.Approximately(vignette.intensity, normalIntesity)) {
            vignette.intensity.value = Mathf.Lerp(endIntensity, normalIntesity, returnTimer.TimePercentagePassed());
        }
        #if (UNITY_EDITOR)
                if (Input.GetKeyDown("k")) {
                    if (active) {
                        Deactivate();
                    }
                    else {
                        Activate(INFINITE);
                    }
                }
        #endif
    }

    public void Activate(int loopAmount) {
        Deactivate();
        this.loopAmount = loopAmount;
        if (loopAmount < 0) {
            this.loopAmount = INFINITE;
        }
        else {
            this.loopAmount = loopAmount;
        }

        active = true;
        startTime = Time.realtimeSinceStartup;
    }

    public void Deactivate() {
        active = false;
        endIntensity = vignette.intensity;
        returnTimer.Start();
    }

}
