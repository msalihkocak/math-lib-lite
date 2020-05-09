using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    private float _x;
    private float _y;
    private readonly float _z;

    public Coordinate(float x, float y)
    {
        _x = x;
        _y = y;
        _z = -1;
    }
    
    public Coordinate(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public Coordinate(Vector3 vector3)
    {
        _x = vector3.x;
        _y = vector3.y;
        _z = vector3.z;
    }

    public float X
    {
        get => _x;
        set => _x = value;
    }

    public float Y
    {
        get => _y;
        set => _y = value;
    }

    public float Z => _z;
    
    public Coordinate perp => new Coordinate(-_y, _x);
    
    public float magnitude => GetAsVector3().magnitude;

    public Vector3 GetAsVector3(float marginZ = 0)
    {
        return new Vector3(_x, _y, _z + marginZ);
    }

    public float GetMagnitude()
    {
        return GetAsVector3().magnitude;
    }

    public static Coordinate operator +(Coordinate a, Coordinate b) {
        return new Coordinate(a._x + b.X, a._y + b._y, a._z + b._z);
    }
    
    public static Coordinate operator -(Coordinate a, Coordinate b) {
        return new Coordinate(a._x - b._x, a._y - b._y, a._z - b._z);
    }
    
    public static Coordinate operator *(Coordinate a, float t) {
        return new Coordinate(a._x * t, a._y * t, a._z * t);
    }

    public static void DrawPoint(Coordinate position, float width, Color color)
    {
        var line = new GameObject("Point_" + position.ToString());
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(position._x - width / 3.0f, position._y - width / 3.0f, position._z));
        lineRenderer.SetPosition(1, new Vector3(position._x + width / 3.0f, position._y + width / 3.0f, position._z));
        SetLineRenderDefaultParams(lineRenderer, color, width);
    }

    public static void DrawLine(Coordinate startPosition, Coordinate endPosition, float width, 
        Color color, float marginZ = 0)
    {
        var line = new GameObject("Line_" + startPosition.ToString() + " - " + endPosition.ToString());
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition.GetAsVector3(marginZ));
        lineRenderer.SetPosition(1, endPosition.GetAsVector3(marginZ));
        SetLineRenderDefaultParams(lineRenderer, color, width);
    }

    private static void SetLineRenderDefaultParams(LineRenderer lineRenderer, Color color, float width)
    {
        lineRenderer.material = new Material(Shader.Find("Unlit/Color")) {color = color};
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public override string ToString()
    {
        return "(" + _x + "," + _y + "," + _z + ")";
    }
}
