using SimpleSudokuSolver.Model;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Interface of a sudoku solver.
  /// </summary>
  public interface ISudokuSolver
  {
    /// <summary>
    /// Solves entire sudoku puzzle.
    /// </summary>
    SudokuPuzzle Solve(int[,] sudoku);

    /// <summary>
    /// Solves entire sudoku puzzle and outputs the steps used to solve it.
    /// </summary>
    SudokuPuzzle Solve(int[,] sudoku, out SingleStepSolution[] steps);

    /// <summary>
    /// Solves single cell in a sudoku puzzle.
    /// </summary>
    /// <returns>null if no cell can be solved.</returns>
    SingleStepSolution SolveSingleStep(int[,] sudoku);

    /// <summary>
    /// Returns whether the puzzle is solved or not.
    /// </summary>
    bool IsSolved(int[,] sudoku);
  }
}
