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
    /// Progresses solution of the sudoku puzzle by one step.
    /// Single step solution can be:
    /// - promotion of candidate value to result value (<see cref="SingleStepSolution.Result"/>)
    /// - elimination of candidate values (<see cref="SingleStepSolution.Eliminations"/>)
    /// - null if no progress can be made
    /// </summary>
    /// <returns><see cref="SingleStepSolution"/> instance or null if no progress can be made.</returns>
    SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle);
  }
}
