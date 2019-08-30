using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class SingleInCellsTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new SingleInCells();

    [Test]
    public void SingleInCellsTest1()
    {
      var sudoku = new [,]
      {
        { 0,0,0,0,0,0,0,1,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,3,0 },
        { 0,0,0,0,0,0,0,4,0 },
        { 0,0,0,0,0,0,0,5,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 1,2,3,0,5,6,8,7,9 },
        { 4,5,9,0,0,0,0,8,0 },
        { 6,0,8,0,0,0,0,9,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      // SingleInRow
      Assert.That(sudokuPuzzle.Cells[6, 3].Value, Is.EqualTo(4));

      // SingleInColumn
      Assert.That(sudokuPuzzle.Cells[5, 7].Value, Is.EqualTo(6));

      // SingleInBlock
      Assert.That(sudokuPuzzle.Cells[8, 1].Value, Is.EqualTo(7));
    }
  }
}
