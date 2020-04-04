using NUnit.Framework;
using SimpleSudokuSolver.Model;
using System;

namespace SimpleSudokuSolver.Tests.Model
{
  [TestFixture]
  public class CellTests
  {
    [Test]
    public void EmptyCellTest()
    {
      var emptyCell = new Cell(0, 0, 0);
      Assert.That(emptyCell.Value, Is.EqualTo(0));
      Assert.That(emptyCell.HasValue, Is.False);
      Assert.That(emptyCell.CanBe, Is.Empty);
    }

    [Test]
    public void ValidCellTest()
    {
      var cell = new Cell(9, 1, 2);
      Assert.That(cell.Value, Is.EqualTo(9));
      Assert.That(cell.HasValue, Is.True);
      Assert.That(cell.CanBe, Is.Empty);
      Assert.That(cell.RowIndex, Is.EqualTo(1));
      Assert.That(cell.ColumnIndex, Is.EqualTo(2));
    }

    [Test]
    public void InvalidCellTest()
    {
      Assert.That(() => new Cell(-1, 0, 0), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Cell(10, 0, 0), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToStringTest()
    {
      var cell = new Cell(5, 1, 2);
      var toString = cell.ToString();
      Assert.That(toString, Is.EqualTo("[1,2]=5"));

      cell = new Cell(0, 3, 4);
      toString = cell.ToString();
      Assert.That(toString, Is.EqualTo("[3,4]=0"));
    }
  }
}
