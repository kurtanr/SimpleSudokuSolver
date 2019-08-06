using SimpleSudokuSolver.Model;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Individual strategy for solving a sudoku puzzle.
  /// </summary>
  public interface ISudokuSolverStrategy
  {
    /// <summary>
    /// Progresses solution of the sudoku puzzle by one step.
    /// Returns null if no progress can be made.
    /// </summary>
    SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle);

    /// <summary>
    /// Strategy name.
    /// </summary>
    string StrategyName { get; }
  }
}
