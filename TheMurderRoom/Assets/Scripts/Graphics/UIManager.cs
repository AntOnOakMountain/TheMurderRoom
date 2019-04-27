using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [HideInInspector]
    public Transform interactPrompt;
    public Transform logButton;
    public Transform timeLockButton;

    private static UIManager uiManager;
    public static UIManager Instance {
        get { return uiManager; }
    }


    public void Start() {
        if(uiManager == null) {
            uiManager = this;
        }
        else {
            Debug.Log("Only one UIManager should exist in the scene", this);
        }

        interactPrompt = transform.Find("InteractPrompt");
        interactPrompt.gameObject.SetActive(false);

        logButton = transform.Find("ButtonHud/Buttons/Log Button");
        timeLockButton = transform.Find("ButtonHud/Buttons/Time Lock Button");
    }
}
