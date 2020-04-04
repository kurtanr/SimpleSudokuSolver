using System.Collections.Generic;

namespace SimpleSudokuSolver.Model
{
  /// <summary>
  /// Single cell in a sudoku puzzle.
  /// </summary>
  public class Cell
  {
    private int _value;

    /// <summary>
    /// Value of the cell.
    /// 0 represents an unknown value.
    /// </summary>
    public int Value
    {
      get => _value;
      set
      {
        Validation.ValidateCellValue(value);
        _value = value;

        CanBe.Clear();
      }
    }

    /// <summary>
    /// Zero-based row index of cell in the puzzle.
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// Zero-based column index of cell in the puzzle.
    /// </summary>
    public int ColumnIndex { get; }

    /// <summary>
    /// List of values which are allowed in this cell.
    /// Empty if cell has a value.
    /// </summary>
    public List<int> CanBe { get; } = new List<int>();

    public bool HasValue => _value > 0;

    public Cell(int value, int rowIndex, int columnIndex)
    {
      Value = value;
      RowIndex = rowIndex;
      ColumnIndex = columnIndex;
    }

    public override string ToString() => Formatter.CellToString(this);
  }
}
