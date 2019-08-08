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
          var prefix = (j == 0 ? "" : " ");

          if (block.Cells[i, j] != null)
          {
            sb.Append($"{prefix}{block.Cells[i, j].Value}");
          }
          else sb.Append($"{prefix} ");

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
        var prefix = (i == 0 ? "" : " ");

        if (row.Cells[i] != null)
        {
          sb.Append($"{prefix}{row.Cells[i].Value}");
        }
        else sb.Append($"{prefix} ");
      }

      return sb.ToString();
    }

    internal static string ColumnToString(Column column)
    {
      var sb = new StringBuilder();
      var columnCount = column.Cells.Length;

      for (int i = 0; i < columnCount; i++)
      {
        var value = column.Cells[i] != null ? column.Cells[i].Value.ToString() : " ";
        sb.Append($"{value}{Environment.NewLine}");
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
          var prefix = (j == 0 ? "" : " ");

          if (sudokuPuzzle.Cells[i, j] != null)
          {
            sb.Append($"{prefix}{sudokuPuzzle.Cells[i, j].Value}");
          }
          else sb.Append($"{prefix} ");

        }
        sb.Append(Environment.NewLine);
      }

      return sb.ToString();
    }
  }
}
