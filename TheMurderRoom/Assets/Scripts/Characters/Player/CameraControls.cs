using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

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

    void Update() {
        // Calculate new rotation values
        float rotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivityX * Time.deltaTime;
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityY * Time.deltaTime;

        // XAxis rotation clamping
        rotationX %= 360; // modulus to not go over 360
        if(rotationX < 0) {
            rotationX = 360 + rotationX; // loop around if negative
        }
        if (Math2.IsInRange(rotationX, 180, 360)) { // Looks upwards (negatives can be used as input for Xaxis but it will always store it as a positive)
            rotationX = Mathf.Clamp(rotationX, 360 - maxUpwardsAngle, 360);
        }
        else { // looks downwards
            rotationX = Mathf.Clamp(rotationX, 0, maxDownwardsAngle);
        }
        // YAxis rotation clamping
        rotationY = Mathf.Clamp(rotationY, -maxLeftwardsAngle, maxRightwardsAngle); // y axis support values beetween -360 to 360

        // Apply new rotation
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}
