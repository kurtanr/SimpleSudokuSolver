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
      var sudoku = new int[,]
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
      var sudoku = new int[,]
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

    [Test]
    public void HiddenSingleTest3()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,0,3,0,0,8,6 },
        { 0,0,0,0,2,0,0,4,0 },
        { 0,0,0,0,7,8,5,2,0 },
        { 3,7,1,8,5,6,2,9,4 },
        { 9,0,0,1,4,2,3,7,5 },
        { 4,0,0,3,9,7,6,1,8 },
        { 2,0,0,7,0,3,8,5,9 },
        { 0,3,9,2,0,5,4,6,7 },
        { 7,0,0,9,0,4,1,3,2 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      Assert.That(sudokuPuzzle.Cells[2, 1].Value, Is.EqualTo(9));
    }
  }
}
