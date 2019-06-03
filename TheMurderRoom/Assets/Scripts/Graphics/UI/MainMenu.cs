using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button startButton;
    public Button creditsButton;
    public Button quitButton;

    // Use this for initialization
    void Start () {
        startButton.onClick.AddListener(StartButtonPressed);
        creditsButton.onClick.AddListener(CreditsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    void StartButtonPressed() {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("IntroScene");
        StartCoroutine(AsyncLoadScene(loadScene));
    }

    void QuitButtonPressed() {
        Application.Quit();
    }

    void CreditsButtonPressed() {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("OutroScene");
        StartCoroutine(AsyncLoadScene(loadScene));
    }

    IEnumerator AsyncLoadScene(AsyncOperation loadScene) {
        while (!loadScene.isDone) {
            yield return null;
        }
    }
}
