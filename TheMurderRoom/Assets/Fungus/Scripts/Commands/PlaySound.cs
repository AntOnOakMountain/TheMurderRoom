// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Plays a once-off sound effect. Multiple sound effects can be played at the same time.
    /// </summary>
    [CommandInfo("Audio", 
                 "Play Sound",
                 "Plays a once-off sound effect. Multiple sound effects can be played at the same time.")]
    [AddComponentMenu("")]
    public class PlaySound : Command
    {
        [Tooltip("Sound effect clip to play")]
        [FMODUnity.EventRef]
        [SerializeField] protected string soundClip;

        [Range(0,1)]
        [Tooltip("Volume level of the sound effect")]
        [SerializeField] protected float volume = 1;

        [Tooltip("Wait until the sound has finished playing before continuing execution.")]
        [SerializeField] protected bool waitUntilFinished;

        FMOD.Studio.EventInstance soundEventInstance;

        protected virtual void DoWait()
        {
            Continue();
        }

        #region Public members

        public override void OnEnter()
        {
            if (soundClip == null)
            {
                Continue();
                return;
            }

            soundEventInstance = FMODUnity.RuntimeManager.CreateInstance(soundClip);
            soundEventInstance.start();

            if (waitUntilFinished)
            {
                int length;
                FMOD.RESULT res = FMODUnity.RuntimeManager.GetEventDescription(soundClip).getLength(out length);
                Invoke("DoWait", length / 1000);
            }
            else
            {
                Continue();
            }
        }

        public override string GetSummary()
        {
            if (soundClip == null)
            {
                return "Error: No sound clip selected";
            }

            return soundClip;
        }

        public override Color GetButtonColor()
        {
            return new Color32(242, 209, 176, 255);
        }

        #endregion
    }
}
