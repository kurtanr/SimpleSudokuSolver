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
      }
    }

    /// <summary>
    /// List of values which are allowed in this cell.
    /// </summary>
    public List<int> CanBe { get; } = new List<int>();

    /// <summary>
    /// List of values which are not allowed in this cell.
    /// </summary>
    public List<int> CannotBe { get; } = new List<int>();

    public bool HasValue => _value > 0;

    public Cell(int value) => Value = value;

    public override string ToString() => Formatter.CellToString(this);
  }
}
