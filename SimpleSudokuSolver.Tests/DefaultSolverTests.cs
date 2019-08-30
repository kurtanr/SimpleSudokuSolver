using NUnit.Framework;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests
{
  [TestFixture]
  public class DefaultSolverTests
  {
    private readonly int[,] _sudoku = new int[,]
    {
      { 0,0,0,0,0,0,0,0,3 },
      { 3,0,0,0,0,0,0,0,9 },
      { 0,0,0,0,9,3,1,4,2 },
      { 0,0,0,8,6,9,3,2,4 },
      { 0,0,0,3,1,2,8,5,7 },
      { 2,3,8,4,7,5,9,6,1 },
      { 0,0,4,7,5,8,2,3,6 },
      { 5,6,7,2,3,1,4,9,8 },
      { 8,2,3,9,4,6,7,1,5 }
    };

    [Test]
    public void DefaultSolverWithDefaultStrategies_CanSolvePuzzle_Test()
    {
      var defaultSolver = new DefaultSolver();
      var sudokuPuzzle = defaultSolver.Solve(_sudoku);
      Assert.That(sudokuPuzzle.Cells[0, 0].Value, Is.EqualTo(4));
    }

    [Test]
    public void DefaultSolverWithOnlyEliminationStrategies_CannotSolvePuzzle_Test()
    {
      var defaultSolver = new DefaultSolver(new BasicElimination());
      var sudokuPuzzle = defaultSolver.Solve(_sudoku);
      Assert.That(sudokuPuzzle.Cells[0, 0].Value, Is.EqualTo(0));
      Assert.That(sudokuPuzzle.Cells[0, 0].CanBe.Count, Is.EqualTo(5));
    }

    [Test]
    public void SolveSingleStepForSolvedPuzzle_ReturnsNull_Test()
    {
      var defaultSolver = new DefaultSolver();
      var sudokuPuzzle = defaultSolver.Solve(_sudoku);
      Assert.That(sudokuPuzzle.IsSolved);
      Assert.That(defaultSolver.SolveSingleStep(sudokuPuzzle), Is.Null);
    }
  }
}
