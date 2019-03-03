namespace SimpleSudokuSolver
{
  /// <summary>
  /// Details regarding which cell the sudoku solver solved (and how).
  /// </summary>
  public class SingleStepSolution
  {
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

    /// <summary>
    /// Details on how the solver found the solution.
    /// </summary>
    public string SolutionDescription { get; }

    public SingleStepSolution(int indexOfRow, int indexOfColumn, int value, string solutionDescription = "")
    {
      IndexOfRow = indexOfRow;
      IndexOfColumn = indexOfColumn;
      Value = value;
      SolutionDescription = solutionDescription;
    }

    public override string ToString()
    {
      return SolutionDescription;
    }
  }
}
