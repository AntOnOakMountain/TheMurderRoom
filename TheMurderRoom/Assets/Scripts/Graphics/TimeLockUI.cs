using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class TimeLockUI : MonoBehaviour {

    public Sprite fourTimeTickLeft;
    public Sprite threeTimeTickLeft;
    public Sprite twoTimeTickLeft;
    public Sprite oneTimeTickLeft;
    public Sprite noTimeTickLeft;

    private Flowchart globalFlowchart;

    private int lastTick = 4;
    private int currentTick = 4;

    private Image uiImage;

    public void Start() {
        uiImage = GetComponent<Image>();
        SetTicksLeft(4);
        globalFlowchart = GameObject.Find("GlobalFungus").GetComponent<Flowchart>();
    }
	
	// Update is called once per frame
	void Update () {
        currentTick = globalFlowchart.GetIntegerVariable("time_left");

        if(lastTick != currentTick) {
            SetTicksLeft(currentTick);
        }
        lastTick = currentTick;
	}

    private void SetTicksLeft(int ticksLeft) {
        switch (ticksLeft) {
            case 4:
                uiImage.sprite = fourTimeTickLeft;
                break;
            case 3:
                uiImage.sprite = threeTimeTickLeft;
                break;
            case 2:
                uiImage.sprite = twoTimeTickLeft;
                break;
            case 1:
                uiImage.sprite = oneTimeTickLeft;
                break;
            case 0:
                uiImage.sprite = noTimeTickLeft;
                break;
        }
    }
}
