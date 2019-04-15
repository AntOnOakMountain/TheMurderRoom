using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private static Game game;
    public static Game Instance{
        get { return game; }
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
    }

    void Update() {
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
    }

    public void EndDialogue() {
        Player.Instance.EndDialogue();
    }
}
