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

    private Flowchart flowChart;


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

        flowChart = GameObject.Find("GlobalFungus").GetComponent<Flowchart>();
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

            if (flowChart.GetIntegerVariable("time_left") == 0) {
                RewindTime(true);
            }
        }        
    }

    public void RewindTime(bool isForced) {
        SetState(State.Dialogue);
        Journal.journal.Clear();
        Journal.journal.ToggleJournal(true);
        if (isForced) {
            flowChart.ExecuteBlock("Forced Rewind");
        }
        else {
            flowChart.ExecuteBlock("Rewind time");
        }
    }

    public void EndDialogue() {
        Journal.journal.ToggleJournal(false);
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
                Player.instance.fpCamera.dialogueCamera.StopDialogueFocusOn(); // will lead to player entering play state
                //Player.instance.SetState(Player.State.Play);
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
