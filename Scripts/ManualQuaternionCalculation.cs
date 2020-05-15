using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualQuaternionCalculation : MonoBehaviour {
    
    private Coordinate _rotationAxis;
    private float _wAngle;
    
    private void Start() {
        var angleContainer = new Coordinate(1, 1, 1, 0);
        var rotationMatrixFromEulerAngles = RotateMath.GetRotationMatrixFromEulerAngles(angleContainer);
        _wAngle = RotateMath.ExtractAngle(rotationMatrixFromEulerAngles);
        _rotationAxis = RotateMath.ExtractRotationAxis(rotationMatrixFromEulerAngles);
        var rotateLabQuaternion = RotateMath.Quaternion(_rotationAxis, _wAngle);
        var unityQuaternion = Quaternion.AngleAxis(_wAngle, _rotationAxis.GetAsVector3());
        Debug.Log("RotateLab quaternion is: " + rotateLabQuaternion);
        Debug.Log("Unity quaternion is: " + unityQuaternion.x + ", " + unityQuaternion.y + ", " + unityQuaternion.z + ", " + unityQuaternion.w);
    }

    void Update() {
        var quaternion = RotateMath.Quaternion(_rotationAxis, _wAngle);
        transform.rotation *= new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}
