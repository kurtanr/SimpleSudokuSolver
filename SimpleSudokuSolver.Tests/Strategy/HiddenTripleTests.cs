using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class HiddenTripleTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new HiddenTriple();

    [Test]
    public void HiddenTripleTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Hidden_Candidates
        { 0,0,0,0,0,1,0,3,0 },
        { 2,3,1,0,9,0,0,0,0 },
        { 0,6,5,0,0,3,1,0,0 },
        { 6,7,8,9,2,4,3,0,0 },
        { 1,0,3,0,5,0,0,0,6 },
        { 0,0,0,1,3,6,7,0,0 },
        { 0,0,9,3,6,0,5,7,0 },
        { 0,0,6,0,1,9,8,4,3 },
        { 3,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 8);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 6].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 6].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 8].CanBe, 5);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 8].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 8].CanBe, 9);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 8].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 8].CanBe, 9);
    }
  }
}
