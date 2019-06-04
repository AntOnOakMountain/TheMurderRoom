using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour {

    private bool timeUp = false;

    public enum State {
        Play, Dialogue, Menu, Cutscene
    }

    private static Game game;
    public static Game instance{
        get { return game; }
    }

    [HideInInspector] public Flowchart globalFlowchart;
    [HideInInspector] public int timeLeft;
    private int previousTimeLeft;
    public UnityEvent timeLeftChanged;

    private State state = State.Play;
    public State getState() {
        return state;
    }    

    // Use this for initialization
    void Start () {
        // Init Game singleton
        if (game == null) {
            game = this;
        }
        else {
            Debug.Log("Only one game instance should exist per scene", this);
        }

        globalFlowchart = GameObject.Find("GlobalFungus").GetComponent<Flowchart>();
        timeLeft = globalFlowchart.GetIntegerVariable("time_left");
        previousTimeLeft = timeLeft;
        SetState(State.Play);
    }

    void Update() {
        timeLeft = globalFlowchart.GetIntegerVariable("time_left");
        if(timeLeft != previousTimeLeft) {
            timeLeftChanged.Invoke();
        }
        previousTimeLeft = timeLeft;

        if (!timeUp) {
            if (state == State.Play) {
                 #if (!UNITY_EDITOR)
                    if (Input.GetKey("escape")) {
                        ExitMenu.instance.gameObject.SetActive(true);
                        SetState(State.Menu);
                    }
                 #endif

                if (Input.GetButtonDown("RewindTime")) {
                    RewindTime(false);
                }

                if (timeLeft == 0) {
                    timeUp = true;
                    RewindTime(true);
                }
            }
        }
               
    }

    public void RewindTime(bool isForced) {
        SetState(State.Dialogue);
        DialogWindow.dialogWindow.Clear();
        DialogWindow.dialogWindow.gameObject.SetActive(true);
        UIManager.Instance.relationshipMeter.gameObject.SetActive(false);
        if (isForced) {
            globalFlowchart.ExecuteBlock("Forced Rewind");
        }
        else {
            globalFlowchart.ExecuteBlock("Rewind time");
        }
    }

    public void EndDialogue() {
        DialogWindow.dialogWindow.gameObject.SetActive(false);
        SetState(State.Play);
    }

    public void SetState(State newState) {
        state = newState;

        switch (newState) {
            case State.Dialogue:
                Gramophone.instance.SetParameter(1f);
                Player.instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                UIManager.Instance.quitButton.gameObject.SetActive(false);
                UIManager.Instance.helpButton.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Play:
                Gramophone.instance.SetParameter(0f);
                // Ugly solution for player not being set at Start call
                if (Player.instance.fpCamera.dialogueCamera != null) {
                        
                    Player.instance.fpCamera.dialogueCamera.StopDialogueFocusOn(); // will lead to player entering play state
                }
                else {
                    Player.instance.SetState(Player.State.Play);
                }
                
                UIManager.Instance.timeLockButton.gameObject.SetActive(true);
                UIManager.Instance.quitButton.gameObject.SetActive(true);
                UIManager.Instance.helpButton.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case State.Menu:
                Gramophone.instance.SetParameter(0f);
                Player.instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                UIManager.Instance.quitButton.gameObject.SetActive(false);
                UIManager.Instance.helpButton.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Cutscene:
                Gramophone.instance.SetParameter(0f);
                Player.instance.SetState(Player.State.Cutscene);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                UIManager.Instance.quitButton.gameObject.SetActive(false);
                UIManager.Instance.helpButton.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
