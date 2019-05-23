using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro : MonoBehaviour {

    private VideoPlayer videoPlayer;
    private AsyncOperation loadScene;

    void Start() {
        videoPlayer = GetComponent<VideoPlayer>();

        loadScene = SceneManager.LoadSceneAsync("PlayScene");
        loadScene.allowSceneActivation = false;
        StartCoroutine(AsyncLoadScene());

        videoPlayer.loopPointReached += EndReached;
    }


    void EndReached(VideoPlayer vp) {
        vp.enabled = false;
        // Allow scene load to finish
        loadScene.allowSceneActivation = true;
    }

    IEnumerator AsyncLoadScene() {
        while (!loadScene.isDone) {
            yield return null;
        }
    }

}
