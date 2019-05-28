using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PulsatingVignettePoisonEffect : MonoBehaviour {

    private PostProcessVolume volume;
    private Vignette vignette;

    private float normalIntesity;

    public float intensity = 0.3f;
    public float speed = 2;

    private bool active = false;
    private float startTime = 0;

    // intensity when effect is deactivated
    private float endIntensity = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        vignette = volume.profile.GetSetting<Vignette>();

        normalIntesity = vignette.intensity;
        returnTimer = new Timer(returnTime);
    }

    void Update() {
        if (active) {
            float sinValue = (Time.realtimeSinceStartup - startTime) * speed * Mathf.PI * 0.5f;
            sinValue = Mathf.Sin(sinValue);
            sinValue *= intensity;
            // only positive for better effect
            sinValue = Mathf.Abs(sinValue);
            vignette.intensity.value = normalIntesity + sinValue;
        }
        // go back to normal
        else if(!active && !Mathf.Approximately(vignette.intensity, normalIntesity)) {
            vignette.intensity.value = Mathf.Lerp(endIntensity, normalIntesity, returnTimer.TimePercentagePassed());
        }
        if (Input.GetKeyDown("k")) {
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
        startTime = Time.realtimeSinceStartup;
    }

    public void Deactivate() {
        active = false;
        endIntensity = vignette.intensity;
        returnTimer.Start();
    }

}
