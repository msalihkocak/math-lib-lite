using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate {
  public Coordinate(float x, float y) {
    X = x;
    Y = y;
    Z = -1;
  }

  public Coordinate(float x, float y, float z) {
    X = x;
    Y = y;
    Z = z;
  }

  public Coordinate(Vector3 vector3) {
    X = vector3.x;
    Y = vector3.y;
    Z = vector3.z;
  }

  public Coordinate(float x, float y, float z, float w) {
    X = x;
    Y = y;
    Z = z;
    W = w;
  }

  public Coordinate(Vector3 vector, float w) {
    X = vector.x;
    Y = vector.y;
    Z = vector.z;
    W = w;
  }

  public float X { get; }

  public float Y { get; }

  public float W { get; }

  public float Z { get; }

  public Coordinate perp => new Coordinate(-Y, X);

  public float magnitude => GetAsVector3().magnitude;

  public Vector3 normalized => GetAsVector3().normalized;

  public Vector3 GetAsVector3(float marginZ = 0) {
    return new Vector3(X, Y, Z + marginZ);
  }

  public float[] GetAsFloats() {
    float[] values = {X, Y, Z, W};
    return values;
  }

  public float GetMagnitude() {
    return GetAsVector3().magnitude;
  }

  public static Coordinate operator +(Coordinate a, Coordinate b) {
    return new Coordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
  }

  public static Coordinate operator -(Coordinate a, Coordinate b) {
    return new Coordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
  }

  public static Coordinate operator *(Coordinate a, float t) {
    return new Coordinate(a.X * t, a.Y * t, a.Z * t);
  }

  public static void DrawPoint(Coordinate position, float width, Color color) {
    var line = new GameObject("Point_" + position.ToString());
    var lineRenderer = line.AddComponent<LineRenderer>();
    lineRenderer.SetPosition(0, new Vector3(position.X - width / 3.0f, position.Y - width / 3.0f, position.Z));
    lineRenderer.SetPosition(1, new Vector3(position.X + width / 3.0f, position.Y + width / 3.0f, position.Z));
    SetLineRenderDefaultParams(lineRenderer, color, width);
  }

  public static void DrawLine(Coordinate startPosition, Coordinate endPosition, float width,
    Color color, float marginZ = 0) {
    var line = new GameObject("Line_" + startPosition.ToString() + " - " + endPosition.ToString());
    var lineRenderer = line.AddComponent<LineRenderer>();
    lineRenderer.SetPosition(0, startPosition.GetAsVector3(marginZ));
    lineRenderer.SetPosition(1, endPosition.GetAsVector3(marginZ));
    SetLineRenderDefaultParams(lineRenderer, color, width);
  }

  private static void SetLineRenderDefaultParams(LineRenderer lineRenderer, Color color, float width) {
    lineRenderer.material = new Material(Shader.Find("Unlit/Color")) {color = color};
    lineRenderer.positionCount = 2;
    lineRenderer.startWidth = width;
    lineRenderer.endWidth = width;
  }

  public override string ToString() {
    return "(" + X + "," + Y + "," + Z + ")";
  }
}