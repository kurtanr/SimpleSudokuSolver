using System;
using System.Linq;

namespace SimpleSudokuSolver.Model
{
  internal static class Validation
  {
    private static readonly int NumberOfRowsOrColumnsInPuzzle = 9;
    private static readonly int MaximumRowOrColumnIndexInPuzzle = NumberOfRowsOrColumnsInPuzzle - 1;

    private static readonly int NumberOfRowsOrColumnsInBlock = 3;
    private static readonly int NumberOfBlockRowsOrBlockColumnsInPuzzle = 3;
    private static readonly int MaximumBlockRowOrBlockColumnIndexInPuzzle = NumberOfBlockRowsOrBlockColumnsInPuzzle - 1;

    private static readonly int NumberOfCellsInPuzzle = NumberOfRowsOrColumnsInPuzzle * NumberOfRowsOrColumnsInPuzzle;
    private static readonly int MaximumValueInCell = 9;

    internal static void ValidateSudoku(int[,] sudoku)
    {
      if (sudoku.GetLength(0) != NumberOfRowsOrColumnsInPuzzle || sudoku.GetLength(1) != NumberOfRowsOrColumnsInPuzzle)
        throw new ArgumentException(
          $"Puzzle must consist of {NumberOfCellsInPuzzle} cells " +
          $"(organized in {NumberOfRowsOrColumnsInPuzzle} rows and {NumberOfRowsOrColumnsInPuzzle} columns)",
          nameof(sudoku));

      foreach (var cellValue in sudoku)
        ValidateCellValue(cellValue);
    }

    internal static void ValidateCellValue(int value)
    {
      if (value < 0 || value > MaximumValueInCell)
        throw new ArgumentException($"Allowed values are 0-{MaximumValueInCell}", nameof(value));
    }

    internal static void ValidateNumberOfCellsInRowOrColumn(int numberOfCells)
    {
      if (numberOfCells != NumberOfRowsOrColumnsInPuzzle)
        throw new ArgumentException(
          $"Rows and columns must consist of {NumberOfRowsOrColumnsInPuzzle} cells",
          nameof(numberOfCells));
    }

    internal static void ValidateNumberOfRowsOrColumnsInBlock(int numberOfRowsOrColumnsInBlock)
    {
      if (numberOfRowsOrColumnsInBlock != NumberOfRowsOrColumnsInBlock)
        throw new ArgumentException(
          $"Block must consist of {NumberOfRowsOrColumnsInBlock} rows and {NumberOfRowsOrColumnsInBlock} columns",
          nameof(numberOfRowsOrColumnsInBlock));
    }

    internal static void ValidateRowOrColumnIndexInPuzzle(int index)
    {
      if (index < 0 || index > MaximumRowOrColumnIndexInPuzzle)
        throw new ArgumentException($"Allowed values are 0-{MaximumRowOrColumnIndexInPuzzle}", nameof(index));
    }

    internal static void ValidateBlockRowOrBlockColumnIndex(int index)
    {
      if (index < 0 || index > MaximumBlockRowOrBlockColumnIndexInPuzzle)
        throw new ArgumentException($"Allowed values are 0-{MaximumBlockRowOrBlockColumnIndexInPuzzle}", nameof(index));
    }

    internal static bool IsPuzzleSolved(SudokuPuzzle sudokuPuzzle)
    {
      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        var row = sudokuPuzzle.Rows[i];
        var isValidRow = AreRowOrColumnCellsValid(row.Cells);

        var column = sudokuPuzzle.Columns[i];
        var isValidColumn = AreRowOrColumnCellsValid(column.Cells);

        if (!isValidRow || !isValidColumn)
          return false;
      }
      return true;
    }

    private static bool AreRowOrColumnCellsValid(Cell[] cells)
    {
      if (cells.Count(x => !x.HasValue) > 0)
        return false;

      var values = cells.Select(x => x.Value).Distinct().ToArray();
      return values.Length == 9;
    }
  }
}
