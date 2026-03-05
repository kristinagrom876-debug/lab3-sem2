using System;

namespace MatrixCalculator {
  class SquareMatrix {
    private static double _epsilon = 1e-10;
    private static int _hashMultiplier = 31;

    private int _size;
    private double[,] _data;

    public SquareMatrix(int matrixSize)
    {
      if (matrixSize <= 0)
        throw new MatrixException("Error: matrix size must be positive.");

      _size = matrixSize;
      _data = new double[_size, _size];
    }

    public void InputMatrix()
    {
      Console.WriteLine("Enter matrix elements:");
      for (int row = 0; row < _size; ++row)
      {
        for (int col = 0; col < _size; ++col)
        {
          Console.Write($"Element [{row}][{col}]: ");
          string input = Console.ReadLine();
          _data[row, col] = double.Parse(input);
        }
      }
    }

    public static SquareMatrix operator +(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
        throw new MatrixException("Error: matrices have different sizes.");

      var result = new SquareMatrix(left._size);

      for (int row = 0; row < left._size; ++row)
        for (int col = 0; col < left._size; ++col)
          result._data[row, col] = left._data[row, col] + right._data[row, col];

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
        throw new MatrixException("Error: matrices have different sizes.");

      var result = new SquareMatrix(left._size);

      for (int row = 0; row < left._size; ++row)
      {
        for (int col = 0; col < left._size; ++col)
        {
          double sum = 0.0;
          for (int inner = 0; inner < left._size; ++inner)
            sum += left._data[row, inner] * right._data[inner, col];
          result._data[row, col] = sum;
        }
      }
      return result;
    }

    public static bool operator >(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
        throw new MatrixException("Error: matrices have different sizes.");

      double leftDet = left.Determinant();
      double rightDet = right.Determinant();
      return leftDet > rightDet;
    }

    public static bool operator <(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
        throw new MatrixException("Error: matrices have different sizes.");

      double leftDet = left.Determinant();
      double rightDet = right.Determinant();
      return leftDet < rightDet;
    }

    public static bool operator ==(SquareMatrix left, SquareMatrix right)
    {
      if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        return true;

      if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        return false;

      if (left._size != right._size)
        return false;

      for (int row = 0; row < left._size; ++row)
        for (int col = 0; col < left._size; ++col)
          if (Math.Abs(left._data[row, col] - right._data[row, col]) > _epsilon)
            return false;

      return true;
    }

    public static bool operator !=(SquareMatrix left, SquareMatrix right) => !(left == right);

    public override bool Equals(object obj)
    {
      var other = obj as SquareMatrix;
      return this == other;
    }

    public override int GetHashCode()
    {
      int hash = _size;

      for (int row = 0; row < _size; ++row)
        for (int col = 0; col < _size; ++col)
          hash = hash * _hashMultiplier + (int)(_data[row, col] * 1000);

      return hash;
    }

    public double Determinant()
    {
      if (_size == 1)
        return _data[0, 0];

      if (_size == 2)
        return _data[0, 0] * _data[1, 1] - _data[0, 1] * _data[1, 0];

      double det = 0.0;

      for (int col = 0; col < _size; ++col)
      {
        var sub = new SquareMatrix(_size - 1);

        for (int row = 1; row < _size; ++row)
        {
          int subCol = 0;
          for (int origCol = 0; origCol < _size; ++origCol)
          {
            if (origCol == col) continue;
            sub._data[row - 1, subCol] = _data[row, origCol];
            ++subCol;
          }
        }

        double sign = (col % 2 == 0) ? 1.0 : -1.0;
        det += sign * _data[0, col] * sub.Determinant();
      }

      return det;
    }

    public SquareMatrix Inverse()
    {
      double det = Determinant();

      if (Math.Abs(det) < _epsilon)
        throw new MatrixException("Error: matrix is singular. Inverse does not exist.");

      var result = new SquareMatrix(_size);

      if (_size == 1)
      {
        result._data[0, 0] = 1.0 / _data[0, 0];
        return result;
      }

      for (int row = 0; row < _size; ++row)
      {
        for (int col = 0; col < _size; ++col)
        {
          var sub = new SquareMatrix(_size - 1);
          int subRow = 0;

          for (int origRow = 0; origRow < _size; ++origRow)
          {
            if (origRow == row) continue;

            int subCol = 0;
            for (int origCol = 0; origCol < _size; ++origCol)
            {
              if (origCol == col) continue;

              sub._data[subRow, subCol] = _data[origRow, origCol];
              ++subCol;
            }
            ++subRow;
          }

          double sign = ((row + col) % 2 == 0) ? 1.0 : -1.0;
          double cofactor = sign * sub.Determinant();
          result._data[col, row] = cofactor / det; 
        }
      }

      return result;
    }

    public int CompareTo(SquareMatrix other)
    {
      if (other == null)
        return 1;

      double thisDet = Determinant();
      double otherDet = other.Determinant();

      if (Math.Abs(thisDet - otherDet) < _epsilon)
        return 0;

      return thisDet < otherDet ? -1 : 1;
    }

    public bool Equals(SquareMatrix other) => this == other;

    public SquareMatrix Clone()
    {
      var clone = new SquareMatrix(_size);
      Array.Copy(_data, clone._data, _data.Length);
      return clone;
    }

    public override string ToString()
    {
      string result = $"[{_size}x{_size}]:\n";

      for (int row = 0; row < _size; ++row)
      {
        result += "[ ";
        for (int col = 0; col < _size; ++col)
          result += _data[row, col].ToString("F2") + " ";
        result += "]\n";
      }

      return result;
    }
  }
}