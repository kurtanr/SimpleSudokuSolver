using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class LockedCandidatesPointingTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new LockedCandidatesPointing();

    [Test]
    public void LockedCandidatesPointingInColumnTest()
    {
      var sudoku = new[,]
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
    public void LockedCandidatesPointingInRowTest()
    {
      // From: https://sudoku9x9.com/locked_candidates.html, transposed
      var sudoku = new[,]
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

    [Test]
    public void LockedCandidatesPointingTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Intersection_Removal
        { 0,1,7,9,0,3,6,0,0 },
        { 0,0,0,0,8,0,0,0,0 },
        { 9,0,0,0,0,0,5,0,7 },
        { 0,7,2,0,1,0,4,3,0 },
        { 0,0,0,4,0,2,0,7,0 },
        { 0,6,4,3,7,0,2,5,0 },
        { 7,0,1,0,0,0,0,6,5 },
        { 0,0,0,0,3,0,0,0,0 },
        { 0,0,5,6,0,1,7,2,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);

      // must first use NakedQuad to eliminate some candidates
      SolveUsingStrategy(sudokuPuzzle, new NakedQuad());
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 1].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 3);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 1].CanBe, 2);
    }

    [Test]
    public void LockedCandidatesPointingTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Intersection_Removal
        { 0,3,2,0,0,6,1,0,0 },
        { 4,1,0,0,0,0,0,0,0 },
        { 0,0,0,9,0,1,0,0,0 },
        { 5,0,0,0,9,0,0,0,4 },
        { 0,6,0,0,0,0,0,7,1 },
        { 3,0,0,0,2,0,0,0,5 },
        { 0,0,0,5,0,8,0,0,0 },
        { 0,0,0,0,0,0,5,1,9 },
        { 0,5,7,0,0,9,8,6,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);

      // must first use HiddenPair to eliminate some candidates
      SolveUsingStrategy(sudokuPuzzle, new HiddenPair());
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      // In a row
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 6].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 7].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 8].CanBe, 2);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 1].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 2].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 4].CanBe, 4);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 4].CanBe, 7);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 3].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 4].CanBe, 4);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 5].CanBe, 4);

      // In a column
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 1].CanBe, 7);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[6, 2].CanBe, 1);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 5].CanBe, 7);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 6].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 6].CanBe, 6);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 7].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 7].CanBe, 8);
    }

    [Test]
    public void LockedCandidatesPointingTest3()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Intersection_Removal
        { 9,3,0,0,5,0,0,0,0 },
        { 2,0,0,6,3,0,0,9,5 },
        { 8,5,6,0,0,2,0,0,0 },
        { 0,0,3,1,8,0,5,7,0 },
        { 0,0,5,0,2,0,9,8,0 },
        { 0,8,0,0,0,5,0,0,0 },
        { 0,0,0,8,0,0,1,5,9 },
        { 5,0,8,2,1,0,0,0,4 },
        { 0,0,0,5,6,0,0,0,8 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 5].CanBe, 3);
    }
  }
}
