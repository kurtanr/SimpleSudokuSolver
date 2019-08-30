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
      var sudoku = new [,]
      {
        // From: http://www.sudokuwiki.org/Getting_Started
        { 2,0,0,0,7,0,0,3,8 },
        { 0,0,0,0,0,6,0,7,0 },
        { 3,0,0,0,4,0,6,0,0 },
        { 0,0,8,0,2,0,7,0,0 },
        { 1,0,0,0,0,0,0,0,6 },
        { 0,0,7,0,3,0,4,0,0 },
        { 0,0,4,0,8,0,0,0,9 },
        { 0,6,0,4,0,0,0,0,0 },
        { 9,1,0,0,6,0,0,0,2 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      Assert.That(sudokuPuzzle.IsSolved());
    }

    [Test]
    public void HiddenSingleTest2()
    {
      var sudoku = new [,]
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
