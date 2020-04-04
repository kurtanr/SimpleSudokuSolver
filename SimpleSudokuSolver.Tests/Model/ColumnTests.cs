using NUnit.Framework;
using SimpleSudokuSolver.Model;
using System;
using System.Linq;

namespace SimpleSudokuSolver.Tests.Model
{
  [TestFixture]
  public class ColumnTests
  {
    [Test]
    public void ValidColumnTest()
    {
      var column = new Column(1, 9);

      Assert.That(column, Is.Not.Null);
      Assert.That(column.ColumnIndex, Is.EqualTo(1));
      Assert.That(column.Cells.Length, Is.EqualTo(9));
      Assert.That(column.Cells.All(x => x == null));
    }

    [Test]
    public void InvalidColumnTest()
    {
      Assert.That(() => new Column(-1, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Column(9, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Column(1, 8), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToStringTest()
    {
      string newLine = Environment.NewLine;

      var column = new Column(0, 9);
      var toString = column.ToString();
      Assert.That(toString, Is.EqualTo($" {newLine} {newLine} {newLine} {newLine} {newLine} {newLine} {newLine} {newLine} {newLine}"));

      column.Cells[0] = new Cell(1, 0, 0);
      column.Cells[3] = new Cell(4, 3, 0);
      column.Cells[8] = new Cell(0, 8, 0);
      toString = column.ToString();
      Assert.That(toString, Is.EqualTo($"1{newLine} {newLine} {newLine}4{newLine} {newLine} {newLine} {newLine} {newLine}0{newLine}"));
    }
  }
}
