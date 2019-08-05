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
        var rowSolution = HiddenSingleCore(sudokuPuzzle, row.Cells);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = HiddenSingleCore(sudokuPuzzle, column.Cells);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = HiddenSingleCore(sudokuPuzzle, block.Cells.OfType<Cell>());
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution HiddenSingleCore(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var usedValues = cells.Where(x => x.HasValue).Select(x => x.Value).ToArray();
      var possibleValues = sudokuPuzzle.PossibleCellValues.Except(usedValues).ToArray();

      // if a possible value can only be in one cell, then it must be in that cell
      foreach (var possibleValue in possibleValues)
      {
        var candidateCells = cellsWithNoValue.Where(x => x.CanBe.Contains(possibleValue)).ToArray();
        if(candidateCells.Length == 1)
        {
          var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(candidateCells[0]);
          return new SingleStepSolution(RowIndex, ColumnIndex, possibleValue, "Hidden Single");
        }
      }

      return null;
    }
  }
}
