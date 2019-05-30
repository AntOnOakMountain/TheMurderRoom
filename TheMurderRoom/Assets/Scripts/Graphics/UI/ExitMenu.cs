using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour {

    private static ExitMenu exitMenu;
    public static ExitMenu instance {
        get { return exitMenu; }
    }

    private Button yes;
    private Button no;



	void Start () {
        // Init ExitMenu singleton
        if (exitMenu == null) {
            exitMenu = this;
        }
        else {
            Debug.Log("Only one ExitMenu instance should exist per scene", this);
        }
        yes = transform.Find("Buttons/Yes").GetComponent<Button>();
        no = transform.Find("Buttons/No").GetComponent<Button>();
        yes.onClick.AddListener(Yes);
        no.onClick.AddListener(No);
        this.gameObject.SetActive(false);
    }

    private void Yes() {
        Game.instance.ExitGame();
    }

    private void No() {
        exitMenu.gameObject.SetActive(false);
        Game.instance.SetState(Game.State.Play);
    }

    

}
