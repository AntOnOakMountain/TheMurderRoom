using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    void Start() {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
