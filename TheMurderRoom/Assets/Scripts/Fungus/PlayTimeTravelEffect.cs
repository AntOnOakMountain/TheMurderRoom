// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("TheMurderRoom",
                 "Play Time Travel Effect",
                 "")]
    [AddComponentMenu("")]
    public class PlayTimeTravelEffect : Command {

        [Tooltip("If the effect should play in reverse")]
        [SerializeField] protected bool reverse;

        protected virtual void CallTheMethod() {

            TimeTravelLightEffect.instance.Activate(reverse);
        }

        public override void OnEnter() {
            CallTheMethod();
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }
    }
}