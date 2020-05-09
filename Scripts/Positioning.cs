using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioning : MonoBehaviour {

    public Transform lineStart;
    public Transform lineEnd;
    public Line line;

    public Transform planeOrigin; 
    public Transform planeEdgeV; 
    public Transform planeEdgeU;
    public Plane plane;
    
    [SerializeField]
    private float lineLerpParam = 0.5f;
    
    [SerializeField]
    private float planeLerpV = 0.5f;
    
    [SerializeField]
    private float planeLerpU = 0.5f;
    
    void Start()
    {
        line = new Line( new Coordinate(lineStart.position), new Coordinate(lineEnd.position), Line.LineType.Segment);
        plane = new Plane(new Coordinate(planeOrigin.position), new Coordinate(planeEdgeV.position), new Coordinate(planeEdgeU.position));
        for (float v = 0; v < 1; v += 0.1f) {
            for (float u = 0; u < 1; u += 0.1f) {
                var sphere = CreateSphere(Color.yellow);
                sphere.transform.position = plane.Lerp(v, u).GetAsVector3();
            }
        }
    }

    public static GameObject CreateSphere(Color color) {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = color;
        sphere.GetComponent<Renderer>().material = redMaterial;
        return sphere;
    }

    void FixedUpdate() {
        //transform.position = line.Lerp(lineLerpParam).GetAsVector3();
        transform.position = plane.Lerp(planeLerpV, planeLerpU).GetAsVector3();
    }
}
