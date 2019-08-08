using NUnit.Framework;
using SimpleSudokuSolver.Model;
using System;
using System.Linq;

namespace SimpleSudokuSolver.Tests.Model
{
  [TestFixture]
  public class RowTests
  {
    [Test]
    public void ValidRowTest()
    {
      var row = new Row(1, 9);

      Assert.That(row, Is.Not.Null);
      Assert.That(row.RowIndex, Is.EqualTo(1));
      Assert.That(row.Cells.Length, Is.EqualTo(9));
      Assert.That(row.Cells.All(x => x == null));
    }

    [Test]
    public void InvalidRowTest()
    {
      Assert.That(() => new Row(-1, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Row(9, 9), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Row(1, 8), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToStringTest()
    {
      var row = new Row(0, 9);
      var toString = row.ToString();
      Assert.That(toString, Is.EqualTo(new string(' ', 17)));

      row.Cells[0] = new Cell(1);
      row.Cells[3] = new Cell(4);
      row.Cells[8] = new Cell(0);
      toString = row.ToString();
      Assert.That(toString, Is.EqualTo($"1     4         0"));
    }
  }
}
