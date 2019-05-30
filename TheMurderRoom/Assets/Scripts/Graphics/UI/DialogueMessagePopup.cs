using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMessagePopup : MonoBehaviour {

    [HideInInspector] public Image image;
    [HideInInspector] public Text text;

    private Timer durationTimer;
    private Timer fadeDurationTimer;


    private bool fadingIn = true;

	// Use this for initialization
	void Start () {
        image = this.GetComponent<Image>();
        text = transform.GetComponentInChildren<Text>();

        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(fadeDurationTimer.IsActive() && fadingIn) {
            SetAlpha(Mathf.Clamp(fadeDurationTimer.TimePercentagePassed(), 0, 1));
            if (fadeDurationTimer.IsDone()) {
                fadeDurationTimer.Pause();
                fadingIn = false;
                durationTimer.Start();
            }
        }
        else if (durationTimer.IsActive()) {
            if (durationTimer.IsDone()) {
                
                durationTimer.Pause();
                fadeDurationTimer.Start();
            }
        }
        // fade out
        else if (fadeDurationTimer.IsActive() && !fadingIn) {
            SetAlpha(Mathf.Clamp(1 - fadeDurationTimer.TimePercentagePassed(), 0, 1));
            if (fadeDurationTimer.IsDone()) {
                fadeDurationTimer.Pause();
                this.gameObject.SetActive(false);
            }
        }
	}

    public void Display(string text, float duration, float fadeDuration) {
        durationTimer = new Timer(duration);
        fadeDurationTimer = new Timer(fadeDuration);

        fadingIn = true;
        this.text.text = text;
        fadeDurationTimer.Start();
        this.gameObject.SetActive(true);

        SetAlpha(0);
    }

    private void SetAlpha(float a) {
        image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        text.color = new Color(text.color.r, text.color.g, text.color.b, a);
    }

}
