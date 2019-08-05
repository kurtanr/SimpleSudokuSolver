using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution NakedSingle(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var rowSolution = NakedSingleCore(sudokuPuzzle, row.Cells);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = NakedSingleCore(sudokuPuzzle, column.Cells);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = NakedSingleCore(sudokuPuzzle, block.Cells.OfType<Cell>());
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution NakedSingleCore(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      // if cell can contain only one value, then it must contain that value
      foreach (var cell in cellsWithNoValue)
      {
        if (cell.CanBe.Count == 1)
        {
          var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cell);
          return new SingleStepSolution(RowIndex, ColumnIndex, cell.CanBe.Single(), "Naked Single");
        }
      }

      return null;
    }
  }
}
