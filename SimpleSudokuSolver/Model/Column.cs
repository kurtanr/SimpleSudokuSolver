namespace SimpleSudokuSolver.Model
{
  /// <summary>
  /// Single column in a sudoku puzzle.
  /// </summary>
  public class Column
  {
    /// <summary>
    /// Cells in the column.
    /// </summary>
    public Cell[] Cells { get; }

    /// <summary>
    /// Zero-based index of the column.
    /// </summary>
    public int ColumnIndex { get; }

    public Column(int columnIndex, int numberOfCells)
    {
      Validation.ValidateRowOrColumnIndexInPuzzle(columnIndex);
      Validation.ValidateNumberOfCellsInRowOrColumn(numberOfCells);

      ColumnIndex = columnIndex;
      Cells = new Cell[numberOfCells];
    }

    public override string ToString() => Formatter.ColumnToString(this);
  }
}
