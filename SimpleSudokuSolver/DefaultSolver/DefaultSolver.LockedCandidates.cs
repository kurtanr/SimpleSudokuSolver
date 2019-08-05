using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution LockedCandidates(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockCells = block.Cells.OfType<Cell>();
        var cellsWithValue = blockCells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = blockCells.Where(x => !x.HasValue).ToArray();
        var possibleCellValuesInBlock = sudokuPuzzle.PossibleCellValues.Except(
          cellsWithValue.Select(x => x.Value)).ToArray();

        var valuesWhichCanAppearOnlyInSingleBlockRow = GetValuesWhichCanAppearOnlyInSingleBlockRow(
          sudokuPuzzle, block, possibleCellValuesInBlock);

        var valuesWhichCanAppearOnlyInSingleBlockColumn = GetValuesWhichCanAppearOnlyInSingleBlockColumn(
          sudokuPuzzle, block, possibleCellValuesInBlock);

        foreach (var value in valuesWhichCanAppearOnlyInSingleBlockRow)
        {
          int cellValue = value.Item1;
          int rowIndex = block.BlockRowIndex * sudokuPuzzle.NumberOfRowsOrColumnsInBlock + value.Item2;
          foreach (var cell in sudokuPuzzle.Rows[rowIndex].Cells)
          {
            if (blockCells.Contains(cell))
              continue;

            if (cell.CanBe.Contains(cellValue))
            {
              cell.CanBe.Remove(cellValue);
              var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cell);
              eliminations.Add(new SingleStepSolution.Candidate(RowIndex, ColumnIndex, cellValue));
            }
          }
        }

        foreach (var value in valuesWhichCanAppearOnlyInSingleBlockColumn)
        {
          int cellValue = value.Item1;
          int columnIndex = block.BlockColumnIndex * sudokuPuzzle.NumberOfRowsOrColumnsInBlock + value.Item2;
          foreach (var cell in sudokuPuzzle.Columns[columnIndex].Cells)
          {
            if (blockCells.Contains(cell))
              continue;

            if (cell.CanBe.Contains(cellValue))
            {
              cell.CanBe.Remove(cellValue);
              var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cell);
              eliminations.Add(new SingleStepSolution.Candidate(RowIndex, ColumnIndex, cellValue));
            }
          }
        }
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.ToArray(), "Locked Candidates") :
        null;
    }

    /// <summary>
    /// Returns an array of values that can appear only in the single row of the block (and not other rows).
    /// </summary>
    /// <returns>Item1=value, Item2=rowIndex</returns>
    private Tuple<int, int>[] GetValuesWhichCanAppearOnlyInSingleBlockRow(
      SudokuPuzzle sudokuPuzzle, Block block, int[] possibleCellValuesInBlock)
    {
      // Item1=value, Item2=rowsThatContainIt
      var valueAndRowsThatContainIt = new List<Tuple<int, List<int>>>();

      foreach (var possibleCellValue in possibleCellValuesInBlock)
      {
        var rowIndexes = new List<int>();

        for (int i = 0; i < sudokuPuzzle.NumberOfRowsOrColumnsInBlock; i++)
        {
          var valueFound = false;

          for (int j = 0; j < sudokuPuzzle.NumberOfRowsOrColumnsInBlock; j++)
          {
            if (block.Cells[i, j].CanBe.Contains(possibleCellValue))
            {
              valueFound = true;
              break;
            }
          }

          if (valueFound)
            rowIndexes.Add(i);
        }

        valueAndRowsThatContainIt.Add(new Tuple<int, List<int>>(possibleCellValue, rowIndexes));
      }

      return valueAndRowsThatContainIt
        .Where(x => x.Item2.Count == 1)
        .Select(x => new Tuple<int, int>(x.Item1, x.Item2.Single())).ToArray();
    }

    /// <summary>
    /// Returns an array of values that can appear only in the single column of the block (and not other columns).
    /// </summary>
    /// <returns>Item1=value, Item2=columnIndex</returns>
    private Tuple<int, int>[] GetValuesWhichCanAppearOnlyInSingleBlockColumn(
      SudokuPuzzle sudokuPuzzle, Block block, int[] possibleCellValuesInBlock)
    {
      // Item1=value, Item2=columnsThatContainIt
      var valueAndColumnsThatContainIt = new List<Tuple<int, List<int>>>();

      foreach (var possibleCellValue in possibleCellValuesInBlock)
      {
        var columnIndexes = new List<int>();

        for (int i = 0; i < sudokuPuzzle.NumberOfRowsOrColumnsInBlock; i++)
        {
          var valueFound = false;

          for (int j = 0; j < sudokuPuzzle.NumberOfRowsOrColumnsInBlock; j++)
          {
            if (block.Cells[j, i].CanBe.Contains(possibleCellValue))
            {
              valueFound = true;
              break;
            }
          }

          if (valueFound)
            columnIndexes.Add(i);
        }

        valueAndColumnsThatContainIt.Add(new Tuple<int, List<int>>(possibleCellValue, columnIndexes));
      }

      return valueAndColumnsThatContainIt
        .Where(x => x.Item2.Count == 1)
        .Select(x => new Tuple<int, int>(x.Item1, x.Item2.Single())).ToArray();
    }
  }
}
