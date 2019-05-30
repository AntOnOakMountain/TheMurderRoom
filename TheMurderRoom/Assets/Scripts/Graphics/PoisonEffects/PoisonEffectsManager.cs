using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffectsManager : MonoBehaviour {

    public enum Effect{
        ChromaticAberration,
        Contrast,
        DepthOfField,
        LensDistortion,
        Vignette,
        All
    }


    private static PoisonEffectsManager pem;
    public static PoisonEffectsManager instance {
        get { return pem; }
    }

    private ChromaticAberrationPoisonEffect chromaticAberration;
    private ContrastPoisonEffect contrast;
    private DepthOfFieldPoisonEffect depthOfField;
    private LensDistortionPoisonEffect lensDistortion;
    private VignettePoisonEffect vignette;

    private Timer timer;
    private EffectSetting selectedEffect;

    public TimeTickSetting fourTicksLeft;
    public TimeTickSetting threeTicksLeft;
    public TimeTickSetting twoTicksLeft;
    public TimeTickSetting oneTicksLeft;

    // Use this for initialization
    void Start () {
        if (pem == null) {
            pem = this;
        }
        else {
            Debug.Log("Only one instance of PoisonEffectsManager should exist in the scene", this);
        }

        chromaticAberration = GetComponent<ChromaticAberrationPoisonEffect>();
        contrast = GetComponent<ContrastPoisonEffect>();
        depthOfField = GetComponent<DepthOfFieldPoisonEffect>();
        lensDistortion = GetComponent<LensDistortionPoisonEffect>();
        vignette = GetComponent<VignettePoisonEffect>();
        Game.instance.timeLeftChanged.AddListener(TimeLeftChanged);
        RandomizeEffect();
    }
	
	public void PlayEffect(Effect effect, int loops) {
        switch (effect){
            case Effect.ChromaticAberration:
                chromaticAberration.Activate(loops);
                break;
            case Effect.Contrast:
                contrast.Activate(loops);
                break;
            case Effect.DepthOfField:
                depthOfField.Activate(loops);
                break;
            case Effect.LensDistortion:
                lensDistortion.Activate(loops);
                break;
            case Effect.Vignette:
                vignette.Activate(loops);
                break;
            case Effect.All:
                chromaticAberration.Activate(loops);
                contrast.Activate(loops);
                depthOfField.Activate(loops);
                lensDistortion.Activate(loops);
                vignette.Activate(loops * 2);
                break;
        }
    }

    void Update() {
        if(timer.IsActive() && timer.IsDone()) {
            PlayEffect(selectedEffect.effect, selectedEffect.loops);
            RandomizeEffect();
        }
    }

    private void TimeLeftChanged() {
        timer.InstantFinish();
    }

    private void RandomizeEffect() {
        TimeTickSetting setting = null;

        switch (Game.instance.timeLeft) {
            case 4:
                setting = fourTicksLeft;
                break;
            case 3:
                setting = threeTicksLeft;
                break;
            case 2:
                setting = twoTicksLeft;
                break;
            case 1:
                setting = oneTicksLeft;
                break;
        }

        if(setting != null && setting.effectSettings.Length > 0) {
            float time = Random.Range(setting.minTime, setting.maxTime);
            int effectIndex = Random.Range(0, setting.effectSettings.Length - 1);
            selectedEffect = setting.effectSettings[effectIndex];
            timer = new Timer(time);
            timer.Start();
        }
        else {
            timer.Pause();
        }
    }

    [System.Serializable]
    public class TimeTickSetting {
        [Tooltip("Least amount of time needed to pass for an effect to trigger.")]
        public float minTime = 60;
        [Tooltip("Max amount of time needed to pass for an effect to trigger.")]
        public float maxTime = 120;
        public EffectSetting[] effectSettings;
    }


    [System.Serializable]
    public class EffectSetting {
        public Effect effect;
        [Range(1, 20)]
        public int loops = 2;
    }
}
