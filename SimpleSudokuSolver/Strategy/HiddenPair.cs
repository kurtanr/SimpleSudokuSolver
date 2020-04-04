using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy looks for two cells in the same row / column / block that have two candidate values that cannot
  /// be in any other cell of the same row / column / block.
  /// If such two cells are found, all other candidate values from those two cells can be removed.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/hidden_pair.html
  /// - http://www.sudokuwiki.org/Hidden_Candidates
  /// </remarks>
  public class HiddenPair : HiddenPairTripleQuadBase, ISudokuSolverStrategy
  {
    public string StrategyName => "Hidden Pair";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      return GetSingleStepSolution(sudokuPuzzle, StrategyName);
    }

    protected override IEnumerable<SingleStepSolution.Candidate> GetHiddenEliminations(
      IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var hiddenCandidates = GetHiddenCandidates(cellsWithNoValue, sudokuPuzzle, 2);
      var eliminations = new List<SingleStepSolution.Candidate>();

      for (int i = 1; i <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - 1; i++)
      {
        for (int j = i + 1; j <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (hiddenCandidates.ContainsKey(i) &&
            hiddenCandidates.ContainsKey(j) &&
            hiddenCandidates[i].SequenceEqual(hiddenCandidates[j]))
          {
            eliminations.AddRange(GetEliminations(hiddenCandidates[i][0], i, j));
            eliminations.AddRange(GetEliminations(hiddenCandidates[i][1], i, j));
          }
        }
      }

      return eliminations;
    }
  }
}
