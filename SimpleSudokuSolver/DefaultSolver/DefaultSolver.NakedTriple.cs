using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution NakedTriple(SudokuPuzzle sudokuPuzzle)
    {
      int reducedNumberOfCandidates = 0;

      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();
        if (NakedTripleCore(cellsWithNoValue))
          reducedNumberOfCandidates++;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();
        if (NakedTripleCore(cellsWithNoValue))
          reducedNumberOfCandidates++;
      }

      if (reducedNumberOfCandidates == 0)
        return null;

      return
        HiddenSingleCore(sudokuPuzzle, "HiddenSingle+NakedTriple") ??
        NakedSingleCore(sudokuPuzzle, "NakedSingle+NakedTriple");
    }

    private bool NakedTripleCore(Cell[] cellsWithNoValue)
    {
      // we need to have at least 3 cells which have 2 or 3 possible potential values
      var nakedTripleCandidates = cellsWithNoValue.Where(x => x.CanBe.Count == 2 || x.CanBe.Count == 3).ToArray();
      if (nakedTripleCandidates.Length < 3)
        return false;

      List<Tuple<Cell, Cell, Cell>> nakedTriples = new List<Tuple<Cell, Cell, Cell>>();

      for (int i = 0; i < nakedTripleCandidates.Length - 2; i++)
      {
        Cell first = nakedTripleCandidates[i];

        for (int j = i + 1; j < nakedTripleCandidates.Length - 1; j++)
        {
          Cell second = nakedTripleCandidates[j];
          if (first == second)
            continue;

          for (int k = j + 1; k < nakedTripleCandidates.Length; k++)
          {
            Cell third = nakedTripleCandidates[k];
            if (first == third || second == third)
              continue;

            var distinctPotentialCellValuesInCandidates = GetDistinctPotentialCellValuesInCandidates(
              first.CanBe, second.CanBe, third.CanBe);

            if (distinctPotentialCellValuesInCandidates.Length == 3)
            {
              nakedTriples.Add(new Tuple<Cell, Cell, Cell>(first, second, third));
            }
          }
        }
      }

      bool reducedNumberOfCandidates = false;

      if (nakedTriples.Count() > 0)
      {
        foreach (var nakedTriple in nakedTriples)
        {
          foreach (var cellWithNoValue in cellsWithNoValue)
          {
            if (nakedTriple.Item1 == cellWithNoValue || nakedTriple.Item2 == cellWithNoValue || nakedTriple.Item3 == cellWithNoValue)
              continue;

            var distinctPotentialCellValuesInCandidates = GetDistinctPotentialCellValuesInCandidates(
              nakedTriple.Item1.CanBe, nakedTriple.Item2.CanBe, nakedTriple.Item3.CanBe);

            var finalItems = cellWithNoValue.CanBe.Intersect(distinctPotentialCellValuesInCandidates).ToArray();

            if (finalItems.Length > 0)
            {
              foreach(var finalItem in finalItems)
              {
                cellWithNoValue.CanBe.Remove(finalItem);
                if (!cellWithNoValue.CannotBe.Contains(finalItem))
                {
                  cellWithNoValue.CannotBe.Add(finalItem);
                }
              }
              reducedNumberOfCandidates = true;
            }
          }
        }
      }

      return reducedNumberOfCandidates;
    }

    private int[] GetDistinctPotentialCellValuesInCandidates(params IEnumerable<int>[] items)
    {
      List<int> result = new List<int>();

      foreach(var item in items)
      {
        result.AddRange(item);
      }

      return result.Distinct().ToArray();
    }
  }
}
