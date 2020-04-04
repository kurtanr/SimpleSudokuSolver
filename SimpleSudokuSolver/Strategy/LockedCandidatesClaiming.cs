using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy is examining rows and columns, and is looking for candidates which are grouped together in just one block.
  /// If such a candidate is found, we can exclude it from the rest of the block.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - http://www.sudokuwiki.org/Intersection_Removal (Box Line Reduction)
  /// </remarks>
  public class LockedCandidatesClaiming : ISudokuSolverStrategy
  {
    public string StrategyName => "Locked Candidates (Claiming)";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var cellCandidatePairsPerRow = new List<Tuple<Cell, int>>();
      var cellCandidatePairsPerColumn = new List<Tuple<Cell, int>>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        cellCandidatePairsPerRow.AddRange(GetCellCandidatePairsWhichAppearOnlyInSingleBlock(
          row.Cells, x => x.ColumnIndex));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        cellCandidatePairsPerColumn.AddRange(GetCellCandidatePairsWhichAppearOnlyInSingleBlock(
          column.Cells, x => x.RowIndex));
      }

      var eliminations = new List<SingleStepSolution.Candidate>();
      eliminations.AddRange(GetEliminations(cellCandidatePairsPerRow, true, sudokuPuzzle));
      eliminations.AddRange(GetEliminations(cellCandidatePairsPerColumn, false, sudokuPuzzle));

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), StrategyName) :
        null;
    }

    /// <summary>
    /// Method examines all the cells from <paramref name="cellsOfSingleRowOrColumn"/> and
    /// returns all the cells that have a candidate value that is present only in a single block.
    /// </summary>
    /// <param name="cellsOfSingleRowOrColumn">Cells of a row or cells of a column.</param>
    /// <param name="getIndexOfColumnOrRow">
    /// Returns column index of a cell if <paramref name="cellsOfSingleRowOrColumn"/> 
    /// are part of a row and row index if they are part of a column.
    /// That index is later used to determine which block the cells belongs to.
    /// </param>
    /// <returns>Item1 of tuple is cell, and Item2 is cell's candidate value.</returns>
    private IEnumerable<Tuple<Cell, int>> GetCellCandidatePairsWhichAppearOnlyInSingleBlock(
      Cell[] cellsOfSingleRowOrColumn, Func<Cell, int> getIndexOfColumnOrRow)
    {
      var cellsWithNoValue = cellsOfSingleRowOrColumn.Where(x => !x.HasValue).ToArray();
      var indexesPerCandidate = new Dictionary<int, HashSet<int>>();

      foreach (var cellWithNoValue in cellsWithNoValue)
      {
        var index = getIndexOfColumnOrRow(cellWithNoValue);

        foreach (var candidate in cellWithNoValue.CanBe)
        {
          if (!indexesPerCandidate.ContainsKey(candidate))
          {
            indexesPerCandidate[candidate] = new HashSet<int>();
          }
          indexesPerCandidate[candidate].Add(index);
        }
      }

      var candidateValuesWhichAppearOnlyInSingleBlock = new List<int>();

      foreach (var item in indexesPerCandidate)
      {
        // We assume block contains 3 cells
        var allInSingleBlock = item.Value.Select(x => x / 3).Distinct().Count() == 1;
        if (allInSingleBlock)
        {
          candidateValuesWhichAppearOnlyInSingleBlock.Add(item.Key);
        }
      }

      var result = new List<Tuple<Cell, int>>();

      foreach (var item in candidateValuesWhichAppearOnlyInSingleBlock)
      {
        // We could return all the cells, but just one is enough to eliminate other candidates
        var cell = cellsWithNoValue.First(x => x.CanBe.Contains(item));
        result.Add(new Tuple<Cell, int>(cell, item));
      }

      return result;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetEliminations(
      IEnumerable<Tuple<Cell, int>> cellCandidatePairs, bool perRow, SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var cellCandidatePair in cellCandidatePairs)
      {
        var cell = cellCandidatePair.Item1;
        var candidate = cellCandidatePair.Item2;
        var blockIndex = sudokuPuzzle.GetBlockIndex(cell);
        var block = sudokuPuzzle.Blocks[blockIndex.RowIndex, blockIndex.ColumnIndex];

        foreach (var blockCell in block.Cells)
        {
          // Ignore the same cell, or block cells that do not contain candidate
          if (blockCell == cell || !blockCell.CanBe.Contains(candidate))
          {
            continue;
          }

          // If outside cells are part of the same row,
          // ignore block cells that are part of that same row (similar for columns)
          if ((perRow && cell.RowIndex == blockCell.RowIndex) ||
            (!perRow && cell.ColumnIndex == blockCell.ColumnIndex))
          {
            continue;
          }

          eliminations.Add(new SingleStepSolution.Candidate(
            blockCell.RowIndex, blockCell.ColumnIndex, candidate));
        }
      }

      return eliminations;
    }
  }
}
