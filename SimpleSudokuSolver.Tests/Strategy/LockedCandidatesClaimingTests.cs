using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class LockedCandidatesClaimingTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new LockedCandidatesClaiming();

    [Test]
    public void LockedCandidatesClaimingTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Intersection_Removal
        { 0,1,6,0,0,7,8,0,3 },
        { 0,9,0,8,0,0,0,0,0 },
        { 8,7,0,0,0,1,2,6,0 },
        { 0,4,8,0,0,0,3,0,0 },
        { 6,5,0,0,0,9,0,8,2 },
        { 0,3,9,0,0,0,6,5,0 },
        { 0,6,0,9,0,0,0,2,0 },
        { 0,8,0,0,0,2,9,3,6 },
        { 9,2,4,6,0,0,5,1,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 4].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 3].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 4].CanBe, 2);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 6].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 8].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 6].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 8].CanBe, 4);
    }

    [Test]
    public void LockedCandidatesClaimingTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Intersection_Removal
        { 0,2,0,9,4,3,7,1,5 },
        { 9,0,4,0,0,0,6,0,0 },
        { 7,5,0,0,0,0,0,4,0 },
        { 5,0,0,4,8,0,0,0,0 },
        { 2,0,0,0,0,0,4,5,3 },
        { 4,0,0,3,5,2,0,0,0 },
        { 0,4,2,0,0,0,0,8,1 },
        { 0,0,5,0,0,4,2,6,0 },
        { 0,9,0,2,0,8,5,0,4 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 2].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 2].CanBe, 6);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 1].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 2].CanBe, 3);
    }
  }
}
