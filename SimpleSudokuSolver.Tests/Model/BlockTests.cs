using NUnit.Framework;
using SimpleSudokuSolver.Model;
using System;
using System.Linq;

namespace SimpleSudokuSolver.Tests.Model
{
  [TestFixture]
  public class BlockTests
  {
    [Test]
    public void ValidBlockTest()
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
    public void InvalidBlockTest()
    {
      Assert.That(() => new Block(-1, 2, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(3, 2, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, -1, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, 3, 3), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new Block(1, 2, 9), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToStringTest()
    {
      string newLine = Environment.NewLine;

      var block = new Block(0, 0, 3);
      var toString = block.ToString();
      Assert.That(toString, Is.EqualTo($"     {newLine}     {newLine}     {newLine}"));

      block.Cells[0, 0] = new Cell(1);
      block.Cells[1, 1] = new Cell(5);
      block.Cells[2, 2] = new Cell(0);
      toString = block.ToString();
      Assert.That(toString, Is.EqualTo($"1    {newLine}  5  {newLine}    0{newLine}"));
    }
  }
}
