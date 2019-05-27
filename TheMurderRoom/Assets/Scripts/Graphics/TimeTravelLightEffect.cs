using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelLightEffect : MonoBehaviour {

    public static TimeTravelLightEffect instance;

    public float duration;
    public float goalRange;

    public float reverseDuration;

    private float start;
    private float end;

    private Timer timer;
    private new Light light;
    private bool reverse;

    public void Start() {
        instance = this;
        light = GetComponent<Light>();
        gameObject.SetActive(false);
    }

    public void Activate(bool reverse) {
        this.reverse = reverse;
        if (reverse) {
            start = goalRange;
            end = 0;
            timer = new Timer(reverseDuration);
        }
        else {
            start = 0;
            end = goalRange;
            timer = new Timer(duration);
        }
        light.range = start;

        gameObject.SetActive(true);
        timer.Start();
    }

    public void Update() {
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
