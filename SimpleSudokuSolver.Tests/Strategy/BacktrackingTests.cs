using NUnit.Framework;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class BacktrackingTests : BaseStrategyTest
  {
    private readonly ISudokuSolver solver = new DefaultSolver(new HiddenSingle(), new Backtracking());

    [Test]
    public void BacktrackingTest1()
    {
      var sudoku = new[,]
      {
        // From: https://en.wikipedia.org/wiki/Sudoku_solving_algorithms#Backtracking
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,3,0,8,5 },
        { 0,0,1,0,2,0,0,0,0 },
        { 0,0,0,5,0,7,0,0,0 },
        { 0,0,4,0,0,0,1,0,0 },
        { 0,9,0,0,0,0,0,0,0 },
        { 5,0,0,0,0,0,0,7,3 },
        { 0,0,2,0,1,0,0,0,0 },
        { 0,0,0,0,4,0,0,0,9 }
      };

      var sudokuPuzzle = solver.Solve(sudoku);
      Assert.That(sudokuPuzzle.IsSolved(), Is.True);
    }
  }
}
