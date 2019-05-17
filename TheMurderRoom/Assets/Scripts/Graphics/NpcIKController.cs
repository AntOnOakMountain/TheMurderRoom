using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIKController : MonoBehaviour {

    private Animator animator;
    private AnimationPlayOnceManager apom;

    [HideInInspector] public Transform head;
    [HideInInspector] public Transform chest;
    [HideInInspector] public Npc npc;

    private bool lookAtPlayerActive = false;
    private float lookAmount = 0f;

    public float rotationSpeed = 1f;
    [Range(0, 1)]
    public float headRotationAmount = 0.45f;
    [Range(0, 1)]
    public float chestRotationAmount = 0.45f;

    [Tooltip("How close the player need to be for npc to turn its head and chest towards the player ")]
    public float lookAtDistance = 3;

    [Tooltip("Maximum angle at which the npc will follow the player with its head and chest ")]
    public float lookAtMaxAngle = 60;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        apom = GetComponent<AnimationPlayOnceManager>();

        head = animator.GetBoneTransform(HumanBodyBones.Head);
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        npc = GetComponentInParent<Npc>();
    }

    void Update() {
        Vector3 playerDistance = Player.instance.fpCamera.transform.position - npc.transform.position;

        Vector3 nForward = npc.transform.forward;

        if (Vector3.Angle(nForward, playerDistance) < lookAtMaxAngle &&
            playerDistance.sqrMagnitude < Mathf.Pow(lookAtDistance, 2)) {
            LookAtPlayer();
        }
        else {
            StopLookingAtPlayer();
        }

        if (lookAtPlayerActive) {
            lookAmount += rotationSpeed * Time.deltaTime;
        }
        else {
            lookAmount -= rotationSpeed * Time.deltaTime;
        }
        lookAmount = Mathf.Clamp(lookAmount, 0, 1);
    }

    void OnAnimatorIK(int layerIndex) {
        Vector3 headToPlayerV = Player.instance.fpCamera.transform.position - head.position;

      // Head
        // create a direction beetween direct look at and looking forward
        Vector3 newHeadDir = Vector3.Slerp(headToPlayerV, npc.transform.forward, headRotationAmount * lookAmount);
        // Get how much you need to rotate to reach that direction
        Quaternion headRotationQuaternion = Quaternion.FromToRotation(newHeadDir, headToPlayerV);
        // Rotate it by that much (will still keep other animations effect since it rotates by a set amount instead of overriding)
        head.rotation = headRotationQuaternion * head.rotation;
        animator.SetBoneLocalRotation(HumanBodyBones.Head, head.localRotation);

        // Chest
        // create a direction beetween direct look at and looking forward
        Vector3 newChestDir = Vector3.Slerp(headToPlayerV, npc.transform.forward, chestRotationAmount * lookAmount);
        // Get how much you need to rotate to reach that direction
        Quaternion chestRotationQuaternion = Quaternion.FromToRotation(newChestDir, headToPlayerV);
        // Rotate it by that much (will still keep other animations effect since it rotates by a set amount instead of overriding)
        chest.rotation = chestRotationQuaternion * chest.rotation;
        animator.SetBoneLocalRotation(HumanBodyBones.Chest, chest.localRotation);
        
        /*
         * // normal IK
        animator.SetLookAtWeight(lookAmount);
        animator.SetLookAtPosition(Player.instance.GetCameraPosition());
        */
    }

    public void LookAtPlayer() {
        lookAtPlayerActive = true;
    }

    public void StopLookingAtPlayer() {
        lookAtPlayerActive = false;
    }
}

