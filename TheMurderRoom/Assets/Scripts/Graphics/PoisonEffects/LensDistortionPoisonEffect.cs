using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LensDistortionPoisonEffect : MonoBehaviour, ICameraEffect {

    private static int INFINITE = -1;

    private PostProcessVolume volume;
    private LensDistortion ld;

    public float goalIntensity = -50;
    public float xRange = 0.3f;
    public float speed = 2;

    private bool active = false;
    private float startTime = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    private Timer fadeInTimer;

    private int loopAmount;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        ld = volume.profile.GetSetting<LensDistortion>();

        ld.intensity.value = 0f;
        ld.centerX.value = 0f;

        returnTimer = new Timer(returnTime);
        fadeInTimer = new Timer(1);
    }

    void Update() {
        if (active) {
            ld.intensity.value = Mathf.Lerp(0, goalIntensity, fadeInTimer.TimePercentagePassed());

            float sinValue = (Time.realtimeSinceStartup - startTime) * speed;
            float value = Mathf.Sin(sinValue);

            // make value be between -xRange to xRange
            value *= xRange;

            ld.centerX.value = value;

            if (loopAmount != INFINITE) {
                if (((Time.realtimeSinceStartup - startTime) * speed) / (2 * Mathf.PI) >= loopAmount) {
                    Deactivate();
                }
            }
        }
        // go back to normal
        else if (!active && ld.active) {
            ld.intensity.value = Mathf.Lerp(goalIntensity, 0, returnTimer.TimePercentagePassed());
            if (Mathf.Approximately(ld.intensity.value, 0)) {
                ld.active = false;
            }
        }
        #if (UNITY_EDITOR)
                if (Input.GetKeyDown("g")) {
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
        ld.active = true;
        active = true;
        startTime = Time.realtimeSinceStartup;
        fadeInTimer.Start();
    }

    public void Deactivate() {
        active = false;
        returnTimer.Start();
    }
}
