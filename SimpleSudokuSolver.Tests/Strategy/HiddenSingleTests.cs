using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class HiddenSingleTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new HiddenSingle();

    [Test]
    public void HiddenSingleTest1()
    {
      var sudoku = new[,]
      {
        // From: http://manifestmaster.com/jetsudoku/hiddensingle.html
        { 0,6,0,8,0,0,0,4,0 },
        { 0,0,0,0,4,0,0,0,2 },
        { 2,0,4,6,0,0,0,0,9 },
        { 0,0,1,0,0,9,3,0,0 },
        { 0,9,6,0,0,0,4,5,0 },
        { 0,0,8,3,0,0,0,0,0 },
        { 1,0,7,0,0,3,2,0,5 },
        { 9,0,2,0,5,0,0,0,0 },
        { 0,3,5,0,0,1,0,7,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      Assert.That(sudokuPuzzle.Cells[4, 0].Value, Is.EqualTo(3));
    }

    [Test]
    public void HiddenSingleTest2()
    {
      var sudoku = new[,]
      {
        // From: https://sudoku9x9.com/hidden_single.html
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,2,0,0,0 },
        { 1,0,3,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      Assert.That(sudokuPuzzle.Cells[4, 1].Value, Is.EqualTo(2));
    }
  }
}
