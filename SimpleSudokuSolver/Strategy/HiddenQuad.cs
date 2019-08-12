using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy looks for four cells in the same row / column / block that have IN TOTAL four candidate values 
  /// that cannot be in any other cell of the same row / column / block.
  /// If such four cells are found, all other candidate values from those four cells can be removed.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/hidden_pair.html
  /// - http://www.sudokuwiki.org/Hidden_Candidates
  /// </remarks>
  public class HiddenQuad : HiddenPairTripleQuadBase, ISudokuSolverStrategy
  {
    public string StrategyName => "Hidden Quad";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      return GetSingleStepSolution(sudokuPuzzle, StrategyName);
    }

    protected override IEnumerable<SingleStepSolution.Candidate> GetHiddenEliminations(
      IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var hiddenCandidates = GetHiddenCandidates(cellsWithNoValue, sudokuPuzzle, 2, 3, 4);
      var eliminations = new List<SingleStepSolution.Candidate>();

      for (int i = 1; i <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - 3; i++)
      {
        for (int j = i + 1; j <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - 2; j++)
        {
          for (int k = j + 1; k <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - 1; k++)
          {
            for (int m = k + 1; m <= sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; m++)
            {
              if (hiddenCandidates.ContainsKey(i) &&
                hiddenCandidates.ContainsKey(j) &&
                hiddenCandidates.ContainsKey(k) &&
                hiddenCandidates.ContainsKey(m))
              {
                var union = hiddenCandidates[i].Union(hiddenCandidates[j]).
                  Union(hiddenCandidates[k]).Union(hiddenCandidates[m]).Distinct().ToArray();
                if (union.Length == 4)
                {
                  foreach (var cell in union)
                  {
                    eliminations.AddRange(GetEliminations(cell, sudokuPuzzle, i, j, k, m));
                  }
                }
              }
            }
          }
        }
      }

      return eliminations;
    }
  }
}
