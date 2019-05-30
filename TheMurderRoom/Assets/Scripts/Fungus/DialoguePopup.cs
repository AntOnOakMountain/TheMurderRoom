using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("TheMurderRoom",
                 "Dialogue popup",
                 "Displays a popup message.")]
    [AddComponentMenu("")]
    public class DialoguePopup : Command {
        [Tooltip("The text to display")]
        [TextArea(5, 10)]
        [SerializeField] protected string text = "";

        [Tooltip("How log the message will be displayed at full strenght")]
        [SerializeField] protected float duration = 5;

        [Tooltip("How long it takes for the message to fade in and out")]
        [SerializeField] protected float fadeDuration = 1;

        protected virtual void CallTheMethod() {
            DialogueMessagePopupManager.instance.Display(text, duration, fadeDuration);
        }

        public override void OnEnter() {
            if(duration <= 0) {
                Debug.Log("no duration for popup", this);
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
