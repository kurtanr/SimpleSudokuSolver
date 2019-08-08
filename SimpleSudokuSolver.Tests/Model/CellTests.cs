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
      var emptyCell = new Cell(0);
      Assert.That(emptyCell.Value, Is.EqualTo(0));
      Assert.That(emptyCell.HasValue, Is.False);
      Assert.That(emptyCell.CanBe, Is.Empty);
    }

    [Test]
    public void ValidCellTest()
    {
      var cell = new Cell(9);
      Assert.That(cell.Value, Is.EqualTo(9));
      Assert.That(cell.HasValue, Is.True);
      Assert.That(cell.CanBe, Is.Empty);
    }

    [Test]
    public void InvalidCellTest()
    {
      Assert.That(() => new Cell(-1), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Cell(10), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToStringTest()
    {
      var cell = new Cell(5);
      var toString = cell.ToString();
      Assert.That(toString, Is.EqualTo("5"));

      cell = new Cell(0);
      toString = cell.ToString();
      Assert.That(toString, Is.EqualTo("0"));
    }
  }
}
