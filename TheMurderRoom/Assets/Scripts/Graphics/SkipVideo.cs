using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkipVideo : MonoBehaviour {

    public RectTransform panel;
    public RectTransform barFill;
    public Text text;

    public float holdDuration = 2f;

    private Timer timer;

    private float barFillMaxSize;

    public static UnityEvent skipVideo = new UnityEvent();

	// Use this for initialization
	void Start () {
        timer = new Timer(holdDuration);

        barFillMaxSize = barFill.sizeDelta.x;

        Vector2 newSize = new Vector2(0, barFill.sizeDelta.y);
        barFill.sizeDelta = newSize;

        panel.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel")) {
            timer.Start();
            panel.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            Vector2 newSize = new Vector2(0, barFill.sizeDelta.y);
            barFill.sizeDelta = newSize;
        }
        if (Input.GetButtonUp("Cancel")) {
            timer.Pause();
            panel.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
        if (timer.IsActive()) {
            float newWidth = Mathf.Lerp(0, barFillMaxSize, timer.TimePercentagePassed());
            Vector2 newSize = new Vector2(newWidth, barFill.sizeDelta.y);
            barFill.sizeDelta = newSize;

            if (timer.IsDone()) {
                skipVideo.Invoke();
            }
        }
	}
}
