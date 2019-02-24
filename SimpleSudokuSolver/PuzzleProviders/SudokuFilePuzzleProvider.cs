using System;
using System.IO;
using System.Linq;

namespace SimpleSudokuSolver.PuzzleProviders
{
  /// <summary>
  /// Reads sudoku puzzle from file.
  /// Each row in the file represents one row of the puzzle.
  /// Values must be separated using either comma, space or tab.
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
      var lines = File.ReadAllLines(_filePath).Where(x => !string.IsNullOrWhiteSpace(x.Trim())).ToArray();
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
      var splitCharacters = new char[] { ',', ' ', '\t' };
      return row.Split(splitCharacters)
        .Where(x => !string.IsNullOrWhiteSpace(x.Trim()))
        .Select(x => int.Parse(x.Trim()))
        .ToArray();
    }
  }
}
