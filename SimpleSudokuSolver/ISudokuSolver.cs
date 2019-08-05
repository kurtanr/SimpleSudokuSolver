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
    /// Progresses solution of the sudoku puzzle by one step.
    /// Single step solution can be:
    /// - promotion of candidate value to result value (<see cref="SingleStepSolution.Result"/>)
    /// - elimination of candidate values (<see cref="SingleStepSolution.Eliminations"/>)
    /// </summary>
    /// <returns>null if no progress can be made.</returns>
    SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle);

    /// <summary>
    /// Returns whether the puzzle is solved or not.
    /// </summary>
    bool IsSolved(int[,] sudoku);
  }
}
