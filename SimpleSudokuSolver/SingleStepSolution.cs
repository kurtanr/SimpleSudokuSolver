using System;
using System.Linq;
using System.Text;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Single step of a solution can be:
  /// - one value in one cell being set, in which case the <see cref="Result"/> property is set
  /// - one or more candidate values eliminated, in which case the <see cref="Eliminations"/> property is set
  /// </summary>
  public class SingleStepSolution
  {
    /// <summary>
    /// Contains value and zero-based row and column index of the cell.
    /// </summary>
    public class Candidate : IEquatable<Candidate>
    {
      public Candidate(int indexOfRow, int indexOfColumn, int value)
      {
        IndexOfRow = indexOfRow;
        IndexOfColumn = indexOfColumn;
        Value = value;
      }

      /// <summary>
      /// Zero-based index of a row where the cell which is solved is located.
      /// </summary>
      public int IndexOfRow { get; }

      /// <summary>
      /// Zero-based index of a column where the cell which is solved is located.
      /// </summary>
      public int IndexOfColumn { get; }

      /// <summary>
      /// Value of the cell which is solved.
      /// </summary>
      public int Value { get; }

      /// <remarks>
      /// For implementation details see:
      /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
      /// </remarks>
      public static bool operator ==(Candidate lhs, Candidate rhs)
      {
        // Check for null on left side.
        if (object.ReferenceEquals(lhs, null))
        {
          if (object.ReferenceEquals(rhs, null))
          {
            // null == null = true.
            return true;
          }

          // Only the left side is null.
          return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
      }

      public static bool operator !=(Candidate lhs, Candidate rhs)
      {
        return !(lhs == rhs);
      }

      public override bool Equals(object obj)
      {
        return Equals(obj as Candidate);
      }

      public bool Equals(Candidate other)
      {
        return other != null &&
               IndexOfRow == other.IndexOfRow &&
               IndexOfColumn == other.IndexOfColumn &&
               Value == other.Value;
      }

      public override int GetHashCode()
      {
        var hashCode = -613434448;
        hashCode = hashCode * -1521134295 + IndexOfRow.GetHashCode();
        hashCode = hashCode * -1521134295 + IndexOfColumn.GetHashCode();
        hashCode = hashCode * -1521134295 + Value.GetHashCode();
        return hashCode;
      }

      public override string ToString()
      {
        return $"[{IndexOfRow},{IndexOfColumn}] - {Value}";
      }
    }

    /// <summary>
    /// Candidate value which has been promoted to result.
    /// </summary>
    public Candidate Result { get; }

    /// <summary>
    /// Candidate values which have been eliminated.
    /// </summary>
    public Candidate[] Eliminations { get; }

    /// <summary>
    /// Strategy used to find the solution.
    /// </summary>
    public string Strategy { get; }

    /// <summary>
    /// Details on how the solver found the solution.
    /// </summary>
    public string SolutionDescription { get; }

    public SingleStepSolution(int indexOfRow, int indexOfColumn, int value, string strategyName)
    {
      Eliminations = null;
      Result = new Candidate(indexOfRow, indexOfColumn, value);
      Strategy = strategyName;
      SolutionDescription = $"Row {Result.IndexOfRow + 1} Column {Result.IndexOfColumn + 1} Value {Result.Value} [{strategyName}]";
    }

    public SingleStepSolution(Candidate[] eliminations, string strategyName)
    {
      Eliminations = eliminations;
      Result = null;

      var groupedEliminations = Eliminations.GroupBy(x => new { x.IndexOfRow, x.IndexOfColumn })
        .Select(g => new { g.Key.IndexOfRow, g.Key.IndexOfColumn, Values = g.Select(x => x.Value).Distinct().OrderBy(x => x) }).ToArray();

      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"Eliminated candidates [{strategyName}]:");

      foreach(var groupedElimination in groupedEliminations)
      {
        stringBuilder.AppendLine($"- Row {groupedElimination.IndexOfRow + 1} Column {groupedElimination.IndexOfColumn + 1} " +
          $"Values {string.Join(",", groupedElimination.Values)}");
      }

      Strategy = strategyName;
      SolutionDescription = stringBuilder.ToString().TrimEnd('\r', '\n');
    }

    public override string ToString()
    {
      return SolutionDescription;
    }
  }
}
