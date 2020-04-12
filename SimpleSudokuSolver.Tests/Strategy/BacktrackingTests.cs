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

    [Test]
    public void BacktrackingTest2()
    {
      var sudoku = new[,]
      {
        // Unsolvable, taken from: https://www.sudokudragon.com/unsolvable.htm
        { 5,1,6,8,4,9,7,3,2 },
        { 3,0,7,6,0,5,0,0,0 },
        { 8,0,9,7,0,0,0,6,5 },
        { 1,3,5,0,6,0,9,0,7 },
        { 4,7,2,5,9,1,0,0,6 },
        { 9,6,8,3,7,0,0,5,0 },
        { 2,5,3,1,8,6,0,7,4 },
        { 6,8,4,2,0,7,5,0,0 },
        { 7,9,1,0,5,0,6,0,8 }
      };

      var sudokuPuzzle = solver.Solve(sudoku);
      Assert.That(sudokuPuzzle.IsSolved(), Is.False);
    }
  }
}
