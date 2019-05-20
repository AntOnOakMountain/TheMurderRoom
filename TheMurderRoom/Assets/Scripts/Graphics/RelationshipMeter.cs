using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class RelationshipMeter : MonoBehaviour {

    private Flowchart flowchart;

    public float meterFillSpeed = 32f;
    public float sizeOfMeter = 320;
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
        intervalUISize = sizeOfMeter / 10;
        halfNumberOfValues = numberOfValues / 2;
        badMeter = transform.Find("Bad").GetComponent<Image>();
        goodMeter = transform.Find("Good").GetComponent<Image>();

        interpolation = 0;
    }

    public void SetFlowchart(Flowchart chart) {
        flowchart = chart;
        lastValue = chart.GetIntegerVariable("relation");

        lastFrameValue = lastValue;
        currentValue = lastValue;
        SetRelationshipValue(lastValue);
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
