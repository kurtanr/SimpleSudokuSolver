using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class NakedQuadTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new NakedQuad();

    [Test]
    public void NakedQuadTest1()
    {
      var sudoku = new int[,]
      {
        // From: http://www.sudokuwiki.org/Naked_Candidates
        // Contains one naked quad in block, and after that, one naked quad in row
        { 0,0,0,0,3,0,0,8,6 },
        { 0,0,0,0,2,0,0,4,0 },
        { 0,9,0,0,7,8,5,2,0 },
        { 3,7,1,8,5,6,2,9,4 },
        { 9,0,0,1,4,2,3,7,5 },
        { 4,0,0,3,9,7,6,1,8 },
        { 2,0,0,7,0,3,8,5,9 },
        { 0,3,9,2,0,5,4,6,7 },
        { 7,0,0,9,0,4,1,3,2 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 1].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 1].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 2].CanBe, 6);
    }

    [Test]
    public void NakedQuadTest2()
    {
      var sudoku = new int[,]
      {
        // From: http://www.manifestmaster.com/jetsudoku/nakedQuad.html
        // Naked quad in column - must first use LockedCandidates to eliminate some candidates
        { 0,0,0,0,9,0,0,0,0 },
        { 0,0,0,0,3,1,6,0,0 },
        { 0,0,0,0,4,8,0,9,0 },
        { 7,1,9,8,6,3,4,5,2 },
        { 6,0,0,0,7,0,0,3,0 },
        { 2,0,0,0,1,0,7,6,0 },
        { 1,0,0,0,2,0,0,8,6 },
        { 8,6,0,0,5,9,2,0,0 },
        { 0,0,0,1,8,6,0,4,5 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, new LockedCandidates());
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 2].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 2].CanBe, 5);
    }
  }
}
