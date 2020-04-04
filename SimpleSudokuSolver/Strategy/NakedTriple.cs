using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy looks for three cells in the same row / column / block that contain IN TOTAL three candidates.
  /// Each of the three cells can contain two or three candidates.
  /// If such three cells are found, then the three candidate values cannot be in any other cell in the same row / column / block.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/naked_pair.html
  /// - http://www.sudokuwiki.org/Naked_Candidates
  /// </remarks>
  public class NakedTriple : ISudokuSolverStrategy
  {
    public string StrategyName => "Naked Triple";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetNakedTripleEliminations(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetNakedTripleEliminations(column.Cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        eliminations.AddRange(GetNakedTripleEliminations(block.Cells.OfType<Cell>(), sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), StrategyName) :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetNakedTripleEliminations(IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var eliminations = new List<SingleStepSolution.Candidate>();

      // we need to have at least 3 cells which have 2 or 3 possible potential values
      var nakedTripleCandidates = cellsWithNoValue.Where(x => x.CanBe.Count == 2 || x.CanBe.Count == 3).ToArray();
      if (nakedTripleCandidates.Length < 3)
        return eliminations;

      for (int i = 0; i < nakedTripleCandidates.Length - 2; i++)
      {
        Cell first = nakedTripleCandidates[i];

        for (int j = i + 1; j < nakedTripleCandidates.Length - 1; j++)
        {
          Cell second = nakedTripleCandidates[j];

          for (int k = j + 1; k < nakedTripleCandidates.Length; k++)
          {
            Cell third = nakedTripleCandidates[k];

            var distinctPotentialCellValuesInCandidates = GetDistinctPotentialCellValuesInCandidates(
              first.CanBe, second.CanBe, third.CanBe);

            if (distinctPotentialCellValuesInCandidates.Length == 3)
            {
              var nakedTriple = new Tuple<Cell, Cell, Cell>(first, second, third);

              eliminations.AddRange(GetNakedTripleEliminationsCore(
                nakedTriple, distinctPotentialCellValuesInCandidates, cellsWithNoValue, sudokuPuzzle));
            }
          }
        }
      }

      return eliminations;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetNakedTripleEliminationsCore(
      Tuple<Cell, Cell, Cell> nakedTriple, int[] distinctPotentialCellValuesInCandidates, Cell[] cellsWithNoValue, SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var cellWithNoValue in cellsWithNoValue)
      {
        if (nakedTriple.Item1 == cellWithNoValue || nakedTriple.Item2 == cellWithNoValue || nakedTriple.Item3 == cellWithNoValue)
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
    /// Flattens the array of enumerables into a single array and returns unique items of that array (no repetition).
    /// </summary>
    private int[] GetDistinctPotentialCellValuesInCandidates(params IEnumerable<int>[] items)
    {
      return items.SelectMany(x => x).Distinct().ToArray();
    }
  }
}
