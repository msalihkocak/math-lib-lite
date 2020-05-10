using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectLinePlane : MonoBehaviour {
    
    public Transform lineEnd;
    public Transform planeOrigin;
    public Transform planeVectorV;
    public Transform planeVectorU;

    private Line _line;
    private Plane _plane;
    
    public GameObject ball;
    private MoveBall _ballMover;
    
    void Start() { 
        _ballMover = ball.GetComponent<MoveBall>();
        
        _line = new Line(new Coordinate(ball.transform.position), lineEnd.position - ball.transform.position);
        _plane = new Plane(new Coordinate(planeOrigin.position), planeVectorV.position - planeOrigin.position, 
            planeVectorU.position - planeOrigin.position);
        _line.Draw(0.1f, Color.cyan);
        PopulatePlaneWithSpheres();
        //ShowIntersectionPoint();
        
        var ballPathSegment = CalculateIntersection();
        if (ballPathSegment == null) return;
        _ballMover.OnIntersectionCalculate(ballPathSegment);
        
        var reflectionLineSegment = ballPathSegment.ReflectsFrom(_plane);
        reflectionLineSegment.Draw(0.1f, Color.magenta);
        _ballMover.OnReflectionCalculate(reflectionLineSegment);
    }

    private Line CalculateIntersection() {
        var intersectionFactor = _line.IntersectsAt(_plane);
        if (!intersectionFactor.HasValue) return null;
        var ballHittingPoint = _line.Lerp(intersectionFactor.Value).GetAsVector3();
        return new Line(new Coordinate(ball.transform.position), ballHittingPoint - ball.transform.position);
    }

    private void ShowIntersectionPoint() {
        var planeIntersectT = _line.IntersectsAt(_plane);
        if (!planeIntersectT.HasValue) return;
        var intersectionPoint = _line.Lerp(planeIntersectT.Value);
        var sphere = CreateSphere(Color.red);
        sphere.transform.position = intersectionPoint.GetAsVector3();
    }

    void PopulatePlaneWithSpheres() {
        for (float v = 0; v < 1; v += 0.1f) {
            for (float u = 0; u < 1; u += 0.1f) {
                var sphere = CreateSphere(Color.yellow);
                sphere.transform.position = _plane.Lerp(v, u).GetAsVector3();
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
}
