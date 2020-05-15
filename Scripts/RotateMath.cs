using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class RotateMath : MonoBehaviour {
  public static float Distance(Coordinate point1, Coordinate point2) {
    return Mathf.Sqrt((Mathf.Pow(point2.X - point1.X, 2) + Mathf.Pow(point2.Y - point1.Y, 2)));
  }

  public static float DotProduct(Coordinate vector1, Coordinate vector2) {
    return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
  }

  public static Coordinate CrossProduct(Coordinate vector1, Coordinate vector2) {
    var newX = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
    var newY = vector1.Z * vector2.X - vector1.X * vector2.Z;
    var newZ = vector1.X * vector2.Y - vector1.Y * vector2.X;
    return new Coordinate(newX, newY, newZ);
  }

  public static float AngleBetween(Coordinate vector1, Coordinate vector2) {
    var reverseCosInput = DotProduct(vector1, vector2) / (vector1.magnitude * vector2.magnitude);
    var angleBetween = Mathf.Acos(reverseCosInput);
    return angleBetween;
  }

  public static Coordinate RotateOnZAxis(Coordinate vector, float angle) {
    var newX = vector.X * Mathf.Cos(angle) - vector.Y * Mathf.Sin(angle);
    var newY = vector.X * Mathf.Sin(angle) + vector.Y * Mathf.Cos(angle);
    return new Coordinate(newX, newY, 0);
  }

  public static Coordinate Translate(Coordinate currentPosition, Coordinate currentFacing, Coordinate inputVector) {
    var movementVector = CalculateMovementVector(currentFacing, inputVector);
    var newX = currentPosition.X + movementVector.x;
    var newY = currentPosition.Y + movementVector.y;
    var newZ = currentPosition.Z + movementVector.z;
    return new Coordinate(newX, newY, newZ);
  }

  public static Coordinate TranslateByMatrix(Coordinate position, Coordinate vector) {
    return (new Matrix(vector, MatrixInitializationType.TranslateVector) * new Matrix(position)).AsCoordinate();
  }
  
  public static Coordinate ScaleByMatrix(Coordinate scale, Coordinate vector) {
    return (new Matrix(vector, MatrixInitializationType.ScaleVector) * new Matrix(scale)).AsCoordinate();
  }

  public static Coordinate RotateByMatrix(Coordinate position, Coordinate anglesContainer) {
    var rotationMatrixFromEulerAngles = GetRotationMatrixFromEulerAngles(anglesContainer);
    return (rotationMatrixFromEulerAngles * new Matrix(position)).AsCoordinate();
  }

  private static Vector3 CalculateMovementVector(Coordinate currentFacing, Coordinate inputVector) {
    if (inputVector.GetAsVector3().Equals(Vector3.zero)) return Vector3.zero;

    var angleBetween = AngleBetween(inputVector, currentFacing);
    var worldAngle = AngleBetween(inputVector, new Coordinate(Vector3.up));
    if (CrossProduct(inputVector, currentFacing).Z < 0) {
      angleBetween = -angleBetween;
    }

    return RotateOnZAxis(inputVector, angleBetween + worldAngle).GetAsVector3();
  }

  public static Coordinate LookAt(Transform sourceTransform, Transform targetTransform) {
    var rotationVector = targetTransform.position - sourceTransform.position;
    var angleBetweenVectors =
      AngleBetween(new Coordinate(sourceTransform.up), new Coordinate(rotationVector));
    Assert.IsFalse(float.IsNaN(angleBetweenVectors));
    if (CrossProduct(new Coordinate(sourceTransform.up), new Coordinate(rotationVector)).Z < 0) {
      angleBetweenVectors = -angleBetweenVectors;
    }

    return RotateOnZAxis(new Coordinate(sourceTransform.up), angleBetweenVectors);
  }

  public static Coordinate Lerp(Coordinate pointA, Coordinate pointB, float t) {
    var clampedT = Mathf.Clamp(t, 0, 1);
    var vector = new Coordinate(pointB.X - pointA.X, pointB.Y - pointA.Y, pointB.Z - pointA.Z);
    var xOfPoint = pointA.X + vector.X * clampedT;
    var yOfPoint = pointA.Y + vector.Y * clampedT;
    var zOfPoint = pointA.Z + vector.Z * clampedT;
    return new Coordinate(xOfPoint, yOfPoint, zOfPoint);
  }

  public static Coordinate Shear(Coordinate position, float shearXAmount) {
    return (new Matrix(shearXAmount, MatrixInitializationType.ShearVector) * new Matrix(position)).AsCoordinate();
  }
  
  public static Coordinate Reflection(Coordinate position) {
    return (new Matrix(0.0f, MatrixInitializationType.ReflectionVector) * new Matrix(position)).AsCoordinate();
  }

  public static Coordinate Quaternion(Coordinate rotationAxis, float angle) {
    var normalized = rotationAxis.normalized;
    var w = Mathf.Cos(angle * Mathf.Deg2Rad / 2.0f);
    var sin = Mathf.Sin(angle * Mathf.Deg2Rad / 2.0f);
    return new Coordinate(normalized.x * sin, normalized.y * sin, normalized.z * sin, w);
  }

  public static Coordinate RotateByQuaternion(Coordinate position, Coordinate rotationAxis, float angle) {
    var quaternion = Quaternion(rotationAxis, angle);
    var quaternionMatrix = new Matrix(quaternion, MatrixInitializationType.QuaternionVector);
    return (quaternionMatrix * new Matrix(position)).AsCoordinate();
  }
  
  public static Matrix GetRotationMatrixFromEulerAngles(Coordinate anglesContainer) {
    var anglesInRadian = anglesContainer * Mathf.Deg2Rad;
    var xRotationMatrix = new Matrix(anglesInRadian.X, MatrixInitializationType.RotateXVector);
    var yRotationMatrix = new Matrix(anglesInRadian.Y, MatrixInitializationType.RotateYVector);
    var zRotationMatrix = new Matrix(anglesInRadian.Z, MatrixInitializationType.RotateZVector);
    return zRotationMatrix * yRotationMatrix * xRotationMatrix;
  }

  public static float ExtractAngle(Matrix rotationMatrix) {
    return Mathf.Acos(0.5f * (rotationMatrix.GetDiagonal().Sum() - 2));
  }
  
  public static Coordinate ExtractRotationAxis(Matrix rotationMatrix) {
    var angle = ExtractAngle(rotationMatrix);
    var denominator = 2 * Mathf.Sin(angle);
    
    var rotAxisX = (rotationMatrix.ElementAt(2,1) - 
                    rotationMatrix.ElementAt(1,2)) / denominator;
    var rotAxisY = (rotationMatrix.ElementAt(0,2) - 
                    rotationMatrix.ElementAt(2,0)) / denominator;
    var rotAxisZ = (rotationMatrix.ElementAt(1,0) - 
                    rotationMatrix.ElementAt(0,1)) / denominator;
    return new Coordinate(rotAxisX, rotAxisY, rotAxisZ, 0);
  }
}