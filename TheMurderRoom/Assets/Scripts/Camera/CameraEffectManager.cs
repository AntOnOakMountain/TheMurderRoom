using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour {

    private List<CameraEffect> cameraEffects;

    void Start() {
        cameraEffects = new List<CameraEffect>(transform.GetComponents<CameraEffect>());
    }

	void Update () {
        Vector3 positionOffset = new Vector3(0, 0, 0);
        Vector3 rotationOffset = new Vector3(0, 0, 0);
        foreach (CameraEffect effect in cameraEffects) {
            positionOffset += effect.PositionOffset;
            rotationOffset += effect.RotationOffset;
        }


        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
	}
}
