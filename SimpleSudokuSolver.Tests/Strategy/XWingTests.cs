using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class XWingTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new XWing();

    [Test]
    public void XWingTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/X_Wing_Strategy
        { 1,0,0,0,0,0,5,6,9 },
        { 4,9,2,0,5,6,1,0,8 },
        { 0,5,6,1,0,9,2,4,0 },
        { 0,0,9,6,4,0,8,0,1 },
        { 0,6,4,0,1,0,0,0,0 },
        { 2,1,8,0,3,5,6,0,4 },
        { 0,4,0,5,0,0,0,1,6 },
        { 9,0,5,0,6,1,4,0,2 },
        { 6,2,1,0,0,0,0,0,5 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 3].CanBe, 7);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 7].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 7].CanBe, 7);
    }

    [Test]
    public void XWingTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/X_Wing_Strategy
        { 0,0,0,0,0,0,0,9,4 },
        { 7,6,0,9,1,0,0,5,0 },
        { 0,9,0,0,0,2,0,8,1 },
        { 0,7,0,0,5,0,0,1,0 },
        { 0,0,0,7,0,9,0,0,0 },
        { 0,8,0,0,3,1,0,6,7 },
        { 2,4,0,1,0,0,0,7,0 },
        { 0,1,0,0,9,0,0,4,5 },
        { 9,0,0,0,0,0,1,0,0 }
      };

      // part 1
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 1].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 8].CanBe, 2);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 3].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 8].CanBe, 2);

      // part 2
      sudokuPuzzle.Cells[0, 1].Value = 2;

      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 8].CanBe, 3);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 3].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 5].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 8].CanBe, 3);
    }
  }
}
