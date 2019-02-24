using NUnit.Framework;
using SimpleSudokuSolver.Model;
using System;
using System.Linq;

namespace SimpleSudokuSolver.Test
{
  [TestFixture]
  public class ModelTests
  {
    private readonly int[,] _sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,1,2,3 },
        { 0,0,0,0,0,0,4,5,6 },
        { 0,0,0,0,0,0,7,8,9 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
      };

    [Test]
    public void EmptyCellTest()
    {
      var emptyCell = new Cell(0);
      Assert.That(emptyCell.Value, Is.EqualTo(0));
      Assert.That(emptyCell.HasValue, Is.False);
      Assert.That(emptyCell.CanBe, Is.Empty);
      Assert.That(emptyCell.CannotBe, Is.Empty);
    }

    [Test]
    public void CellTest()
    {
      var cell = new Cell(9);
      Assert.That(cell.Value, Is.EqualTo(9));
      Assert.That(cell.HasValue, Is.True);
      Assert.That(cell.CanBe, Is.Empty);
      Assert.That(cell.CannotBe, Is.Empty);
    }

    [Test]
    public void InvalidCellTest()
    {
      Assert.That(() => new Cell(-1), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Cell(10), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ColumnTests()
    {
      var column = new Column(1, 9);

      Assert.That(column, Is.Not.Null);
      Assert.That(column.ColumnIndex, Is.EqualTo(1));
      Assert.That(column.Cells.Length, Is.EqualTo(9));
      Assert.That(column.Cells.All(x => x == null));
    }

    [Test]
    public void InvalidColumnTests()
    {
      Assert.That(() => new Column(-1, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Column(9, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Column(1, 8), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void RowTests()
    {
      var row = new Row(1, 9);

      Assert.That(row, Is.Not.Null);
      Assert.That(row.RowIndex, Is.EqualTo(1));
      Assert.That(row.Cells.Length, Is.EqualTo(9));
      Assert.That(row.Cells.All(x => x == null));
    }

    [Test]
    public void InvalidRowTests()
    {
      Assert.That(() => new Row(-1, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Row(9, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Row(1, 8), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void BlockTests()
    {
      var block = new Block(1, 2, 3);

      Assert.That(block, Is.Not.Null);
      Assert.That(block.BlockRowIndex, Is.EqualTo(1));
      Assert.That(block.BlockColumnIndex, Is.EqualTo(2));
      Assert.That(block.Cells.GetLength(0), Is.EqualTo(3));
      Assert.That(block.Cells.GetLength(1), Is.EqualTo(3));

      var allCells = block.Cells.Cast<Cell>().ToArray();
      Assert.That(allCells.All(x => x == null));
      Assert.That(allCells.Length, Is.EqualTo(9));
    }

    [Test]
    public void InvalidBlockTests()
    {
      Assert.That(() => new Block(-1, 2, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(3, 2, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, -1, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, 3, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, 2, 9), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void SudokuPuzzleModelTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);

      // Block is correctly set
      var block = sudokuPuzzle.Blocks[1, 2];
      Assert.That(block.Cells[0, 0].Value, Is.EqualTo(1));
      Assert.That(block.Cells[0, 1].Value, Is.EqualTo(2));
      Assert.That(block.Cells[0, 2].Value, Is.EqualTo(3));
      Assert.That(block.Cells[1, 0].Value, Is.EqualTo(4));
      Assert.That(block.Cells[1, 1].Value, Is.EqualTo(5));
      Assert.That(block.Cells[1, 2].Value, Is.EqualTo(6));
      Assert.That(block.Cells[2, 0].Value, Is.EqualTo(7));
      Assert.That(block.Cells[2, 1].Value, Is.EqualTo(8));
      Assert.That(block.Cells[2, 2].Value, Is.EqualTo(9));

      // Rows are correctly set
      var row1 = sudokuPuzzle.Rows[3];
      Assert.That(row1.Cells[6].Value, Is.EqualTo(1));
      Assert.That(row1.Cells[7].Value, Is.EqualTo(2));
      Assert.That(row1.Cells[8].Value, Is.EqualTo(3));

      var row2 = sudokuPuzzle.Rows[4];
      Assert.That(row2.Cells[6].Value, Is.EqualTo(4));
      Assert.That(row2.Cells[7].Value, Is.EqualTo(5));
      Assert.That(row2.Cells[8].Value, Is.EqualTo(6));

      var row3 = sudokuPuzzle.Rows[5];
      Assert.That(row3.Cells[6].Value, Is.EqualTo(7));
      Assert.That(row3.Cells[7].Value, Is.EqualTo(8));
      Assert.That(row3.Cells[8].Value, Is.EqualTo(9));

      // Columns are correctly set
      var column1 = sudokuPuzzle.Columns[6];
      Assert.That(column1.Cells[3].Value, Is.EqualTo(1));
      Assert.That(column1.Cells[4].Value, Is.EqualTo(4));
      Assert.That(column1.Cells[5].Value, Is.EqualTo(7));

      var column2 = sudokuPuzzle.Columns[7];
      Assert.That(column2.Cells[3].Value, Is.EqualTo(2));
      Assert.That(column2.Cells[4].Value, Is.EqualTo(5));
      Assert.That(column2.Cells[5].Value, Is.EqualTo(8));

      var column3 = sudokuPuzzle.Columns[8];
      Assert.That(column3.Cells[3].Value, Is.EqualTo(3));
      Assert.That(column3.Cells[4].Value, Is.EqualTo(6));
      Assert.That(column3.Cells[5].Value, Is.EqualTo(9));

      // Cells are correctly set
      Assert.That(sudokuPuzzle.Cells[3, 6].Value, Is.EqualTo(1));
      Assert.That(sudokuPuzzle.Cells[3, 7].Value, Is.EqualTo(2));
      Assert.That(sudokuPuzzle.Cells[3, 8].Value, Is.EqualTo(3));
      Assert.That(sudokuPuzzle.Cells[4, 6].Value, Is.EqualTo(4));
      Assert.That(sudokuPuzzle.Cells[4, 7].Value, Is.EqualTo(5));
      Assert.That(sudokuPuzzle.Cells[4, 8].Value, Is.EqualTo(6));
      Assert.That(sudokuPuzzle.Cells[5, 6].Value, Is.EqualTo(7));
      Assert.That(sudokuPuzzle.Cells[5, 7].Value, Is.EqualTo(8));
      Assert.That(sudokuPuzzle.Cells[5, 8].Value, Is.EqualTo(9));
    }

    [Test]
    public void SudokuPuzzleModelModificationTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cell = sudokuPuzzle.Cells[4, 7];
      Assert.That(cell.Value, Is.EqualTo(5));

      // Modification through puzzle modifies cell
      sudokuPuzzle.Cells[4, 7].Value = 1;
      Assert.That(cell.Value, Is.EqualTo(1));

      // Modification through block modifies cell
      sudokuPuzzle.Blocks[1,2].Cells[1, 1].Value = 2;
      Assert.That(cell.Value, Is.EqualTo(2));

      // Modification through row modifies cell
      sudokuPuzzle.Rows[4].Cells[7].Value = 3;
      Assert.That(cell.Value, Is.EqualTo(3));

      // Modification through column modifies cell
      sudokuPuzzle.Columns[7].Cells[4].Value = 4;
      Assert.That(cell.Value, Is.EqualTo(4));
    }

    [Test]
    public void SudokuPuzzleToIntArrayTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var result = sudokuPuzzle.ToIntArray();
      CollectionAssert.AreEqual(result, _sudoku);
    }

    [Test]
    public void SudokuPuzzleGetCellIndexTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cell = sudokuPuzzle.Cells[4, 7];
      Assert.That(cell.Value, Is.EqualTo(5));

      var cellIndex = sudokuPuzzle.GetCellIndex(cell);

      Assert.That(cellIndex.RowIndex, Is.EqualTo(4));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(7));
    }

    [Test]
    public void SudokuPuzzleGetCellIndexForNonExistingCellTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cellIndex = sudokuPuzzle.GetCellIndex(new Cell(5));

      Assert.That(cellIndex.RowIndex, Is.EqualTo(-1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(-1));
    }
  }
}
