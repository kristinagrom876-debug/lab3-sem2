using System;

namespace MatrixCalculator {
  class SquareMatrix {
    public class MatrixException : Exception {
      public MatrixException(string message) : base(message) { }
    }

    private static double _epsilon;
    private double PositiveSign;
    private double NegativeSign;
    private double One;
    private static int _hashMultiplier;
    private int HashScaleFactor;
    private int SizeOne;
    private int SizeTwo;
    private int First;
    private int EvenDivisor;
    private int FirstRow;
    private int FirstCol;
    private int RowOffset;
    private int StartIndex;
    private int SubMatrixSizeDecrement;

    private int _size;
    private double[,] _data;

    static SquareMatrix()
    {
      _epsilon = 1e-10;
      _hashMultiplier = 31;
    }

    public SquareMatrix()
    {
      PositiveSign = 1.0;
      NegativeSign = -1.0;
      HashScaleFactor = 1000;
      SizeOne = 1;
      SizeTwo = 2;
      First = 1;
      One = 1.0;
      EvenDivisor = 2;
      FirstRow = 0;
      FirstCol = 0;
      RowOffset = 1;
      StartIndex = 0;
      SubMatrixSizeDecrement = 1;
      RowOffset = 1;
    }

    public SquareMatrix(int matrixSize) : this()
    {
      if (matrixSize <= 0)
      {
        throw new MatrixException("Error: matrix size must be positive.");
      }

      _size = matrixSize;
      _data = new double[_size, _size];
    }

    public void InputMatrix()
    {
      string input;

      Console.WriteLine("Enter matrix elements:");
      for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < _size; ++colIndex)
        {
          Console.Write($"Element [{rowIndex}][{colIndex}]: ");
          input = Console.ReadLine();
          _data[rowIndex, colIndex] = double.Parse(input);
        }
      }
    }

    public static SquareMatrix operator +(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      SquareMatrix result = new SquareMatrix(left._size);

      for (int rowIndex = 0; rowIndex < left._size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < left._size; ++colIndex)
        {
          result._data[rowIndex, colIndex] = left._data[rowIndex, colIndex] + right._data[rowIndex, colIndex];
        }
      }

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix left, SquareMatrix right)
    {
      double sum;
      if (left._size != right._size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      SquareMatrix result = new SquareMatrix(left._size);

      for (int rowIndex = 0; rowIndex < left._size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < left._size; ++colIndex)
        {
          sum = 0.0;
          for (int inner = 0; inner < left._size; ++inner)
          {
            sum += left._data[rowIndex, inner] * right._data[inner, colIndex];
          }
          result._data[rowIndex, colIndex] = sum;
        }
      }
      return result;
    }

    public static bool operator >(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      double leftDet;
      double rightDet;
      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet > rightDet;
    }

    public static bool operator <(SquareMatrix left, SquareMatrix right)
    {
      if (left._size != right._size)
      {
        throw new MatrixException("Error: matrices have different sizes.");
      }

      double leftDet;
      double rightDet;
      leftDet = left.Determinant();
      rightDet = right.Determinant();
      return leftDet < rightDet;
    }

    public static bool operator ==(SquareMatrix left, SquareMatrix right)
    {
      if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
      {
        return true;
      }

      if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
      {
        return false;
      }

      if (left._size != right._size)
      {
        return false;
      }

      for (int rowIndex = 0; rowIndex < left._size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < left._size; ++colIndex)
        {
          if (Math.Abs(left._data[rowIndex, colIndex] - right._data[rowIndex, colIndex]) > _epsilon)
          {
            return false;
          }
        }
      }

      return true;
    }

    public static bool operator !=(SquareMatrix left, SquareMatrix right)
    {
      return !(left == right);
    }

    public override bool Equals(object obj)
    {
      SquareMatrix other = obj as SquareMatrix;
      return this == other;
    }

    public override int GetHashCode()
    {
      int hash;
      hash = _size;

      for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < _size; ++colIndex)
        {
          hash = hash * _hashMultiplier + (int)(_data[rowIndex, colIndex] * HashScaleFactor);
        }
      }

      return hash;
    }

    public double Determinant()
    {
      if (_size == SizeOne)
        return _data[StartIndex, StartIndex];

      if (_size == SizeTwo)
      {
        return _data[StartIndex, StartIndex] * _data[StartIndex + 1, StartIndex + 1] -
               _data[StartIndex, StartIndex + 1] * _data[StartIndex + 1, StartIndex];
      }

      double det = 0.0;

      for (int colIndex = StartIndex; colIndex < _size; ++colIndex)
      {
        SquareMatrix sub = new SquareMatrix(_size - SubMatrixSizeDecrement);

        for (int rowIndex = StartIndex + 1; rowIndex < _size; ++rowIndex)
        {
          int subCol = StartIndex;

          for (int origCol = StartIndex; origCol < _size; ++origCol)
          {
            if (origCol == colIndex)
            {
              continue;
            }
            sub._data[rowIndex - RowOffset, subCol] = _data[rowIndex, origCol];
            ++subCol;
          }
        }

        double sign = (colIndex % EvenDivisor == 0) ? PositiveSign : NegativeSign;
        det += sign * _data[StartIndex, colIndex] * sub.Determinant();
      }

      return det;
    }

    public SquareMatrix Inverse()
    {
      double det = Determinant();

      if (Math.Abs(det) < _epsilon)
        throw new MatrixException("Error: matrix is singular. Inverse does not exist.");

      SquareMatrix result = new SquareMatrix(_size);

      if (_size == SizeOne)
      {
        result._data[FirstRow, FirstCol] = One / _data[FirstRow, FirstCol];
        return result;
      }

      for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < _size; ++colIndex)
        {
          SquareMatrix sub = new SquareMatrix(_size - First);
          int subRow = 0;

          for (int origRow = 0; origRow < _size; ++origRow)
          {
            if (origRow == rowIndex)
            {
              continue;
            }

            int subCol = 0;
            for (int origCol = 0; origCol < _size; ++origCol)
            {
              if (origCol == colIndex)
              {
                continue;
              }

              sub._data[subRow, subCol] = _data[origRow, origCol];
              ++subCol;
            }
            ++subRow;
          }

          double sign = ((rowIndex + colIndex) % EvenDivisor == 0) ? PositiveSign : NegativeSign;
          double cofactor = sign * sub.Determinant();
          result._data[colIndex, rowIndex] = cofactor / det;
        }
      }

      return result;
    }

    public int CompareTo(SquareMatrix other)
    {
      double thisDet;
      double otherDet;
      int result;

      if (other == null)
      {
        result = 1;
        return result;
      }

      thisDet = Determinant();
      otherDet = other.Determinant();

      if (Math.Abs(thisDet - otherDet) < _epsilon)
      {
        result = 0;
        return result;
      }

      if (thisDet < otherDet)
      {
        result = -1;
        return result;
      }

      result = 1;
      return result;
    }

    public bool Equals(SquareMatrix other)
    {
      return this == other;
    }

    public SquareMatrix Clone()
    {
      SquareMatrix clone = new SquareMatrix(_size);
      Array.Copy(_data, clone._data, _data.Length);
      return clone;
    }

    public override string ToString()
    {
      string results;
      results = $"[{_size}x{_size}]:\n";

      for (int rowIndex = 0; rowIndex < _size; ++rowIndex)
      {
        results += "[ ";
        for (int colIndex = 0; colIndex < _size; ++colIndex)
        {
          results += _data[rowIndex, colIndex].ToString("F2") + " ";
        }
        results += "]\n";
      }

      return results;
    }
  }
}