using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;
using System.Collections.Generic;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Default sudoku solver.
  /// </summary>
  public partial class DefaultSolver : ISudokuSolver
  {
    private SudokuPuzzle _sudokuPuzzleAfterFailedSolveSingleStep;
    private BasicElimination _basicElimination = new BasicElimination();

    /// <inheritdoc />
    public SudokuPuzzle Solve(int[,] sudoku)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);

      while (!sudokuPuzzle.IsSolved())
      {
        var solution = SolveSingleStep(sudokuPuzzle);
        if (solution == null)
        {
          // Sudoku cannot be solved :(
          sudokuPuzzle = _sudokuPuzzleAfterFailedSolveSingleStep;
          break;
        }
        else
        {
          sudokuPuzzle.ApplySingleStepSolution(solution);
        }
      }

      return sudokuPuzzle;
    }

    /// <inheritdoc />
    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      if (sudokuPuzzle.IsSolved())
        return null;

      var solvingTechniques = new ISudokuSolverStrategy[]
      {
        // Ordered from the simples to the most complex
        new SingleInCells(),
        new HiddenSingle(),
        new NakedSingle(),
        new LockedCandidates(),
        new NakedPair(),
        new NakedTriple()
      };

      foreach (var technique in solvingTechniques)
      {
        var elimination = _basicElimination.SolveSingleStep(sudokuPuzzle);
        if (elimination != null)
          return elimination;

        var solution = technique.SolveSingleStep(sudokuPuzzle);
        if (solution != null)
          return solution;
      }

      _sudokuPuzzleAfterFailedSolveSingleStep = sudokuPuzzle;
      return null;
    }
  }
}
