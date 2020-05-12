using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformations : MonoBehaviour {
    
    public GameObject[] points;
    public Transform center;
    
    public Vector3 translation = new Vector3(1, 1, 2);
    public Vector3 scaling = new Vector3(0, 0, 0);
    public Vector3 angles = new Vector3(0,0,0);

    void Start()
    {
        angles = Mathf.Deg2Rad * angles;
        foreach (var point in points) {
            //point.transform.position = RotateMath.TranslateByMatrix( new Coordinate(point.transform.position, 1),
              //  new Coordinate(translation, 0)).GetAsVector3();
              //point.transform.position -= center.position;
              //point.transform.position = RotateMath.ScaleByMatrix(new Coordinate(point.transform.position, 1),
                //new Coordinate(scaling, 0)).GetAsVector3();
              //point.transform.position = RotateMath.RotateByMatrix(new Coordinate(point.transform.position, 1), 
                    //new Coordinate(angles, 0)).GetAsVector3();
              //point.transform.position = RotateMath.Shear(new Coordinate(point.transform.position), 1).GetAsVector3();
              //point.transform.position = RotateMath.Reflection(new Coordinate(point.transform.position)).GetAsVector3();
              //point.transform.position += center.position;
        }
        DrawLines();
    }

    private void DrawLines() {
        Coordinate.DrawLine(new Coordinate(points[0].transform.position), new Coordinate(points[1].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[1].transform.position), new Coordinate(points[3].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[2].transform.position), new Coordinate(points[3].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[2].transform.position), new Coordinate(points[0].transform.position), 0.02f,Color.yellow);
        
        Coordinate.DrawLine(new Coordinate(points[0].transform.position), new Coordinate(points[4].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[1].transform.position), new Coordinate(points[5].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[2].transform.position), new Coordinate(points[6].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[3].transform.position), new Coordinate(points[7].transform.position), 0.02f,Color.yellow);
        
        Coordinate.DrawLine(new Coordinate(points[4].transform.position), new Coordinate(points[5].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[5].transform.position), new Coordinate(points[7].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[7].transform.position), new Coordinate(points[6].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[6].transform.position), new Coordinate(points[4].transform.position), 0.02f,Color.yellow);
        
        Coordinate.DrawLine(new Coordinate(points[4].transform.position), new Coordinate(points[9].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[6].transform.position), new Coordinate(points[9].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[5].transform.position), new Coordinate(points[8].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[7].transform.position), new Coordinate(points[8].transform.position), 0.02f,Color.yellow);
        Coordinate.DrawLine(new Coordinate(points[8].transform.position), new Coordinate(points[9].transform.position), 0.02f,Color.yellow);
    }
    // rotation and scaling with vertices will be implemented
}
