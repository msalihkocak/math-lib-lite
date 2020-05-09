using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane {
    private Coordinate _pointA;
    private Coordinate _vectorV;
    private Coordinate _vectorU;
    
    public Plane(Coordinate pointA, Coordinate pointB, Coordinate pointC) {
        _pointA = pointA;
        _vectorV = pointB - pointA;
        _vectorU = pointC - pointA;
    }

    public Plane(Coordinate pointA, Vector3 vectorV, Vector3 vectorU) {
        _pointA = pointA;
        _vectorV = new Coordinate(vectorV);
        _vectorU = new Coordinate(vectorU);
    }

    public Coordinate Lerp(float v, float u) {
        var clampedV = Mathf.Clamp(v, 0, 1);
        var clampedU = Mathf.Clamp(u, 0, 1);
        return _pointA + _vectorV * clampedV + _vectorU * clampedU;
    }
}
