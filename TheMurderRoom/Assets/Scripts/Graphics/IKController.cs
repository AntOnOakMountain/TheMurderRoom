using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour {

    private Animator animator;
    private AnimationPlayOnceManager apom;

    [HideInInspector] public Transform head;

    private bool lookAtPlayerActive = false;
    private float lookAmount = 0f;
    public float lookSpeed = 1f;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        apom = GetComponent<AnimationPlayOnceManager>();

        head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    void Update() {
        if (lookAtPlayerActive) {
            lookAmount += lookSpeed * Time.deltaTime;
        }
        else {
            lookAmount -= lookSpeed * Time.deltaTime;
        }
        lookAmount = Mathf.Clamp(lookAmount, 0, 1);
    }

    void OnAnimatorIK(int layerIndex) {
        animator.SetLookAtWeight(lookAmount);
        animator.SetLookAtPosition(Player.instance.GetCameraPosition());
    }

    public void LookAtPlayer() {
        lookAtPlayerActive = true;
    }

    public void StopLookingAtPlayer() {
        lookAtPlayerActive = false;
    }
}

