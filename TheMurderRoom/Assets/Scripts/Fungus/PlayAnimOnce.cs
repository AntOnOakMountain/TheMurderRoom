using UnityEngine;
using UnityEngine.Serialization;

namespace Fungus {
    /// <summary>
    /// Plays an animation once by setting a boolean parameter on an Animator component to true for it's related clips duration"
    /// </summary>
    [CommandInfo("Animation",
                 "Play Animation Once",
                 "Play an animation once")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class PlayAnimOnce : Command {
        [Tooltip("Reference to an Animator component in a game object")]
        // Just used to find the paired AnimationPlayOnceManager object and not need an extra variable type for it
        [SerializeField] protected AnimatorData _animator;

        [Tooltip("Name of the boolean Animator parameter")]
        [SerializeField] protected StringData _boolName;

        [Tooltip("Name of the animation clip")]
        [SerializeField] protected StringData _clipName;

        public override void OnEnter() {
            AnimationPlayOnceManager apom = _animator.Value.GetComponent<AnimationPlayOnceManager>();
            if (apom != null) {
                apom.PlayOnce(_boolName.Value, _clipName.Value);
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
            else if (_clipName.Value == null || _clipName.Value == "") {
                return "Error: No clip name specified";
            }

            return _animator.Value.name + " (" + _boolName.Value + ")";
        }

        public override Color GetButtonColor() {
            return new Color32(170, 204, 169, 255);
        }
    }
}
