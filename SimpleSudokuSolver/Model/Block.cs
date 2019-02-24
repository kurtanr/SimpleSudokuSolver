namespace SimpleSudokuSolver.Model
{
  /// <summary>
  /// For a 9x9 sudoku, block contains 9 <see cref="Cell"/> objects (organized in 3 rows and 3 columns).
  /// </summary>
  public class Block
  {
    /// <summary>
    /// 2-dimensional array of block cells.
    /// </summary>
    public Cell[,] Cells { get; }

    /// <summary>
    /// Zero-based index of the block row.
    /// </summary>
    public int BlockRowIndex { get; }

    /// <summary>
    /// Zero-based index of the block column.
    /// </summary>
    public int BlockColumnIndex { get; }

    public Block(int blockRowIndex, int blockColumnIndex, int numberOfRowsOrColumnsInBlock)
    {
      Validation.ValidateBlockRowOrBlockColumnIndex(blockRowIndex);
      Validation.ValidateBlockRowOrBlockColumnIndex(blockColumnIndex);
      Validation.ValidateNumberOfRowsOrColumnsInBlock(numberOfRowsOrColumnsInBlock);

      BlockRowIndex = blockRowIndex;
      BlockColumnIndex = blockColumnIndex;
      Cells = new Cell[numberOfRowsOrColumnsInBlock, numberOfRowsOrColumnsInBlock];
    }

    public override string ToString() => Formatter.BlockToString(this);
  }
}
