using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Lightning : MonoBehaviour {

    private new Light light;

    [Header("Time range for how often lightning will strike")]
    public float howOftenMin = 1f;
    public float howOftenMax = 1f;

    [Tooltip("how long the light will stay (and fade out during)")]
    public float duration = 0.1f;

    private float defaultRange;
    private float defaultIntensity;

    [Tooltip("how high intensity the light will have when lightning strikes")]
    public float lightningIntensity = 90;
    [Tooltip("how long  range the light will have when lightning strikes")]
    public float lightningRange = 100;

    private Timer timer;

    private bool lightOn = false;

    public float maxYOffset = 2f;
    public float maxXOffset = 5f;

    private Vector3 originalPosition;
    private Vector3 right;
    private Vector3 up;

    // Use this for initialization
    void Start() {
        originalPosition = transform.position;
        right = transform.right;
        up = transform.up;

        light = GetComponent<Light>();
        defaultRange = light.range;
        defaultIntensity = light.intensity;

        timer = new Timer(Random.Range(howOftenMin, howOftenMax));
        timer.Start();
    }

    // Update is called once per frame
    void Update() {
        if (timer.IsDone()) {
            // turn off light
            if (lightOn) {
                light.intensity = 0;
                light.range = 0;

                lightOn = false;

                timer = new Timer(Random.Range(howOftenMin, howOftenMax));
                timer.Start();
            }
            // turn on light
            else {
                float xOffset = Random.Range(-maxXOffset, maxXOffset);
                float yOffset = Random.Range(-maxYOffset, maxYOffset);
                Vector3 newPosition = originalPosition;
                newPosition += up * yOffset;
                newPosition += right * xOffset;
                transform.position = newPosition;
                lightOn = true;
                light.intensity = lightningIntensity;
                light.range = lightningRange;

                timer = new Timer(duration);
                timer.Start();// restart timer
            }
        }

        if (lightOn) {
            // fade out light
            light.intensity = Mathf.Lerp(lightningIntensity, 0, timer.TimePercentagePassed());
            //light.range = Mathf.Lerp(lightningRange, 0, timer.TimePercentagePassed());
        }
    }
}
