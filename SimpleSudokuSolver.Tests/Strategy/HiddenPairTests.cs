using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class HiddenPairTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new HiddenPair();

    [Test]
    public void HiddenPairTest1()
    {
      var sudoku = new [,]
      {
        // From: http://www.sudokuwiki.org/Hidden_Candidates
        { 0,0,0,0,0,0,0,0,0 },
        { 9,0,4,6,0,7,0,0,0 },
        { 0,7,6,8,0,4,1,0,0 },
        { 3,0,9,7,0,1,0,8,0 },
        { 0,0,8,0,0,0,3,0,0 },
        { 0,5,0,3,0,8,7,0,2 },
        { 0,0,7,5,0,2,6,1,0 },
        { 0,0,0,4,0,3,2,0,8 },
        { 0,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 9);
    }

    [Test]
    public void HiddenPairTest2()
    {
      var sudoku = new [,]
      {
        // From: http://www.sudokuwiki.org/Hidden_Candidates
        { 7,2,0,4,0,8,0,3,0 },
        { 0,8,0,0,0,0,0,4,7 },
        { 4,0,1,0,7,6,8,0,2 },
        { 8,1,0,7,3,9,0,0,0 },
        { 0,0,0,8,5,1,0,0,0 },
        { 0,0,0,2,6,4,0,8,0 },
        { 2,0,9,6,8,0,4,1,3 },
        { 3,4,0,0,0,0,0,0,8 },
        { 1,6,8,9,4,3,2,7,5 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 2].CanBe, 6);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 1].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 7);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 6].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 6].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 6].CanBe, 9);
    }
  }
}
