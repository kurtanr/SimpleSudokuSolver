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
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetEliminationsTriple(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetEliminationsTriple(column.Cells, sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.ToArray(), "Naked Triple") :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetEliminationsTriple(Cell[] cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var eliminations = new List<SingleStepSolution.Candidate>();

      // we need to have at least 3 cells which have 2 or 3 possible potential values
      var nakedTripleCandidates = cellsWithNoValue.Where(x => x.CanBe.Count == 2 || x.CanBe.Count == 3).ToArray();
      if (nakedTripleCandidates.Length < 3)
        return eliminations;

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

      if (nakedTriples.Count() > 0)
      {
        foreach (var nakedTriple in nakedTriples)
        {
          var distinctPotentialCellValuesInCandidates = GetDistinctPotentialCellValuesInCandidates(
            nakedTriple.Item1.CanBe, nakedTriple.Item2.CanBe, nakedTriple.Item3.CanBe);

          eliminations.AddRange(GetEliminationsTripleCore(
            nakedTriple, distinctPotentialCellValuesInCandidates, cellsWithNoValue, sudokuPuzzle));

          // if all three members of a naked triple belong to the same block, their values can be removed 
          // as potential values from all the other cells in the block
          var item1BlockIndex = sudokuPuzzle.GetBlockIndex(nakedTriple.Item1);
          var item2BlockIndex = sudokuPuzzle.GetBlockIndex(nakedTriple.Item2);
          var item3BlockIndex = sudokuPuzzle.GetBlockIndex(nakedTriple.Item3);

          if (item1BlockIndex.Equals(item2BlockIndex) && item1BlockIndex.Equals(item3BlockIndex) && item2BlockIndex.Equals(item3BlockIndex))
          {
            var block = sudokuPuzzle.Blocks[item1BlockIndex.RowIndex, item1BlockIndex.ColumnIndex];
            var cellWithNoValueInBlock = block.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();

            eliminations.AddRange(GetEliminationsTripleCore(
              nakedTriple, distinctPotentialCellValuesInCandidates, cellWithNoValueInBlock, sudokuPuzzle));
          }
        }
      }

      return eliminations;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetEliminationsTripleCore(
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
            if (cellWithNoValue.CanBe.Contains(finalItem))
            {
              cellWithNoValue.CanBe.Remove(finalItem);
              var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cellWithNoValue);
              eliminations.Add(new SingleStepSolution.Candidate(RowIndex, ColumnIndex, finalItem));
            }
          }
        }
      }
      return eliminations;
    }

    private int[] GetDistinctPotentialCellValuesInCandidates(params IEnumerable<int>[] items)
    {
      List<int> result = new List<int>();

      foreach (var item in items)
      {
        result.AddRange(item);
      }

      return result.Distinct().ToArray();
    }
  }
}
