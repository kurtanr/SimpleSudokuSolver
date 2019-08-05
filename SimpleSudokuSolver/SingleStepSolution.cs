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
    public class Candidate
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
