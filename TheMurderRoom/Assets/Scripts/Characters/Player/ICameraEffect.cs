using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICameraEffect : MonoBehaviour{

    protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    protected Vector3 positionOffset = new Vector3(0, 0, 0);

    public Vector3 PositionOffset{
        get { return positionOffset; }
    }
    public Vector3 RotationOffset{
        get { return rotationOffset; }
    }
}
