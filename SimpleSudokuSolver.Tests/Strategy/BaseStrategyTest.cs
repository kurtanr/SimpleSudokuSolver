using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _basicElimination = new BasicElimination();

    public SudokuPuzzle SolveUsingStrategy(SudokuPuzzle sudokuPuzzle, ISudokuSolverStrategy sudokuSolverStrategy)
    {
      var step = ExecuteStep(sudokuPuzzle, sudokuSolverStrategy);

      while (step != null)
      {
        sudokuPuzzle.ApplySingleStepSolution(step);
        step = ExecuteStep(sudokuPuzzle, sudokuSolverStrategy);
      }

      return sudokuPuzzle;
    }

    private SingleStepSolution ExecuteStep(SudokuPuzzle sudokuPuzzle, ISudokuSolverStrategy sudokuSolverStrategy)
    {
      var elimination = _basicElimination.SolveSingleStep(sudokuPuzzle);
      if (elimination != null)
      {
        sudokuPuzzle.ApplySingleStepSolution(elimination);
      }
      return sudokuSolverStrategy.SolveSingleStep(sudokuPuzzle);
    }
  }
}
