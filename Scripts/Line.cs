using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line {
    private Coordinate _pointA;
    private Coordinate _vector;
    private LineType _type;
    private const float Tolerance = 0.001f;

    public Coordinate Vector {
        get => _vector;
        set => _vector = value;
    }

    public Coordinate PointA {
        get => _pointA;
        set => _pointA = value;
    }

    public enum LineType {
        Line, Segment, Ray
    }
    
    public Line(Coordinate pointA, Coordinate pointB, LineType type = LineType.Line) {
        _pointA = pointA;
        _vector = pointB - pointA;
        _type = type;
    }
    
    public Line(Coordinate pointA, Vector3 vector, LineType type = LineType.Segment) {
        _pointA = pointA;
        _vector = new Coordinate(vector);
        _type = type;
    }

    public float? IntersectsAt(Line line) {
        if (Math.Abs(RotateMath.DotProduct(line._vector.perp, _vector)) < Tolerance) return null;
        
        var intersectPoint = RotateMath.DotProduct(line._vector.perp, line._pointA - _pointA) /
                               RotateMath.DotProduct(line._vector.perp, _vector);
        switch (_type) {
            case LineType.Segment when intersectPoint > 1 || intersectPoint < 0:
            case LineType.Ray when intersectPoint < 0:
                return null;
            default:
                return intersectPoint;
        }
    }

    public void Draw(float width, Color color) {
        Coordinate.DrawLine(_pointA, _pointA + _vector, width, color);
    }

    public Coordinate Lerp(float t) {
        var refinedT = RefineT(t);
        var xOfPoint = _pointA.X + _vector.X * refinedT;
        var yOfPoint = _pointA.Y + _vector.Y * refinedT;
        var zOfPoint = _pointA.Z + _vector.Z * refinedT;
        return new Coordinate(xOfPoint, yOfPoint, zOfPoint);
    }

    private float RefineT(float t) {
        switch (_type) {
            case LineType.Line:
                return t;
            case LineType.Ray:
                return Mathf.Clamp(t, 0, Single.PositiveInfinity);
            case LineType.Segment:
                return Mathf.Clamp(t, 0, 1);
            default:
                throw new ArgumentOutOfRangeException();
        }
    } 
}
