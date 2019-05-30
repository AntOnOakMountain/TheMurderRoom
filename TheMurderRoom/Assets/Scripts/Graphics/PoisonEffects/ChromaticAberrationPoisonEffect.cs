using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChromaticAberrationPoisonEffect : MonoBehaviour, ICameraEffect {

    private static int INFINITE = -1;

    private PostProcessVolume volume;
    private ChromaticAberration ca;

    public float speed = 2;
    [Range(0, 1)]
    public float intensity = 1;

    private bool active = false;
    private float startTime = 0;

    // used to interpolate from when done for a smooth ending 
    private float endIntensity = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    private int loopAmount;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        ca = volume.profile.GetSetting<ChromaticAberration>();

        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed;
            float value = Mathf.Sin(sinValue - (Mathf.PI/2));

            // make sinValue be between 0-1
            value += 1;
            value /= 2;
            value *= intensity;
            ca.intensity.value = value;

            if (loopAmount != INFINITE) {
                if (((Time.realtimeSinceStartup - startTime) * speed) / (2 * Mathf.PI) >= loopAmount) {
                    Deactivate();
                }
            }
        }
        // go back to normal
        else if (!active && ca.active) {
            ca.intensity.value = Mathf.Lerp(endIntensity, 0, returnTimer.TimePercentagePassed());
            if (Mathf.Approximately(ca.intensity.value, 0)) {
                ca.active = false;
            }
        }
        #if (UNITY_EDITOR)
                if (Input.GetKeyDown("j")) {
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
        ca.active = true;
        startTime = Time.realtimeSinceStartup;
    }

    public void Deactivate() {
        active = false;
        returnTimer.Start();
        endIntensity = ca.intensity.value;
    }
}
