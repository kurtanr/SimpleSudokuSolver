using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution NakedSingle(SudokuPuzzle sudokuPuzzle)
    {
      int[] possibleCellValuesBasedOnRow, possibleCellValuesBasedOnColumn, possibleCellValuesBasedOnBlock;

      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithValue = row.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnRow = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          cell.CanBe.AddRange(possibleCellValuesBasedOnRow);
        }
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithValue = column.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnColumn = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var possibleValues = cell.CanBe.Intersect(possibleCellValuesBasedOnColumn).ToArray();
          cell.CanBe.Clear();
          cell.CanBe.AddRange(possibleValues);
        }
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockCells = block.Cells.OfType<Cell>();
        var cellsWithValue = blockCells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = blockCells.Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnBlock = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var possibleValues = cell.CanBe.Intersect(possibleCellValuesBasedOnBlock).ToArray();
          cell.CanBe.Clear();
          cell.CanBe.AddRange(possibleValues);
        }
      }

      return NakedSingleCore(sudokuPuzzle, "NakedSingle");
    }

    private SingleStepSolution NakedSingleCore(SudokuPuzzle sudokuPuzzle, string strategyName)
    {
      for (int i = 0; i < sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; j++)
        {
          var cell = sudokuPuzzle.Cells[i, j];
          if (!cell.HasValue && cell.CanBe.Count == 1)
            return new SingleStepSolution(i, j, cell.CanBe[0],
              $"Row {i + 1} Column {j + 1} Value {cell.CanBe[0]} [{strategyName}]");
        }
      }

      return null;
    }
  }
}
