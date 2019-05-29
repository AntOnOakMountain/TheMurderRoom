using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelLightEffect : MonoBehaviour {

    public static TimeTravelLightEffect instance;
    public Transform magicCircle;
    public Transform reverseMagicCircle;

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

        magicCircle.gameObject.SetActive(false);
        reverseMagicCircle.gameObject.SetActive(false);

        waitForParticleSystem = new Timer(2f);
        timer = new Timer(duration);
    }

    public void Activate(bool reverse) {
        this.reverse = reverse;
        if (reverse) {
            reverseMagicCircle.gameObject.SetActive(true);
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
            timer = new Timer(duration);
        }
        light.range = start;

        gameObject.SetActive(true);
    }

    public void Update() {
        if(waitForParticleSystem.IsDone() && !timer.IsActive()) {
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
            reverseMagicCircle.gameObject.SetActive(false);
            magicCircle.gameObject.SetActive(false);
        }
    }
}
