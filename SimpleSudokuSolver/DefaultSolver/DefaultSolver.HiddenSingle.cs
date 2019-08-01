using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution HiddenSingle(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithValue = row.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          cell.CannotBe.AddRange(cellsWithValue.Select(x => x.Value));
        }
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithValue = column.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var forbiddenValues = cell.CannotBe.Union(cellsWithValue.Select(x => x.Value)).ToArray();
          cell.CannotBe.Clear();
          cell.CannotBe.AddRange(forbiddenValues);
        }
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockCells = block.Cells.OfType<Cell>();
        var cellsWithValue = blockCells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = blockCells.Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var forbiddenValues = cell.CannotBe.Union(cellsWithValue.Select(x => x.Value)).OrderBy(x => x).ToArray();
          cell.CannotBe.Clear();
          cell.CannotBe.AddRange(forbiddenValues);
        }
      }

      return HiddenSingleCore(sudokuPuzzle, "HiddenSingle");
    }

    private SingleStepSolution HiddenSingleCore(SudokuPuzzle sudokuPuzzle, string strategyName)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var rowSolution = HiddenSingleCore(sudokuPuzzle, row.Cells, strategyName);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = HiddenSingleCore(sudokuPuzzle, column.Cells, strategyName);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = HiddenSingleCore(sudokuPuzzle, block.Cells.OfType<Cell>(), strategyName);
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution HiddenSingleCore(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells, string strategyName)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      foreach (var cellWithNoValue in cellsWithNoValue)
      {
        // Possible values in cellWithNoValue
        var possibleValues = sudokuPuzzle.PossibleCellValues.Except(cellWithNoValue.CannotBe).ToArray();

        // If a possible value cannot be in any other empty cell in the block, it must be in this cellWithNoValue
        foreach (var possibleValue in possibleValues)
        {
          var allOtherCells = cellsWithNoValue.Except(new[] { cellWithNoValue }).ToArray();
          if (allOtherCells.All(x => x.CannotBe.Contains(possibleValue)))
          {
            var cellIndex = sudokuPuzzle.GetCellIndex(cellWithNoValue);
            return new SingleStepSolution(cellIndex.RowIndex, cellIndex.ColumnIndex, possibleValue,
              $"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {possibleValue} [{strategyName}]");
          }
        }
      }

      return null;
    }
  }
}
