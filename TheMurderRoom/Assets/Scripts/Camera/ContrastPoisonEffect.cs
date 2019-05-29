using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ContrastPoisonEffect : MonoBehaviour {

    private static int INFINITE = -1;

    private PostProcessVolume volume;
    private ColorGrading cg;

    public float speed = 2;
    [Range(0, 1)]
    public float intensity = 1f;

    private bool active = false;
    private float startTime = 0;

    // used to interpolate from when done for a smooth ending 
    private float endContrast = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

   
    private int loopAmount = INFINITE;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        cg = volume.profile.GetSetting<ColorGrading>();

        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed;
            float value = Mathf.Sin(sinValue - (Mathf.PI / 2));

            // make sinValue be between 0-100
            value += 1;
            value /= 2;
            value *= 100;
            value *= intensity;
            cg.contrast.value = value;

            if(loopAmount != INFINITE) {
                if(((Time.realtimeSinceStartup - startTime) * speed) / (2*Mathf.PI) >= loopAmount) {
                    Deactivate();
                }
            }
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
                Activate(INFINITE);
            }
        }
    }

    public void Activate(int loopAmount) {
        this.loopAmount = loopAmount;
        if(loopAmount < 0) {
            this.loopAmount = INFINITE;
        }
        else {
            this.loopAmount = loopAmount;
        }

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
