﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    private new Light light;

    public float rangeAmount = 0.5f;
    public float intensityAmount = 0.1f;
    public float howOften = 0.1f;

    private float defaultRange;
    private float defaultIntensity;

    private float previousRange;
    private float previousIntensity;
    private float goalRange;
    private float goalIntensity;

    private Timer timer;

    // Use this for initialization
    void Start() {
        light = GetComponent<Light>();
        defaultRange = light.range;
        defaultIntensity = light.intensity;

        goalRange = defaultRange;
        goalIntensity = defaultIntensity;
        previousRange = defaultRange;
        previousIntensity = defaultIntensity;
        timer = new Timer(howOften);
        timer.Start();
    }

    // Update is called once per frame
    void Update() {
        if (timer.IsDone()) {
            goalRange = defaultRange + Random.Range(-rangeAmount, rangeAmount);
            goalIntensity = defaultIntensity + Random.Range(-intensityAmount, intensityAmount);

            previousRange = light.range;
            previousIntensity = light.intensity;

            timer.Start();// restart timer
        }
        else {
            light.range = Mathf.Lerp(previousRange, goalRange, timer.TimePercentagePassed());
            light.intensity = Mathf.Lerp(previousIntensity, goalIntensity, timer.TimePercentagePassed());
        }

    }
}