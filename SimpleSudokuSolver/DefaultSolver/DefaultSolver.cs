using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Default sudoku solver.
  /// </summary>
  public partial class DefaultSolver : ISudokuSolver
  {
    private SudokuPuzzle _sudokuPuzzleAfterFailedSolveSingleStep;

    /// <inheritdoc />
    public SudokuPuzzle Solve(int[,] sudoku)
    {
      return Solve(sudoku, out _);
    }

    /// <inheritdoc />
    public SudokuPuzzle Solve(int[,] sudoku, out SingleStepSolution[] steps)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var stepsList = new List<SingleStepSolution>();

      while (!IsSolved(sudokuPuzzle))
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
          stepsList.Add(solution);
        }
      }

      steps = stepsList.ToArray();
      return sudokuPuzzle;
    }

    /// <inheritdoc />
    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      if (IsSolved(sudokuPuzzle))
        return null;

      var solvingTechniques = new Func<SudokuPuzzle, SingleStepSolution>[]
      {
        // Ordered from the simples to the most complex
        SingleInRow,
        SingleInColumn,
        SingleInBlock,
        HiddenSingle,
        NakedSingle,
        LockedCandidates,
        NakedPair,
        NakedTriple
      };

      foreach (var technique in solvingTechniques)
      {
        var elimination = CandidateElimination(sudokuPuzzle);
        if (elimination != null)
          return elimination;

        var solution = technique(sudokuPuzzle);
        if (solution != null)
          return solution;
      }

      _sudokuPuzzleAfterFailedSolveSingleStep = sudokuPuzzle;
      return null;
    }

    /// <inheritdoc />
    public bool IsSolved(int[,] sudoku)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      return IsSolved(sudokuPuzzle);
    }

    private bool IsSolved(SudokuPuzzle sudokuPuzzle)
    {
      return Validation.IsPuzzleSolved(sudokuPuzzle);
    }
  }
}
