using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    private new Light light;


    public float howOftenMin = 1f;
    public float howOftenMax = 1f;

    public float duration = 0.1f;

    private float defaultRange;
    private float defaultIntensity;

    public float lightningIntensity = 90;
    public float lightningRange = 100;

    private Timer timer;
    private Timer timerDuration;

    // Use this for initialization
    void Start() {
        light = GetComponent<Light>();
        defaultRange = light.range;
        defaultIntensity = light.intensity;

        timerDuration = new Timer(duration);
        timer = new Timer(Random.Range(howOftenMin, howOftenMax));
        timer.Start();
    }

    // Update is called once per frame
    void Update() {
        if (timer.IsDone() && !timerDuration.IsActive()) {
            light.intensity = lightningIntensity;
            light.range = lightningRange;

            timerDuration.Start();// restart timer
        }
        //else if(timerDuration.IsDone() && timer.){
            //light.range = Mathf.Lerp(previousRange, goalRange, timer.TimePercentagePassed());
            //light.intensity = Mathf.Lerp(previousIntensity, goalIntensity, timer.TimePercentagePassed());
        //}

    }
}
