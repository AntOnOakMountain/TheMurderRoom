using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public enum State {
        Play, Dialogue
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
        state = State.Play;
    }

    void Update() {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }

        if(state == State.Play) {
            if (Input.GetButtonDown("RewindTime")) {
                RewindTime();
            }

            if (flowChart.GetIntegerVariable("time_left") == 0) {
                SetState(State.Dialogue);
                flowChart.ExecuteBlock("Forced Rewind");
            }
        }        
    }

    public void RewindTime() {
        if (state == State.Play) {
            SetState(State.Dialogue);
            flowChart.ExecuteBlock("Rewind time");
        }
    }

    public void EndDialogue() {
        Player.Instance.EndDialogue();
    }

    public void SetState(State newState) {
        state = newState;
        switch (newState) {
            case State.Dialogue:
                Player.Instance.SetState(Player.State.Dialogue);
                UIManager.Instance.timeLockButton.gameObject.SetActive(false);
                break;
            case State.Play:
                Player.Instance.SetState(Player.State.Play);
                UIManager.Instance.timeLockButton.gameObject.SetActive(true);
                break;
        }
    }
}
