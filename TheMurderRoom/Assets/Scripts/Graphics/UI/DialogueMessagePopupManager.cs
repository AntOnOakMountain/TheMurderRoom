using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMessagePopupManager : MonoBehaviour {

    private static DialogueMessagePopupManager dmpm;
    public static DialogueMessagePopupManager instance {
        get { return dmpm; }
    }

    private int current = 0;
    private DialogueMessagePopup[] list;

    // Use this for initialization
    void Start () {
        if (dmpm == null) {
            dmpm = this;
        }
        else {
            Debug.Log("Only one instance of DialogueMessagePopupManager should exist in the scene", this);
        }

        list = GetComponentsInChildren<DialogueMessagePopup>();
        current = 0;
        for(int i = 0; i < list.Length; i++) {
            list[i].gameObject.SetActive(false);
        }
        
    }

    public void Display(string text, float duration, float fadeDuration) {
        list[current].Display(text, duration, fadeDuration);
        list[current].transform.SetSiblingIndex(0);
        current++;
        if(current >= list.Length) {
            current = 0;
        }
    }
}
