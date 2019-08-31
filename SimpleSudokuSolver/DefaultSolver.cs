using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Default sudoku solver.
  /// </summary>
  public class DefaultSolver : ISudokuSolver
  {
    private SudokuPuzzle _sudokuPuzzleAfterFailedSolveSingleStep;
    private readonly BasicElimination _basicElimination = new BasicElimination();
    private readonly ISudokuSolverStrategy[] _strategies;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="strategies">
    /// Strategies used to try and solve the puzzle.
    /// If no strategies are provided, a default set of strategies is used.
    /// </param>
    public DefaultSolver(params ISudokuSolverStrategy[] strategies)
    {
      if (strategies.Length > 0)
      {
        _strategies = strategies;
      }
      else
      {
        _strategies = new ISudokuSolverStrategy[]
        {
          // Ordered from the simplest to the most complex
          new SingleInCells(),
          new HiddenSingle(),
          new NakedSingle(),
          new LockedCandidatesPointing(),
          new LockedCandidatesClaiming(),
          new NakedPair(),
          new NakedTriple(),
          new NakedQuad(),
          new HiddenPair(),
          new HiddenTriple(),
          new HiddenQuad()
        };
      }
    }

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
          _sudokuPuzzleAfterFailedSolveSingleStep = sudokuPuzzle;
        }
      }

      return sudokuPuzzle;
    }

    /// <inheritdoc />
    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      if (sudokuPuzzle.IsSolved())
        return null;

      foreach (var strategy in _strategies)
      {
        var elimination = _basicElimination.SolveSingleStep(sudokuPuzzle);
        if (elimination != null)
          return elimination;

        var solution = strategy.SolveSingleStep(sudokuPuzzle);
        if (solution != null)
          return solution;
      }

      return null;
    }
  }
}
