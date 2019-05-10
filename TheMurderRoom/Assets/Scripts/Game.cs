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
                RewindTime();
            }

            if (flowChart.GetIntegerVariable("time_left") == 0) {
                SetState(State.Dialogue);
                Journal.journal.Clear();
                Journal.journal.ToggleJournal(true);
                flowChart.ExecuteBlock("Forced Rewind");
            }
        }        
    }

    public void RewindTime() {
        if (state == State.Play) {
            SetState(State.Dialogue);
            Journal.journal.Clear();
            Journal.journal.ToggleJournal(true);
            flowChart.ExecuteBlock("Rewind time");
        }
    }

    public void EndDialogue() {
        Player.instance.EndDialogue();
    }

    public void SetState(State newState) {
        state = newState;

        if(newState == State.Play) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }

        switch (newState) {
            case State.Dialogue:
                Player.instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                break;
            case State.Play:
                Player.instance.SetState(Player.State.Play);
                UIManager.Instance.timeLockButton.gameObject.SetActive(true);
                break;
            case State.Menu:
                Player.instance.SetState(Player.State.Dialogue);
                break;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
