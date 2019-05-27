using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour {

    public static DialogWindow dialogWindow;

    public GameObject sayLogEntry;
    public GameObject menuLogEntry;

    public GameObject log;

    public ScrollRect scrollView;
    public float scrollSpeed = 20000;

    private bool isScrollingTowardsBottom = false;

    void Start() {
        dialogWindow = this;
        gameObject.SetActive(false);
    }

    void Update() {
        
        if (isScrollingTowardsBottom) {
            // if scrolling is at bottom, stop it
            if (scrollView.verticalNormalizedPosition - Mathf.Epsilon <= 0) {
                CancelScrollToBottom();
            }
            scrollView.velocity = new Vector3(0, scrollSpeed * Time.deltaTime);
        }
    }

    public void AddLogEntry(string text, bool isMenuLog) {
        GameObject newEntry;
        if (isMenuLog) {
            newEntry = Instantiate(menuLogEntry);
        }
        else {
            newEntry = Instantiate(sayLogEntry);
        }
        newEntry.GetComponentInChildren<Text>().text = text;
        newEntry.transform.SetParent(log.transform, false);
        ScrollToBottom();
    }

    public void ScrollToBottom() {
        isScrollingTowardsBottom = true;
    }

    public void CancelScrollToBottom() {
        scrollView.velocity = new Vector3(0, 0);
        isScrollingTowardsBottom = false;
    }

    public void Clear() {
        foreach (Transform child in log.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
