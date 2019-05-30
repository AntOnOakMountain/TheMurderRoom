using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoToScene : MonoBehaviour {

    private VideoPlayer videoPlayer;
    private AsyncOperation loadScene;

    public string scene;

    void Start() {
        videoPlayer = GetComponent<VideoPlayer>();

        loadScene = SceneManager.LoadSceneAsync(scene);
        loadScene.allowSceneActivation = false;
        StartCoroutine(AsyncLoadScene());

        videoPlayer.loopPointReached += EndReached;
        SkipVideo.skipVideo.AddListener(SkipIntro);
    }


    void EndReached(VideoPlayer vp) {
        vp.enabled = false;
        SkipIntro();
    }

    void SkipIntro() {
        // Allows scene load to finish
        loadScene.allowSceneActivation = true;
    }

    IEnumerator AsyncLoadScene() {
        while (!loadScene.isDone) {
            yield return null;
        }
    }
}
