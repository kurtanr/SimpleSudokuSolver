using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.PuzzleProviders;
using System;
using System.IO;

namespace SimpleSudokuSolver.Tests.PuzzleProviders
{
  [TestFixture]
  public class SudokuFilePuzzleProviderTests
  {
    [Test]
    public void SudokuFilePuzzleProvider_ThrowsExceptionForEmptyPath_Test()
    {
      Assert.That(() => new SudokuFilePuzzleProvider(null), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new SudokuFilePuzzleProvider(string.Empty), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void SudokuFilePuzzleProvider_WorksWithAllFormats_Test()
    {
      var filePath = Path.GetTempFileName();
      var separators = new[] { ",", " ", "\t", "" };
      var sudoku = new int[9, 9];
      sudoku[1, 2] = 5;

      foreach(var separator in separators)
      {
        SaveSudokuUsingSeparator(sudoku, separator, filePath);
        var provider = new SudokuFilePuzzleProvider(filePath);
        var sudokuPuzzle = provider.GetPuzzle();
        Assert.That(sudokuPuzzle[1, 2], Is.EqualTo(5));
      }

      File.Delete(filePath);
    }

    private void SaveSudokuUsingSeparator(int[,] sudoku, string separator, string filePath)
    {
      var sudokuToString = new SudokuPuzzle(sudoku).ToString();
      var sudokuToStringWithSeparator = sudokuToString.Replace(" ", separator);
      File.WriteAllText(filePath, sudokuToStringWithSeparator);
    }
  }
}
