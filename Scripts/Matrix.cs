using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

public class Matrix {
  private float[] _values;
  private int _rowCount;
  private int _columnCount;

  public Matrix(int rowCount, int columnCount, float[] values) {
    _rowCount = rowCount;
    _columnCount = columnCount;
    _values = new float[rowCount * columnCount];
    Array.Copy(values, _values, rowCount * columnCount);
  }

  public Matrix(Coordinate coordinate) {
    _rowCount = 4;
    _columnCount = 1;
    _values = coordinate.GetAsFloats();
  }


  public Matrix(float amount, MatrixInitializationType type) {
    _rowCount = 4;
    _columnCount = 4;
    _values = new float[16];
    var cos = Mathf.Cos(amount);
    var sin = Mathf.Sin(amount);
    switch (type) {
      case MatrixInitializationType.RotateXVector:
        _values[0] = 1;
        
        _values[5] = cos;
        _values[6] = -sin;
        _values[9] = sin;
        _values[10] = cos;
        break;
      case MatrixInitializationType.RotateYVector:
        _values[5] = 1;
        
        _values[0] = cos;
        _values[2] = sin;
        _values[8] = -sin;
        _values[10] = cos;
        break;
      case MatrixInitializationType.RotateZVector:
        _values[10] = 1;
        
        _values[0] = cos;
        _values[1] = -sin;
        _values[4] = sin;
        _values[5] = cos;
        break;
      case MatrixInitializationType.ShearVector:
        _values[0] = 1;
        _values[5] = 1;
        _values[10] = 1;
        _values[15] = 1;
        
        _values[1] = amount;
        break;
      case MatrixInitializationType.ReflectionVector:
        _values[0] = -1;
        _values[5] = 1;
        _values[10] = 1;
        _values[15] = 1;
        break;
    }
  }
  public Matrix(Coordinate coordinate, MatrixInitializationType type) {
    _rowCount = 4;
    _columnCount = 4;
    _values = new float[16];
    switch (type) {
      case MatrixInitializationType.TranslateVector:
        _values[0] = 1;
        _values[5] = 1;
        _values[10] = 1;
        
        _values[3] = coordinate.X;
        _values[7] = coordinate.Y;
        _values[11] = coordinate.Z;
        _values[15] = coordinate.W;
        break;
      case MatrixInitializationType.ScaleVector:
        _values[0] = coordinate.X;
        _values[5] = coordinate.Y;
        _values[10] = coordinate.Z;
        _values[15] = coordinate.W;
        break;
      case MatrixInitializationType.QuaternionVector:
        _values[0] = 1 - 2 * Mathf.Pow(coordinate.Y, 2) - 2 * Mathf.Pow(coordinate.Z, 2);
        _values[1] = 2 * coordinate.X * coordinate.Y - 2 * coordinate.W * coordinate.Z;
        _values[2] = 2 * coordinate.X * coordinate.Z + 2 * coordinate.W * coordinate.Y;
        
        _values[4] = 2 * coordinate.X * coordinate.Y + 2 * coordinate.W * coordinate.Z;
        _values[5] = 1 - 2 * Mathf.Pow(coordinate.X, 2) - 2 * Mathf.Pow(coordinate.Z, 2);
        _values[6] = 2 * coordinate.Y * coordinate.Z - 2 * coordinate.W * coordinate.X;
        
        _values[8] = 2 * coordinate.X * coordinate.Z - 2 * coordinate.W * coordinate.Y;
        _values[9] = 2 * coordinate.Y * coordinate.Z + 2 * coordinate.W * coordinate.X;
        _values[10] = 1 - 2 * Mathf.Pow(coordinate.X, 2) - 2 * Mathf.Pow(coordinate.Y, 2);
        
        _values[15] = 1;
        break;
    }
  }

  public Coordinate AsCoordinate() {
    if (_rowCount == 4 && _columnCount == 1) {
      return new Coordinate(_values[0], _values[1], _values[2], _values[3]);
    }
    throw new Exception("Matrix is not convertible to Coordinate");
  }

  public static Matrix operator +(Matrix a, Matrix b) {
    if (!AreMatricesSummable(a, b)) throw new Exception("Matrices are not summable");
    var sum = new Matrix(a._rowCount, a._columnCount, new float[a._rowCount * a._columnCount]);
    for (var i = 0; i < a._values.Length; i++) {
      sum._values[i] = a._values[i] + b._values[i];
    }

    return sum;
  }

  public static Matrix operator -(Matrix matrix) {
    return new Matrix(matrix._rowCount, matrix._columnCount, matrix._values.Select((t) => t * -1).ToArray());
  }
  
  public static Matrix operator -(Matrix a, Matrix b) {
    return a + -b;
  }

  public static Matrix operator *(Matrix a, Matrix b) {
    if (!AreMatricesMultipliable(a,b)) throw new Exception("Matrices are not multipliable");
    var multiplication = new Matrix(a._rowCount, b._columnCount, new float[a._rowCount * b._columnCount]);
    
    for (var i = 0; i < multiplication._rowCount; i++) {
      for (var j = 0; j < multiplication._columnCount; j++) {
        multiplication._values[i * multiplication._columnCount + j] = MultiplyAggregate(a.GetRow(i), b.GetColumn(j));
      }
    }

    return multiplication;
  }

  private static bool AreMatricesSummable(Matrix a, Matrix b) {
    if (a == null || b == null) return false;
    return a._rowCount == b._rowCount && a._columnCount == b._columnCount;
  }

  private static bool AreMatricesMultipliable(Matrix a, Matrix b) {
    if (a == null || b == null) return false;
    return a._columnCount == b._rowCount;
  }

  public float ElementAt(int rowNum, int columnNum) {
    if (rowNum >= _rowCount || columnNum >= _columnCount) throw new Exception("Index out of bounds of matrix");
    return _values[rowNum * _columnCount + columnNum];
  }

  public static Matrix Zero(int size) {
    return new Matrix(size, size, new float[size * size]);
  }

  public float[] GetRow(int rowNum) {
    if (rowNum >= _rowCount) throw new Exception("Row index is out of bounds for matrix");
    return _values.Skip(rowNum * _columnCount).Take(_columnCount).ToArray();
  }
  
  public float[] GetColumn(int columnNum) {
    if (columnNum >= _columnCount) throw new Exception("Column index is out of bounds for matrix");
    var column = new float[_rowCount];
    for (var i = 0; i < _rowCount; i++) {
      var row = GetRow(i);
      column[i] = row[columnNum];
    }

    return column;
  }
  
  public float[] GetDiagonal() {
    var diagonal = new float[_rowCount];
    for (var i = 0; i < _rowCount; i++) {
      diagonal[i] = ElementAt(i, i);
    }

    return diagonal;
  }

  private static float MultiplyAggregate(float[] a, float[] b) {
    return a.Select((t, i) => t * b[i]).Sum();
  }

  public override string ToString() {
    var matrixString = new StringBuilder("[\n");
    for (var i = 0; i < _rowCount; i++) {
      var currentRow = GetRow(i);
      var rowString = string.Join(", ", currentRow);
      matrixString.Append(" ").Append(rowString).Append("\n");
    }

    matrixString.Append("]");
    return matrixString.ToString();
  }
}

public enum MatrixInitializationType {
  TranslateVector, ScaleVector, RotateXVector, RotateYVector, RotateZVector, ShearVector, ReflectionVector, QuaternionVector
}