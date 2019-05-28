using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LensDistortionPoisonEffect : MonoBehaviour {

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

            float sinValue = (Time.realtimeSinceStartup - startTime) * speed * Mathf.PI * 0.5f;
            sinValue = Mathf.Sin(sinValue);

            // make sinValue be between -xRange to xRange
            sinValue *= xRange;

            ld.centerX.value = sinValue;
        }
        // go back to normal
        else if (!active && ld.active) {
            ld.intensity.value = Mathf.Lerp(goalIntensity, 0, returnTimer.TimePercentagePassed());
            if (Mathf.Approximately(ld.intensity.value, 0)) {
                ld.active = false;
            }
        }
        if (Input.GetKeyDown("g")) {
            if (active) {
                Deactivate();
            }
            else {
                Activate();
            }
        }
    }

    public void Activate() {
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
