using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelLightEffect : MonoBehaviour {

    public static TimeTravelLightEffect instance;
    private Transform magicCircle;

    public float duration;
    public float goalRange;

    public float reverseDuration;

    private float start;
    private float end;

    private Timer timer;
    private new Light light;
    private bool reverse;

    private Timer waitForParticleSystem;

    public void Start() {
        instance = this;
        light = GetComponent<Light>();
        gameObject.SetActive(false);

        magicCircle = transform.Find("PS_TimeTravel");
        magicCircle.gameObject.SetActive(false);

        waitForParticleSystem = new Timer(0f);
        timer = new Timer(duration);
    }

    public void Activate(bool reverse) {
        Debug.Log("ACtivate");
        this.reverse = reverse;
        if (reverse) {
            start = goalRange;
            end = 0;
            timer = new Timer(reverseDuration);
            timer.Start();
        }
        else {
            magicCircle.gameObject.SetActive(true);
            start = 0;
            end = goalRange;
            waitForParticleSystem.Start();
        }
        light.range = start;

        gameObject.SetActive(true);
        
    }

    public void Update() {
        if(waitForParticleSystem.IsDone() && !timer.IsActive()) {
            timer = new Timer(duration);
            timer.Start();
        }

        if (timer.IsActive()) {
            if (reverse) {
                light.range = Mathf.Lerp(start, end, Mathf.Sin(timer.TimePercentagePassed() * Mathf.PI * 0.5f));
            }
            else {
                light.range = Mathf.Lerp(start, end, timer.TimePercentagePassed() * timer.TimePercentagePassed());
            }
        }
        if (timer.IsDone()) {
            timer.Pause();
            gameObject.SetActive(false);
        }
    }
}
