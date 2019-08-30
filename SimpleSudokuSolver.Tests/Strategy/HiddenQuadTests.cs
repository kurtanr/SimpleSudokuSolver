using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class HiddenQuadTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new HiddenQuad();

    [Test]
    public void HiddenQuadTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Hidden_Candidates
        { 6,5,0,0,8,7,0,2,4 },
        { 0,0,0,6,4,9,0,5,0 },
        { 0,4,0,0,2,5,0,0,0 },
        { 5,7,0,4,3,8,0,6,1 },
        { 0,0,0,5,0,1,0,0,0 },
        { 3,1,0,9,0,2,0,8,5 },
        { 0,0,0,8,9,0,0,1,0 },
        { 0,0,0,2,1,3,0,0,0 },
        { 1,3,0,7,5,0,0,9,8 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      // manual adaptation of CanBe so HiddenQuad can be applied
      sudokuPuzzle.Cells[2, 6].CanBe.Remove(9);
      sudokuPuzzle.Cells[4, 6].CanBe.Remove(2);
      sudokuPuzzle.Cells[4, 6].CanBe.Remove(9);
      sudokuPuzzle.Cells[6, 6].CanBe.Remove(2);
      sudokuPuzzle.Cells[6, 6].CanBe.Remove(4);
      sudokuPuzzle.Cells[8, 6].CanBe.Remove(4);

      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 6].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 6].CanBe, 6);
    }

    [Test]
    public void HiddenQuadTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Hidden_Candidates
        { 9,0,1,5,0,0,0,4,6 },
        { 4,2,5,0,9,0,0,8,1 },
        { 8,6,0,0,1,0,0,2,0 },
        { 5,0,2,0,0,0,0,0,0 },
        { 0,1,9,0,0,0,4,6,0 },
        { 6,0,0,0,0,0,0,0,2 },
        { 1,9,6,0,4,0,2,5,3 },
        { 2,0,0,0,6,0,8,1,7 },
        { 0,0,0,0,0,1,6,9,4 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 3].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 3].CanBe, 8);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 5].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 5].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 5].CanBe, 8);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 3].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 3].CanBe, 8);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 5].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 5].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 5].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 5].CanBe, 8);
    }
  }
}
