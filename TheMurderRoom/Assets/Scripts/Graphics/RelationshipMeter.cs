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

    private Image badMeter;
    private Image goodMeter;

	// Use this for initialization
	void Start () {
        
        badMeter = transform.Find("Bad").GetComponent<Image>();
        goodMeter = transform.Find("Good").GetComponent<Image>();
        sizeOfMeter = badMeter.rectTransform.sizeDelta.x;
        intervalUISize = sizeOfMeter / 10;
        halfNumberOfValues = numberOfValues / 2;

        interpolation = 0;
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
            lastValue = currentValue;
            currentValue = value;
            interpolation = 0;
        }
        else {
            interpolation = 2;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (flowchart != null) {
            
            int value = flowchart.GetIntegerVariable("relation");
            // Value have just changed
            
            if (lastFrameValue != value) {
                SetRelationshipValue(value);
            }
            lastFrameValue = value;

            if (interpolation < 1) {
                interpolation += Time.deltaTime * meterFillSpeed;
                
                float newWidth = Mathf.Lerp((lastValue + halfNumberOfValues) * intervalUISize, (currentValue + halfNumberOfValues) * intervalUISize, interpolation);
                Vector2 newSize = new Vector2(newWidth, goodMeter.rectTransform.sizeDelta.y);

                goodMeter.rectTransform.sizeDelta = newSize;
            }
        }
    }
}
