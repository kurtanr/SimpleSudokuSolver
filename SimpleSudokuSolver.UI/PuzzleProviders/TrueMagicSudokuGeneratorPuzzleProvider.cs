namespace SimpleSudokuSolver.UI.PuzzleProviders
{
  /// <summary>
  /// Uses <see cref="TrueMagic.SudokuGenerator.Generator"/> to generate sudoku puzzle.
  /// </summary>
  public class TrueMagicSudokuGeneratorPuzzleProvider : ISudokuPuzzleProvider
  {
    /// <inheritdoc />
    public int[,] GetPuzzle()
    {
      var generator = new TrueMagic.SudokuGenerator.Generator();
      const int blockSize = 3, boardSize = 9;
      var puzzle = new int[boardSize, boardSize];
      var sudoku = generator.Generate(blockSize, TrueMagic.SudokuGenerator.Level.Easy);

      for (int i = 0; i < boardSize; i++)
      {
        for (int j = 0; j < boardSize; j++)
        {
          puzzle[i, j] = sudoku.GetValue(i, j);
        }
      }

      return puzzle;
    }
  }
}
