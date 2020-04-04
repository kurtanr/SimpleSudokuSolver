using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy looks for four cells in the same row / column / block that contain IN TOTAL four candidates.
  /// Each of the four cells can contain two, three or four candidates.
  /// If such four cells are found, then the four candidate values cannot be in any other cell in the same row / column / block.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/naked_pair.html
  /// - http://www.sudokuwiki.org/Naked_Candidates
  /// </remarks>
  public class NakedQuad : ISudokuSolverStrategy
  {
    public string StrategyName => "Naked Quad";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetNakedQuadEliminations(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetNakedQuadEliminations(column.Cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        eliminations.AddRange(GetNakedQuadEliminations(block.Cells.OfType<Cell>(), sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), StrategyName) :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetNakedQuadEliminations(IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var eliminations = new List<SingleStepSolution.Candidate>();

      // we need to have at least 4 cells which have 2, 3 or 4 possible potential values
      var nakedQuadCandidates = cellsWithNoValue.Where(x => x.CanBe.Count >= 2 && x.CanBe.Count <= 4).ToArray();
      if (nakedQuadCandidates.Length < 4)
        return eliminations;

      for (int i = 0; i < nakedQuadCandidates.Length - 3; i++)
      {
        Cell first = nakedQuadCandidates[i];

        for (int j = i + 1; j < nakedQuadCandidates.Length - 2; j++)
        {
          Cell second = nakedQuadCandidates[j];

          for (int k = j + 1; k < nakedQuadCandidates.Length - 1; k++)
          {
            Cell third = nakedQuadCandidates[k];

            for (int m = k + 1; m < nakedQuadCandidates.Length; m++)
            {
              Cell fourth = nakedQuadCandidates[m];

              var distinctPotentialCellValuesInCandidates = GetDistinctPotentialCellValuesInCandidates(
                first.CanBe, second.CanBe, third.CanBe, fourth.CanBe);

              if (distinctPotentialCellValuesInCandidates.Length == 4)
              {
                var nakedQuad = new Tuple<Cell, Cell, Cell, Cell>(first, second, third, fourth);

                eliminations.AddRange(GetNakedQuadEliminationsCore(
                  nakedQuad, distinctPotentialCellValuesInCandidates, cellsWithNoValue, sudokuPuzzle));
              }
            }
          }
        }
      }

      return eliminations;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetNakedQuadEliminationsCore(
      Tuple<Cell, Cell, Cell, Cell> nakedQuad, int[] distinctPotentialCellValuesInCandidates,
      Cell[] cellsWithNoValue, SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var cellWithNoValue in cellsWithNoValue)
      {
        if (nakedQuad.Item1 == cellWithNoValue || nakedQuad.Item2 == cellWithNoValue ||
            nakedQuad.Item3 == cellWithNoValue || nakedQuad.Item4 == cellWithNoValue)
          continue;

        var finalItems = cellWithNoValue.CanBe.Intersect(distinctPotentialCellValuesInCandidates).ToArray();

        if (finalItems.Length > 0)
        {
          foreach (var finalItem in finalItems)
          {
            eliminations.Add(new SingleStepSolution.Candidate(cellWithNoValue.RowIndex, cellWithNoValue.ColumnIndex, finalItem));
          }
        }
      }
      return eliminations;
    }

    /// <summary>
    /// Flattens the array of enumerable into a single array and returns unique items of that array (no repetition).
    /// </summary>
    private int[] GetDistinctPotentialCellValuesInCandidates(params IEnumerable<int>[] items)
    {
      return items.SelectMany(x => x).Distinct().ToArray();
    }
  }
}
