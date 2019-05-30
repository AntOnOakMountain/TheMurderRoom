using UnityEngine;

namespace Fungus {
    /// <summary>
    /// Signals the game that the dialogue ends. 
    /// </summary>
    [CommandInfo("TheMurderRoom",
                 "Set Name",
                 "Set a name to display for dialogue.")]
    [AddComponentMenu("")]
    public class SetName : Command {
        [Tooltip("Name to set")]
        [SerializeField] protected new string name;

        protected virtual void CallTheMethod() {
            UIManager.Instance.dialogCharacterName.text = name;
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