using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public enum State {
        Play, Dialogue, Menu
    }

    private static Game game;
    public static Game instance{
        get { return game; }
    }

    [HideInInspector] public Flowchart globalFlowchart;


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
        SetState(State.Play);
    }

    void Update() {
        #if (!UNITY_EDITOR)
            if (Input.GetKey("escape")) {
                ExitMenu.instance.gameObject.SetActive(true);
                SetState(State.Menu);
            }
        #endif

        if (state == State.Play) {
            if (Input.GetButtonDown("RewindTime")) {
                RewindTime(false);
            }

            if (globalFlowchart.GetIntegerVariable("time_left") == 0) {
                RewindTime(true);
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
                Player.instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                UIManager.Instance.quitButton.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case State.Play:
                // Ugly solution for dialogueCamera not being set when this is run st setup
                if(Player.instance.fpCamera.dialogueCamera != null) {
                    Player.instance.fpCamera.dialogueCamera.StopDialogueFocusOn(); // will lead to player entering play state
                }
                else {
                    Player.instance.SetState(Player.State.Play);
                }
                
                UIManager.Instance.timeLockButton.gameObject.SetActive(true);
                UIManager.Instance.quitButton.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case State.Menu:
                Player.instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                UIManager.Instance.quitButton.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
