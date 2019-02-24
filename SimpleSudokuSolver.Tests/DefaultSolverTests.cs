using NUnit.Framework;
using SimpleSudokuSolver.Model;

namespace SimpleSudokuSolver.Test
{
  [TestFixture]
  public class DefaultSolverTests
  {
    SudokuPuzzle _puzzleForSingleInRowColumnBlock;

    [SetUp]
    public void SetUp()
    {
      _puzzleForSingleInRowColumnBlock = new SudokuPuzzle(new int[,]
      {
        { 0,0,0,0,0,0,0,1,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,3,0 },
        { 0,0,0,0,0,0,0,4,0 },
        { 0,0,0,0,0,0,0,5,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 1,2,3,0,5,6,8,7,9 },
        { 4,5,9,0,0,0,0,8,0 },
        { 6,0,8,0,0,0,0,9,0 },
       });
    }

    [Test]
    public void SingleInRowTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[6, 3].Value, Is.EqualTo(4));
    }

    [Test]
    public void SingleInColumnTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[5, 7].Value, Is.EqualTo(6));
    }

    [Test]
    public void SingleInBlockTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[8, 1].Value, Is.EqualTo(7));
    }

    [Test]
    public void HiddenSingleTest()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,2,0,0,0 },
        { 1,0,3,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      var solver = new DefaultSolver();
      var singleStepSolution = solver.SolveSingleStep(sudoku);

      Assert.That(singleStepSolution.IndexOfRow, Is.EqualTo(4));
      Assert.That(singleStepSolution.IndexOfColumn, Is.EqualTo(1));
      Assert.That(singleStepSolution.Value, Is.EqualTo(2));
    }

    [Test]
    public void NakedSingleTest()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,1,0,2,0,0,0 },
        { 0,8,0,0,0,0,4,5,0 },
        { 0,0,0,0,0,9,0,0,0 },
        { 0,0,0,0,6,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,7,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      var solver = new DefaultSolver();
      var singleStepSolution = solver.SolveSingleStep(sudoku);

      Assert.That(singleStepSolution.IndexOfRow, Is.EqualTo(1));
      Assert.That(singleStepSolution.IndexOfColumn, Is.EqualTo(4));
      Assert.That(singleStepSolution.Value, Is.EqualTo(3));
    }
  }
}
