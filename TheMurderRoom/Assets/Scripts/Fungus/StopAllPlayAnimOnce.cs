using UnityEngine;
using UnityEngine.Serialization;

namespace Fungus {
    /// <summary>
    /// Plays an animation once by setting a boolean parameter on an Animator component to true for it's related clips duration"
    /// </summary>
    [CommandInfo("Animation",
                 "Stop all Play Animation Once",
                 "Stop all Play an animation once")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class StopAllPlayAnimOnce : Command {
        [Tooltip("Reference to an Animator component in a game object")]
        // Just used to find the paired AnimationPlayOnceManager object and not need an extra variable type for it
        [SerializeField] protected AnimatorData _animator;

        public override void OnEnter() {
            AnimationPlayOnceManager apom = _animator.Value.GetComponent<AnimationPlayOnceManager>();
            if (apom != null) {
                apom.StopAll();
            }
            else {
                Debug.Log("Object do not have an AnimationPlayOnceManager attached to it", _animator);
            }

            Continue();
        }

        public override string GetSummary() {
            if (_animator.Value == null) {
                return "Error: No animator selected";
            }

            return _animator.Value.name;
        }

        public override Color GetButtonColor() {
            return new Color32(170, 204, 169, 255);
        }
    }
}

