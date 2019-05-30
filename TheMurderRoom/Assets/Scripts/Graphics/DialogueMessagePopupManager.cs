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
        current = list.Length - 1;
    }

    public void Display(string text, float duration, float fadeDuration) {
        list[current].Display(text, duration, fadeDuration);
        current--;
        if(current < 0) {
            current = list.Length - 1;
        }
    }
}
