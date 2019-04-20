using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private static Game game;
    public static Game instance{
        get { return game; }
    }

    private Flowchart flowChart;

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
    }

    void Update() {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
        if (Input.GetButtonDown("RewindTime"))
            flowChart.ExecuteBlock("Rewind time");
    }

    public void EndDialogue() {
        Player.instance.EndDialogue();
    }
}
