using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionPlayground : MonoBehaviour {
    private Vector3 _anglesContainer = new Vector3(45, 45, 45);
    public GameObject singleSphere;
    private Quaternion quaternion;
    void Start()
    {
        var rotationMatrix = RotateMath.GetRotationMatrixFromEulerAngles(new Coordinate(_anglesContainer, 0));
        Debug.Log(rotationMatrix);
        var extractedRotationAxis = RotateMath.ExtractRotationAxis(rotationMatrix);
        var extractedAngle = RotateMath.ExtractAngle(rotationMatrix);
        var quaternionCoord = RotateMath.Quaternion(extractedRotationAxis, extractedAngle);
        quaternion = new Quaternion(quaternionCoord.X, quaternionCoord.Y, quaternionCoord.Z, quaternionCoord.W);
        var line = new Line(new Coordinate(0,0,0,1),  extractedRotationAxis.GetAsVector3() * 5);
        line.Draw(0.1f, Color.red);
    }

    private void Update() {
        singleSphere.transform.rotation *= new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }

    void Exercise() {
        // var rotationVector = new Coordinate(1,1,1,0);
        // var quaternion = RotateMath.Quaternion(rotationVector, 1);
        // Debug.Log(quaternion);

        // var quaternionN = Quaternion.AngleAxis(45, new Vector3(2, 1, 5));
        // Debug.Log(quaternionN);

        // foreach (var sphere in spheres) {
        //     sphere.transform.position = RotateMath.RotateByMatrix(new Coordinate(sphere.transform.position, 1), 
        //         new Coordinate(angle, 0)).GetAsVector3();
        //     // sphere.transform.position = RotateMath.RotateByQuaternion(new Coordinate(sphere.transform.position, 1), 
        //     //     new Coordinate(new Vector3(1,0,0), 0), angle).GetAsVector3();
        // }
    }
}
