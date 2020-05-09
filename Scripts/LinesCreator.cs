using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LinesCreator : MonoBehaviour {
    private Line _line1;
    private Line _line2;
    void Start()
    {
        DrawLines();

        var intersectionPoint = _line1.IntersectsAt(_line2);
        if (!intersectionPoint.HasValue) return;
        var sphere = Positioning.CreateSphere(Color.blue);
        sphere.transform.position = _line1.Lerp(intersectionPoint.Value).GetAsVector3();
    }

    private void DrawLines() {
        _line1 = new Line(new Coordinate(-100, 0), new Vector3(200, 150));
        _line2 = new Line(new Coordinate(-100, 10), new Vector3(200, 142));
        _line1.Draw(1, Color.green);
        _line2.Draw(1, Color.red);
    }
}
