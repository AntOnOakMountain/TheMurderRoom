// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("TheMurderRoom",
                 "Beat The Game",
                 "You beat the game.")]
    [AddComponentMenu("")]
    public class BeatTheGame : Command {
        protected virtual void CallTheMethod() {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync("OutroScene");
            StartCoroutine(AsyncLoadScene(loadScene));
        }

        public override void OnEnter() {
            CallTheMethod();
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }

        IEnumerator AsyncLoadScene(AsyncOperation loadScene) {
            while (!loadScene.isDone) {
                yield return null;
            }
        }
    }
}