using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour {

    [Header("Mouse sensitivity")]
    public float sensitivityX = 300F;
    public float sensitivityY = 300F;

    [Header("Vertical limits")]
    [Range(1, 89)]
    public float maxUpwardsAngle = 60F;
    [Range(1, 89)]
    public float maxDownwardsAngle = 60F;

    [Header("Horizontal limits")]
    [Range(0, 360)]
    public float maxLeftwardsAngle = 360F;
    [Range(0, 360)]
    public float maxRightwardsAngle = 360F;

    private Transform focusPoint;

    [Header("Dialogue focus on character")]
    [Tooltip("Focus Speed when set to focus on a point")]
    public float focusSpeed = 2f;

    [Tooltip("How far to the side of a npc should the camera focus on during dialogue.")]
    public float npcLookAtOffset = 0.92f;


    void Update() {
        if (Player.instance.IsInState(Player.State.Play)) {
            BasicCameraControls();
        }
        FocusOnPoint();
    }


    private void FocusOnPoint() {
        if (focusPoint != null) {
            //find the vector pointing from our position to the target
            Vector3 actualPoint = focusPoint.position + (transform.right * npcLookAtOffset);

            Vector3 direction = (actualPoint - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * focusSpeed);
        }
    }

    private void BasicCameraControls() {
        // Calculate new rotation values
        float rotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivityX * Time.deltaTime;
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityY * Time.deltaTime;

        // Xaxis rotation clamping
        // modulus to not go over 360
        rotationX %= 360; 
        if (rotationX < 0) {
            // The inspector displays the xAxis value as negative but it is stored as a positive. So need to translate a negative xRotation to the positive value with the same angle
            rotationX = rotationX + 360;
        }
        // Looks upwards 
        if (Math2.IsInRange(rotationX, 180, 360)) {
            rotationX = Mathf.Clamp(rotationX, 360 - maxUpwardsAngle, 360);
        }
        // looks downwards
        else {
            rotationX = Mathf.Clamp(rotationX, 0, maxDownwardsAngle);
        }

        // YAxis rotation clamping
        rotationY = Mathf.Clamp(rotationY, -maxLeftwardsAngle, maxRightwardsAngle); // y axis support values beetween -360 to 360

        // Apply new rotation
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    public void SetDialogueFocusPoint(Transform point) {
        if(point != null) {
            focusPoint = point;
        }
    }

    public void NullFocusPoint() {
        focusPoint = null;
    }
}
