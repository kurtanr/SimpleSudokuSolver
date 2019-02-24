using System.Linq;

namespace SimpleSudokuSolver.Model
{
  public class SudokuPuzzle
  {
    public Cell[,] Cells { get; }
    public Row[] Rows { get; }
    public Column[] Columns { get; }
    public Block[,] Blocks { get; }
    public int[] PossibleCellValues { get; }

    public int NumberOfRowsOrColumnsInPuzzle { get; }
    public int NumberOfRowsOrColumnsInBlock { get; }

    public SudokuPuzzle(int[,] sudoku)
    {
      Validation.ValidateSudoku(sudoku);

      NumberOfRowsOrColumnsInPuzzle = sudoku.GetLength(0);

      // We assume a square puzzle.
      NumberOfRowsOrColumnsInBlock = (int)System.Math.Sqrt(NumberOfRowsOrColumnsInPuzzle);

      Cells = new Cell[NumberOfRowsOrColumnsInPuzzle, NumberOfRowsOrColumnsInPuzzle];
      Rows = new Row[NumberOfRowsOrColumnsInPuzzle];
      Columns = new Column[NumberOfRowsOrColumnsInPuzzle];
      Blocks = new Block[NumberOfRowsOrColumnsInBlock, NumberOfRowsOrColumnsInBlock];
      PossibleCellValues = Enumerable.Range(1, NumberOfRowsOrColumnsInPuzzle).ToArray();

      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        Rows[i] = new Row(i, NumberOfRowsOrColumnsInPuzzle);

        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (i == 0)
          {
            Columns[j] = new Column(j, NumberOfRowsOrColumnsInPuzzle);
          }

          Cells[i, j] = new Cell(sudoku[i, j]);
          Columns[j].Cells[i] = Cells[i, j];
          Rows[i].Cells[j] = Cells[i, j];

          var blockRowIndex = i / NumberOfRowsOrColumnsInBlock;
          var blockColumnIndex = j / NumberOfRowsOrColumnsInBlock;

          if (Blocks[blockRowIndex, blockColumnIndex] == null)
          {
            Blocks[blockRowIndex, blockColumnIndex] = new Block(
              blockRowIndex, blockColumnIndex, NumberOfRowsOrColumnsInBlock);
          }

          Blocks[blockRowIndex, blockColumnIndex].Cells[i % NumberOfRowsOrColumnsInBlock, j % NumberOfRowsOrColumnsInBlock] = Cells[i, j];
        }
      }
    }

    /// <summary>
    /// Converts puzzle into a 2D integer array.
    /// </summary>
    /// <returns>2D integer array where values represent values of cells in the puzzle.</returns>
    public int[,] ToIntArray()
    {
      var result = new int[NumberOfRowsOrColumnsInPuzzle, NumberOfRowsOrColumnsInPuzzle];

      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          result[i, j] = Cells[i, j].Value;
        }
      }
      return result;
    }

    /// <summary>
    /// Returns zero-based row and column index of the <paramref name="cell"/>.
    /// Returns -1 for both row and column index if <paramref name="cell"/> is not part of the puzzle.
    /// </summary>
    public (int RowIndex, int ColumnIndex) GetCellIndex(Cell cell)
    {
      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (Cells[i, j] == cell)
            return (i, j);
        }
      }

      return (-1, -1);
    }

    public override string ToString() => Formatter.PuzzleToString(this);
  }
}
