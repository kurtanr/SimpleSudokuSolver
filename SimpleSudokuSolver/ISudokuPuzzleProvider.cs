namespace SimpleSudokuSolver
{
  /// <summary>
  /// Interface of a sudoku puzzle provider.
  /// </summary>
  public interface ISudokuPuzzleProvider
  {
    /// <summary>
    /// Each value of the array represents a value of the cell.
    /// For a 9x9 sudoku, allowed values are: 0-9 (0 represents an unknown value).
    /// </summary>
    /// <returns>Puzzle in the form of a 2D integer array.</returns>
    int[,] GetPuzzle();
  }
}
