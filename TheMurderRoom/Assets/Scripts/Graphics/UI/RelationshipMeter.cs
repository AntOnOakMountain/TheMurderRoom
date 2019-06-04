using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class RelationshipMeter : MonoBehaviour {

    private Flowchart flowchart;

    public float meterFillSpeed = 32f;
    private float sizeOfMeter;
    public int numberOfValues = 10;
    private int halfNumberOfValues;

    private float intervalUISize;

    private int lastFrameValue;
    private int lastValue;
    private int currentValue;

    private float interpolation;

    public Image badMeter;
    public Image goodMeter;
    public Image blink;

    // Blink valeus
    private float startTime;
    public float blinkSpeed = 10;
    [Range(0, 1)]
    public float blinkIntensity = 0.5f;

    private Color blinkNoAlphaColor;

    private FMODUnity.StudioEventEmitter soundEmitter;

    // Use this for initialization
    void Start () {
        
        //badMeter = transform.Find("Bad").GetComponent<Image>();
        //goodMeter = transform.Find("Good").GetComponent<Image>();
        sizeOfMeter = badMeter.rectTransform.sizeDelta.x;
        intervalUISize = sizeOfMeter / 10;
        halfNumberOfValues = numberOfValues / 2;

        interpolation = 0;

        blinkNoAlphaColor = new Color(blink.color.r, blink.color.g, blink.color.b, 0);
        blink.color = blinkNoAlphaColor;

        soundEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void SetFlowchart(Flowchart chart) {
        flowchart = chart;
        Variable relation = chart.GetVariable("relation");
        if(relation != null) {
            gameObject.SetActive(true);
            lastValue = ((IntegerVariable)relation).Value;

            lastFrameValue = lastValue;
            currentValue = lastValue;
            interpolation = 2;
            float width = (currentValue + halfNumberOfValues) * intervalUISize;
            goodMeter.rectTransform.sizeDelta = new Vector2(width, goodMeter.rectTransform.sizeDelta.y);
        }
        else {
            gameObject.SetActive(false);
        } 
    }

    private void SetRelationshipValue(int value) {
        value =  Mathf.Clamp(value, -halfNumberOfValues, halfNumberOfValues);
       
        if (value != currentValue) {
            if(soundEmitter != null && soundEmitter.Event != null && soundEmitter.Event != "") {
                soundEmitter.Play();
            }
            lastValue = currentValue;
            currentValue = value;
            interpolation = 0;
        }
        else {
            interpolation = 2;
        }

        startTime = Time.realtimeSinceStartup;
    }
	
	// Update is called once per frame
	void Update () {
        if (flowchart != null) {
            
            int relation = flowchart.GetIntegerVariable("relation");

            // if value have just changed
            if (lastFrameValue != relation) {
                SetRelationshipValue(relation);
            }
            lastFrameValue = relation;

            if (interpolation < 1) {
                interpolation += Time.deltaTime * meterFillSpeed;
                
                float newWidth = Mathf.Lerp((lastValue + halfNumberOfValues) * intervalUISize, (currentValue + halfNumberOfValues) * intervalUISize, interpolation);
                Vector2 newSize = new Vector2(newWidth, goodMeter.rectTransform.sizeDelta.y);

                goodMeter.rectTransform.sizeDelta = newSize;
                
                // Blink
                float sinValue = (Time.realtimeSinceStartup - startTime) * blinkSpeed;
                float value = Mathf.Sin(sinValue);
                // only positive for better effect
                value = Mathf.Abs(value);
                value *= blinkIntensity;
                Color newColor = new Color(blink.color.r, blink.color.g, blink.color.b, value);
                blink.color = newColor;
            }
            else if(blink.color != blinkNoAlphaColor) {
                blink.color = blinkNoAlphaColor;
            }
        }
    }
}
