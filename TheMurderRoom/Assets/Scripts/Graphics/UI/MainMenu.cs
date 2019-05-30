using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button startButton;
    public Button quitButton;

    private AsyncOperation loadScene;
    // Use this for initialization
    void Start () {
        startButton.onClick.AddListener(StartButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        loadScene = SceneManager.LoadSceneAsync("IntroScene");
        loadScene.allowSceneActivation = false;
        StartCoroutine(AsyncLoadScene());
    }

    void StartButtonPressed() {
        loadScene.allowSceneActivation = true;
    }

    void QuitButtonPressed() {
        Application.Quit();
    }

    IEnumerator AsyncLoadScene() {
        while (!loadScene.isDone) {
            yield return null;
        }
    }
}
