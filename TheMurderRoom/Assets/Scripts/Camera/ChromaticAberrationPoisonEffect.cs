using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChromaticAberrationPoisonEffect : MonoBehaviour {

    private PostProcessVolume volume;
    private ChromaticAberration ca;

    public float speed = 2;

    private bool active = false;
    private float startTime = 0;
    private float endIntensity = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        ca = volume.profile.GetSetting<ChromaticAberration>();

        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed * Mathf.PI * 0.5f;
            sinValue = Mathf.Sin(sinValue - (Mathf.PI/2));

            // make sinValue be between 0-1
            sinValue += 1;
            sinValue /= 2;
            ca.intensity.value = sinValue;
        }
        // go back to normal
        else if (!active && ca.active) {
            ca.intensity.value = Mathf.Lerp(endIntensity, 0, returnTimer.TimePercentagePassed());
            if (Mathf.Approximately(ca.intensity.value, 0)) {
                ca.active = false;
            }
        }
        if (Input.GetKeyDown("j")) {
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
        ca.active = true;
        startTime = Time.realtimeSinceStartup;
    }

    public void Deactivate() {
        active = false;
        returnTimer.Start();
        endIntensity = ca.intensity.value;
    }
}
