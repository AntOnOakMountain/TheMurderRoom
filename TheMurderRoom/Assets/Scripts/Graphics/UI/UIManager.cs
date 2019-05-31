using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {


    [HideInInspector] public Transform interactPrompt;
    [HideInInspector] public Transform logButton;
    [HideInInspector] public Transform timeLockButton;
    [HideInInspector] public Transform quitButton;
    [HideInInspector] public Transform helpButton;
    [HideInInspector] public Transform helpWindow;
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
        quitButton = transform.Find("ButtonHud/Buttons/Quit Game Button");
        helpButton = transform.Find("ButtonHud/Buttons/Help Button");
        helpButton.GetComponent<Button>().onClick.AddListener(ToggleHelpMenu);
        helpWindow = transform.Find("Help");
        helpWindow.gameObject.SetActive(false);

        relationshipMeter = transform.Find("DialogWindow/Panel/RelationshipMeter").GetComponent<RelationshipMeter>();
        dialogCharacterName = transform.Find("DialogWindow/Panel/CharacterName").GetComponent<Text>();
    }

    void Update() {
        if (Input.GetButtonDown("Help")) {
            ToggleHelpMenu();
        }
    }

    public void ToggleHelpMenu() {
        // if inactive
        if (Game.instance.getState() == Game.State.Play && !UIManager.Instance.helpWindow.gameObject.activeSelf) {
            
            Game.instance.SetState(Game.State.Menu);
            UIManager.Instance.helpButton.gameObject.SetActive(true);
            UIManager.Instance.helpWindow.gameObject.SetActive(true);
            
        }
        // if active
        else if(Game.instance.getState() == Game.State.Menu && UIManager.Instance.helpWindow.gameObject.activeSelf){
            Game.instance.SetState(Game.State.Play);
            UIManager.Instance.helpWindow.gameObject.SetActive(false);
        }
    }
}
