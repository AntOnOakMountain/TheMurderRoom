﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {


    [HideInInspector] public Transform interactPrompt;
    [HideInInspector] public Transform logButton;
    [HideInInspector] public Transform timeLockButton;
    [HideInInspector] public Transform quitButton;
    [HideInInspector] public RelationshipMeter relationshipMeter;
    [HideInInspector] public Text dialogCharacterName;

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
        timeLockButton = transform.Find("TimeLockUI/Time Lock Button");
        quitButton = transform.Find("ButtonHud/Buttons/Quit Game");
        relationshipMeter = transform.Find("DialogWindow/Panel/RelationshipMeter").GetComponent<RelationshipMeter>();
        dialogCharacterName = transform.Find("DialogWindow/Panel/CharacterName").GetComponent<Text>();
    }
}
