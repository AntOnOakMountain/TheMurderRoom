using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldPoisonEffect : MonoBehaviour, ICameraEffect {

    private static int INFINITE = -1;

    private PostProcessVolume volume;
    private DepthOfField dof;

    public float minFocusDistance = 1.5f;
    public float maxFocusDistance = 6f;

    public float speed = 2;

    // interpolate to this to smoothly activate dof
    private float goalFocalLenght = 150;

    private float apeture = 32;
   
    private bool active = false;
    private float startTime = 0;

    [Tooltip("How long in seconds for the effect to return to normal after being deactivated.")]
    public float returnTime = 1f;
    private Timer returnTimer;

    private Timer fadeInTimer;

    private int loopAmount;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        dof = volume.profile.GetSetting<DepthOfField>();

        dof.focalLength.value = 1f;
        dof.aperture.value = apeture;

        returnTimer = new Timer(returnTime);
        fadeInTimer = new Timer(1);
    }

    void Update() {
        if (active) {
            // fade in effect
            dof.focalLength.value = Mathf.Lerp(1, goalFocalLenght, fadeInTimer.TimePercentagePassed());

            float sinValue = (Time.realtimeSinceStartup - startTime) * speed;
            float value = Mathf.Sin(sinValue);

            // make sinValue be between 0-1
            value += 1;
            value /= 2;
            dof.focusDistance.value = Mathf.Lerp(minFocusDistance, maxFocusDistance, value);

            if (loopAmount != INFINITE) {
                if (((Time.realtimeSinceStartup - startTime) * speed) / (2 * Mathf.PI) >= loopAmount) {
                    Deactivate();
                }
            }
        }
        // go back to normal
        else if (!active && dof.active) {
            dof.focalLength.value = Mathf.Lerp(goalFocalLenght, 1, returnTimer.TimePercentagePassed());
            if(Mathf.Approximately(dof.focalLength.value, 1)) {
                dof.active = false;
            }
        }
        #if (UNITY_EDITOR)
                if (Input.GetKeyDown("l")) {
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
        dof.active = true;
        active = true;
        startTime = Time.realtimeSinceStartup;
        fadeInTimer.Start();
    }

    public void Deactivate() {
        active = false;
        returnTimer.Start();
    }
    
}
