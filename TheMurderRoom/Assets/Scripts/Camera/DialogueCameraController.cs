using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCameraController : MonoBehaviour {

    private bool isInDialogue = false;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 goalPosition;
    private Quaternion goalRotation;
    private float lerpAmount = 2;

    [Tooltip("Speed of going to dialogue view")]
    public float focusSpeed = 0.2f;
    [Tooltip("How far to the side of a npc should the camera focus on during dialogue.")]
    public float npcLookAtOffset = 0.55f;
    [Tooltip("Distance to the npc to go to when talking to a npc.")]
    public float distanceToNpc = 2f;

    void Start() {
        // Set the lerp values as "done" (over 1)
        lerpAmount = 2;
    }


    void Update() {
        if (isInDialogue && lerpAmount < 1f) {
            lerpAmount += focusSpeed * Time.deltaTime;
            
            Player.instance.transform.position = Vector3.Slerp(startingPosition, goalPosition, lerpAmount);
            transform.rotation = Quaternion.Slerp(startingRotation, goalRotation, lerpAmount);
        }
        // Go back to previous position and rotation if not in dialogue
        else if (!isInDialogue && lerpAmount < 1f){

            lerpAmount += focusSpeed * Time.deltaTime;
            Player.instance.transform.position = Vector3.Slerp(goalPosition, startingPosition, lerpAmount);
            transform.rotation = Quaternion.Slerp(goalRotation, startingRotation, lerpAmount);

            // When done set player back to playState if game is in playState
            if (lerpAmount >= 1f && Game.instance.getState() == Game.State.Play) {
                Player.instance.SetState(Player.State.Play);
            }
        }
    }

    public void StartDialogueFocusOn(Transform point) {
        if (point != null) {
            isInDialogue = true;
            lerpAmount = 0;

            // Calculate goal position
            Vector3 distance = Player.instance.transform.position - point.position;
            distance.Set(distance.x, 0, distance.z); // don't count in y axis for distance
            goalPosition = point.position + (distance.normalized * distanceToNpc);
            goalPosition.Set(goalPosition.x, Player.instance.transform.position.y, goalPosition.z); // use players y position and not focus points

            // calculate the actual point to look at
            Vector3 right = Vector3.Cross(distance, Vector3.up);

            Vector3 lookAtPoint = point.position + (right * npcLookAtOffset);
            Vector3 direction = (lookAtPoint - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            goalRotation = Quaternion.LookRotation(direction);

            startingPosition = Player.instance.transform.position;
            startingRotation = transform.rotation;
        }
    }

    public void StopDialogueFocusOn() {
        if(isInDialogue) {
            isInDialogue = false;
            lerpAmount = 0;

            // if you start talking to a npc's when looking at its feet it feel odd to go back to that camera rotation afterwards, so remove all up and down rotation
            startingRotation = Quaternion.Euler(0, startingRotation.eulerAngles.y, 0);
        }
        else {
            Player.instance.SetState(Player.State.Play);
        }
    }
}
