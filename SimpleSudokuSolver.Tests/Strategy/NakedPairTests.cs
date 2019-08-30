using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class NakedPairTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new NakedPair();

    [Test]
    public void NakedPairInRowTest()
    {
      // From: https://sudoku9x9.com/naked_pair.html
      var sudoku = new[,]
      {
        { 9,0,0,4,0,0,1,0,0 },
        { 0,5,6,0,0,0,0,0,0 },
        { 0,7,8,0,0,6,2,0,0 },
        { 0,0,0,0,2,3,0,0,0 },
        { 0,0,0,0,5,0,0,7,3 },
        { 0,0,0,0,7,0,0,0,0 },
        { 0,0,0,0,0,0,0,8,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 4].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 7].CanBe, 3);
    }

    [Test]
    public void NakedPairInColumnTest()
    {
      // From: https://sudoku9x9.com/naked_pair.html, transposed
      var sudoku = new[,]
      {
        { 9,0,0,0,0,0,0,0,0 },
        { 0,5,7,0,0,0,0,0,0 },
        { 0,6,8,0,0,0,0,0,0 },
        { 4,0,0,0,0,0,0,0,0 },
        { 0,0,0,2,5,7,0,0,0 },
        { 0,0,6,3,0,0,0,0,0 },
        { 1,0,2,0,0,0,0,0,0 },
        { 0,0,0,0,7,0,8,0,0 },
        { 0,0,0,0,3,0,0,0,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 0].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 0].CanBe, 3);
    }

    [Test]
    public void NakedPairTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Naked_Candidates
        { 4,0,0,0,0,0,9,3,8 },
        { 0,3,2,0,9,4,1,0,0 },
        { 0,9,5,3,0,0,2,4,0 },
        { 3,7,0,6,0,9,0,0,4 },
        { 5,2,9,0,0,1,6,7,3 },
        { 6,0,4,7,0,3,0,9,0 },
        { 9,5,7,0,0,8,3,0,0 },
        { 0,0,3,9,0,0,4,0,0 },
        { 2,4,0,0,3,0,7,0,9 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 4].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 4].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 6);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 0].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 0].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 4].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 4].CanBe, 7);
    }

    [Test]
    public void NakedPairTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Naked_Candidates
        { 0,8,0,0,9,0,0,3,0 },
        { 0,3,0,0,0,0,0,6,9 },
        { 9,0,2,0,6,3,1,5,8 },
        { 0,2,0,8,0,4,5,9,0 },
        { 8,5,1,9,0,7,0,4,6 },
        { 3,9,4,6,0,5,8,7,0 },
        { 5,6,3,0,4,0,9,8,7 },
        { 2,0,0,0,0,0,0,1,5 },
        { 0,1,0,0,5,0,0,2,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 3].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 2].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 4].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[7, 4].CanBe, 7);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[8, 2].CanBe, 7);
    }
  }
}
