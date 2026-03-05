using System;

namespace MatrixCalculator {
  class Program {
    private static int OpExit;
    private static int OpAdd;
    private static int OpMultiply;
    private static int OpDetA;
    private static int OpDetB;
    private static int OpCompare;
    private static int OpInvA;
    private static int OpInvB;
    private static int OpDemo;

    static Program() 
    {
      OpExit = 0;
      OpAdd = 1;
      OpMultiply = 2;
      OpDetA = 3;
      OpDetB = 4;
      OpCompare = 5;
      OpInvA = 6;
      OpInvB = 7;
      OpDemo = 8;
    }

    static void Main(string[] args)
    {
      try
      {
        int matrixSize;
        int userChoice;
        SquareMatrix firstMatrix;
        SquareMatrix secondMatrix;
        SquareMatrix resultMatrix;
        SquareMatrix inverseMatrix;
        userChoice = 0;

        Console.WriteLine(" MATRIX CALCULATOR \n");

        Console.Write("Enter matrix size: ");
        matrixSize = int.Parse(Console.ReadLine());

        firstMatrix = new SquareMatrix(matrixSize);
        secondMatrix = new SquareMatrix(matrixSize);

        Console.WriteLine("\nEnter first matrix:");
        firstMatrix.InputMatrix();

        Console.WriteLine("\nEnter second matrix:");
        secondMatrix.InputMatrix();

        Console.WriteLine("\nFirst Matrix:");
        Console.WriteLine(firstMatrix.ToString());
        Console.WriteLine("Second Matrix:");
        Console.WriteLine(secondMatrix.ToString());

        do
        {
          try
          {
            Console.WriteLine("\n OPERATIONS ");
            Console.WriteLine("1. A + B");
            Console.WriteLine("2. A * B");
            Console.WriteLine("3. Determinant of A");
            Console.WriteLine("4. Determinant of B");
            Console.WriteLine("5. Compare A and B");
            Console.WriteLine("6. Inverse of A");
            Console.WriteLine("7. Inverse of B");
            Console.WriteLine("8. Class methods demo");
            Console.WriteLine("0. Exit");
            Console.Write("Choose operation: ");
            userChoice = int.Parse(Console.ReadLine());

            if (userChoice == OpAdd)
            {
              resultMatrix = firstMatrix + secondMatrix;
              Console.WriteLine("\nA + B:");
              Console.WriteLine(resultMatrix.ToString());
            }
            else if (userChoice == OpMultiply)
            {
              resultMatrix = firstMatrix * secondMatrix;
              Console.WriteLine("\nA * B:");
              Console.WriteLine(resultMatrix.ToString());
            }
            else if (userChoice == OpDetA)
            {
              Console.WriteLine("\nDeterminant of A: " + firstMatrix.Determinant().ToString("F4"));
            }
            else if (userChoice == OpDetB)
            {
              Console.WriteLine("\nDeterminant of B: " + secondMatrix.Determinant().ToString("F4"));
            }
            else if (userChoice == OpCompare)
            {
              Console.WriteLine("\nComparison Results:");
              Console.WriteLine("A > B: " + (firstMatrix > secondMatrix));
              Console.WriteLine("A < B: " + (firstMatrix < secondMatrix));
              Console.WriteLine("A == B: " + (firstMatrix == secondMatrix));
              Console.WriteLine("A != B: " + (firstMatrix != secondMatrix));
              Console.WriteLine("CompareTo: " + firstMatrix.CompareTo(secondMatrix));
            }
            else if (userChoice == OpInvA)
            {
              inverseMatrix = firstMatrix.Inverse();
              Console.WriteLine("\nInverse of A:");
              Console.WriteLine(inverseMatrix.ToString());
            }
            else if (userChoice == OpInvB)
            {
              inverseMatrix = secondMatrix.Inverse();
              Console.WriteLine("\nInverse of B:");
              Console.WriteLine(inverseMatrix.ToString());
            }
            else if (userChoice == OpDemo)
            {
              Console.WriteLine("\n CLASS METHODS DEMONSTRATION ");
              Console.WriteLine("Equals: " + firstMatrix.Equals(secondMatrix));
              Console.WriteLine("GetHashCode of A: " + firstMatrix.GetHashCode());
              Console.WriteLine("GetHashCode of B: " + secondMatrix.GetHashCode());

              SquareMatrix clonedMatrix = firstMatrix.Clone();
              Console.WriteLine("\nClone of A:");
              Console.WriteLine(clonedMatrix.ToString());
              Console.WriteLine("Original equals clone: " + firstMatrix.Equals(clonedMatrix));
            }
          }
          catch (SquareMatrix.MatrixException error)
          {
            Console.WriteLine("Matrix Error: " + error.Message);
          }
          catch (Exception error)
          {
            Console.WriteLine("Unexpected Error: " + error.Message);
          }

        } while (userChoice != OpExit);
      }
      catch (SquareMatrix.MatrixException error)
      {
        Console.WriteLine("Matrix Error during initialization: " + error.Message);
      }
      catch (Exception error)
      {
        Console.WriteLine("Standard Error during initialization: " + error.Message);
      }

      Console.WriteLine("\nPress any key to exit...");
      Console.ReadKey();
    }
  }
}