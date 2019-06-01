using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("TheMurderRoom",
                 "Play poison effect",
                 "Plays a poison effect.")]
    [AddComponentMenu("")]
    public class PlayPoisonEffect : Command {

        [Tooltip("What effect")]
        [SerializeField] protected PoisonEffectsManager.Effect effect;

        [Tooltip("How many loops the effect will last for (-1 is infinite)")]
        [Range(-1, 20)]
        [SerializeField] protected int loops = 1;

        protected virtual void CallTheMethod() {
            PoisonEffectsManager.instance.PlayEffect(effect, loops);
        }

        public override void OnEnter() {
            // -1 is infinite so is allowed
            if (loops <= -2) {
                Debug.Log("Needs at least one loop for poison effect", this);
            }
            else {
                CallTheMethod();
            }
            Continue();
        }

        public override Color GetButtonColor() {
            return new Color32(235, 191, 217, 255);
        }
    }
}

