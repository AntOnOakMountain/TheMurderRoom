using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayOnceManager : MonoBehaviour {

    private Animator animator;
    private AnimationClip[] clips;

    private List<AnimationTimer> animTimers;

    public void Start() {
        animator = GetComponent<Animator>();
        clips = animator.runtimeAnimatorController.animationClips;
        animTimers = new List<AnimationTimer>();
    }

    /// <summary>
    /// Play an animation once by setting a bool to true for it's clips duration.
    /// Requires the clip and bool to have the same name.
    /// </summary>
    /// <param name="name"> Name of bool and clip</param>
    public void PlayOnce(string name) {
        PlayOnce(name, name);
    }

    /// <summary>
    /// Play an animation once by setting a bool to true for a clips duration.
    /// </summary>
    public void PlayOnce(string booolName, string clipName) {
        foreach (AnimationClip clip in clips) {
            
            if (clip.name == clipName) {
                animator.SetBool(booolName, true);
                Timer t = new Timer(clip.length);
                t.Start();
                animTimers.Add(new AnimationTimer(t, booolName));
                return;
            }
        }
        Debug.Log("No clip named: " + clipName + " found in ", animator);
    }


    public void Update() {
        for(int i = animTimers.Count-1; i >= 0; i--) {
            if (animTimers[i].timer.IsDone()) {
                Stop(i);
            }
        }
    }

    public void StopAll() {
        for (int i = animTimers.Count - 1; i >= 0; i--) {
            Stop(i);
        }
    }

    private void Stop(int animTimerIndex) {
        animator.SetBool(animTimers[animTimerIndex].boolName, false);
        animTimers.RemoveAt(animTimerIndex);
    }


    private class AnimationTimer {
        public Timer timer;
        public string boolName;

        public AnimationTimer(Timer timer, string name) {
            this.timer = timer;
            this.boolName = name;
        }
    }
}
