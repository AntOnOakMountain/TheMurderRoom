// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("Scripting",
                 "End Dialogue",
                 "Signals the game that the dialogue ended.")]
    [AddComponentMenu("")]
    public class EndDialogue : Command {
        [Tooltip("Delay (in seconds) before the method will be called")]
        [SerializeField] protected float delay;

        protected virtual void CallTheMethod() {
            Game.Instance.EndDialogue();
        }

        public override void OnEnter() {
            if (Mathf.Approximately(delay, 0f)) {
                CallTheMethod();
            }
            else {
                Invoke("CallTheMethod", delay);
            }

            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }
    }
}