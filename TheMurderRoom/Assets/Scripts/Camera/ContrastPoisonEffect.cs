using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ContrastPoisonEffect : MonoBehaviour {

    private PostProcessVolume volume;
    private ColorGrading cg;

    public float speed = 2;

    private bool active = false;
    private float startTime = 0;
    private float endContrast = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        cg = volume.profile.GetSetting<ColorGrading>();

        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed * Mathf.PI * 0.5f;
            sinValue = Mathf.Sin(sinValue - (Mathf.PI / 2));

            // make sinValue be between 0-100
            sinValue += 1;
            sinValue /= 2;
            sinValue *= 100;
            cg.contrast.value = sinValue;
        }
        // go back to normal
        else if (!active && cg.active) {
            cg.contrast.value = Mathf.Lerp(endContrast, 0, returnTimer.TimePercentagePassed());
            if (Mathf.Approximately(cg.contrast.value, 0)) {
                cg.active = false;
            }
        }
        if (Input.GetKeyDown("h")) {
            if (active) {
                Deactivate();
            }
            else {
                Activate();
            }
        }
    }

    public void Activate() {
        active = true;
        cg.active = true;
        startTime = Time.realtimeSinceStartup;
    }

    public void Deactivate() {
        active = false;
        returnTimer.Start();
        endContrast = cg.contrast.value;
    }
}
