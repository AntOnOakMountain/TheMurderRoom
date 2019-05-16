using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCameraController : MonoBehaviour {

    private bool isInDialogue = false;
    private Vector3 focusPoint;
    private Vector3 defaultLocalPosition;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private Vector3 goalPosition;
    private Quaternion goalRotation;

    private float lerpTo = 1;
    private float lerpFrom = 1;

    [Tooltip("Speed of going to dialogue view")]
    public float focusSpeed = 0.2f;

    [Tooltip("How far to the side of a npc should the camera focus on during dialogue.")]
    public float npcLookAtOffset = 0.92f;

    [Tooltip("Distance to the npc to go to when talking to a npc.")]
    public float distanceToNpc = 2f;

    void Start() {
        // Save original position
        defaultLocalPosition = transform.localPosition;

        // Set the lerp values as "done" (over 1)
        lerpTo = 2;
        lerpFrom = 2;
}


    void Update() {
        if (isInDialogue && lerpTo < 1f) {
            lerpTo += focusSpeed * Time.deltaTime;
            // for a little nicer movement
            float sineLerp = Mathf.Sin(lerpTo * Mathf.PI / 2);
            
            Player.instance.transform.position = Vector3.Lerp(startingPosition, goalPosition, sineLerp);
            transform.rotation = Quaternion.Slerp(startingRotation, goalRotation, sineLerp);
        }
        else if(lerpFrom < 1f){
            // Go back to previous position and rotation
            lerpFrom += focusSpeed * Time.deltaTime;
            // for a little nicer movement
            float sineLerp = Mathf.Sin(lerpFrom * Mathf.PI / 2);

            Player.instance.transform.position = Vector3.Lerp(goalPosition, startingPosition, sineLerp);
            transform.rotation = Quaternion.Slerp(goalRotation, startingRotation, sineLerp);
        }
    }

    public void SetDialogueFocusPoint(Transform point) {
        if (point != null) {
            isInDialogue = true;
            focusPoint = point.position;
            lerpTo = 0;

            // Calculate goal position
            Vector3 distance = Player.instance.transform.position - focusPoint;
            distance.Set(distance.x, 0, distance.z); // don't count in y axis for distance
            goalPosition = focusPoint + (distance.normalized * distanceToNpc);
            goalPosition.Set(goalPosition.x, Player.instance.transform.position.y, goalPosition.z); // don't move in y axis

            // calculate the actual point to look at
            Vector3 right = Vector3.Cross(distance, Vector3.up);

            Vector3 lookAtPoint = focusPoint + (right * npcLookAtOffset);
            Vector3 direction = (lookAtPoint - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            goalRotation = Quaternion.LookRotation(direction);

            startingPosition = Player.instance.transform.position;
            startingRotation = transform.rotation;
        }
    }

    public void NullFocusPoint() {
        if(isInDialogue) {
            isInDialogue = false;
            lerpFrom = 0;

            // if you start talking to a npc's when looking at its feet it feel odd to go back to that camera rotation afterwards, so remove all up and down rotation
            startingRotation = Quaternion.Euler(0, startingRotation.eulerAngles.y, 0);
        }
    }
}
