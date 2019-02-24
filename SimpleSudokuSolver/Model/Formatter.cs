using System;
using System.Text;

namespace SimpleSudokuSolver.Model
{
  /// <summary>
  /// Converts sudoku entities to string.
  /// </summary>
  internal static class Formatter
  {
    internal static string CellToString(Cell cell)
    {
      return cell.Value.ToString();
    }

    internal static string BlockToString(Block block)
    {
      var sb = new StringBuilder();
      var rowCount = block.Cells.GetLength(0);
      var columnCount = block.Cells.GetLength(1);

      for (int i = 0; i < rowCount; i++)
      {
        for (int j = 0; j < columnCount; j++)
        {
          sb.Append(block.Cells[i, j].Value + " ");
        }
        sb.Append(Environment.NewLine);
      }

      return sb.ToString();
    }

    internal static string RowToString(Row row)
    {
      var sb = new StringBuilder();
      var columnCount = row.Cells.Length;

      for (int i = 0; i < columnCount; i++)
      {
        sb.Append(row.Cells[i].Value + " ");
      }

      sb.Append(Environment.NewLine);
      return sb.ToString();
    }

    internal static string ColumnToString(Column column)
    {
      var sb = new StringBuilder();
      var rowCount = column.Cells.Length;

      for (int i = 0; i < rowCount; i++)
      {
        sb.Append(column.Cells[i].Value + Environment.NewLine);
      }

      return sb.ToString();
    }

    internal static string PuzzleToString(SudokuPuzzle sudokuPuzzle)
    {
      var sb = new StringBuilder();
      var rowCount = sudokuPuzzle.Cells.GetLength(0);
      var columnCount = sudokuPuzzle.Cells.GetLength(1);

      for (int i = 0; i < rowCount; i++)
      {
        for (int j = 0; j < columnCount; j++)
        {
          sb.Append(sudokuPuzzle.Cells[i, j].Value + " ");
        }
        sb.Append(Environment.NewLine);
      }

      return sb.ToString();
    }
  }
}
