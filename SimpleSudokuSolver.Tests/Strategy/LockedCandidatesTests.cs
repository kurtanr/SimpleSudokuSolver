using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class LockedCandidatesTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new LockedCandidates();

    [Test]
    public void LockedCandidatesInColumnTest()
    {
      var sudoku = new int[,]
      {
        // From: https://sudoku9x9.com/locked_candidates.html
        { 0,1,2,0,0,0,0,0,0 },
        { 0,0,0,0,0,3,0,0,0 },
        { 0,5,6,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 0].CanBe, 3);
    }

    [Test]
    public void LockedCandidatesInRowTest()
    {
      // From: https://sudoku9x9.com/locked_candidates.html, transposed
      var sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 1,0,5,0,0,0,0,0,0 },
        { 2,0,6,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,3,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 4].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 6].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 8].CanBe, 3);
    }
  }
}
