namespace SimpleSudokuSolver.Model
{
  /// <summary>
  /// Single row in a sudoku puzzle.
  /// </summary>
  public class Row
  {
    /// <summary>
    /// Cells in the row.
    /// </summary>
    public Cell[] Cells { get; }

    /// <summary>
    /// Zero-based index of the row.
    /// </summary>
    public int RowIndex { get; }

    public Row(int rowIndex, int numberOfCells)
    {
      Validation.ValidateRowOrColumnIndexInPuzzle(rowIndex);
      Validation.ValidateNumberOfCellsInRowOrColumn(numberOfCells);

      RowIndex = rowIndex;
      Cells = new Cell[numberOfCells];
    }

    public override string ToString() => Formatter.RowToString(this);
  }
}
