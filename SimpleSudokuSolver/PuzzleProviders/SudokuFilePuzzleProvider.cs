using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleSudokuSolver.PuzzleProviders
{
  /// <summary>
  /// Reads sudoku puzzle from file.
  /// Each row in the file represents one row of the puzzle.
  /// Values in the row are:
  /// - separated using either comma, space or tab
  /// - or not separated at all
  /// Lines starting with '#' are ignored.
  /// </summary>
  public class SudokuFilePuzzleProvider : ISudokuPuzzleProvider
  {
    private readonly string _filePath;

    public SudokuFilePuzzleProvider(string filePath)
    {
      if (string.IsNullOrWhiteSpace(filePath))
        throw new ArgumentException("Empty file path", nameof(filePath));

      _filePath = filePath;
    }

    /// <inheritdoc />
    public int[,] GetPuzzle()
    {
      var lines = File.ReadAllLines(_filePath).Where(x =>
      {
        var trimmed = x.Trim();
        if (trimmed.StartsWith("#"))
          return false;

        return !string.IsNullOrWhiteSpace(trimmed);
      }).ToArray();
      var firstRowElements = GetRowElements(lines[0]);

      int rowCount = lines.Length;
      int columnCount = firstRowElements.Length;

      var values = new int[rowCount, columnCount];

      for (int i = 0; i < rowCount; i++)
      {
        var elements = GetRowElements(lines[i]);
        for (int j = 0; j < columnCount; j++)
        {
          values[i, j] = elements[j];
        }
      }

      return values;
    }

    private int[] GetRowElements(string row)
    {
      var separators = new[] { ",", " ", "\t" };

      // if there are no separators, add them
      if (!separators.Any(x => row.Contains(x)))
      {
        row = Regex.Replace(row, ".{1}", "$0,");
      }

      return row.Split(separators, StringSplitOptions.RemoveEmptyEntries)
        .Where(x => !string.IsNullOrWhiteSpace(x.Trim()))
        .Select(x => int.Parse(x.Trim()))
        .ToArray();
    }
  }
}
