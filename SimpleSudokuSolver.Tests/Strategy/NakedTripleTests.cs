using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;

namespace SimpleSudokuSolver.Tests.Strategy
{
  public class NakedTripleTests : BaseStrategyTest
  {
    private readonly ISudokuSolverStrategy _strategy = new NakedTriple();

    [Test]
    public void NakedTripleTest1()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Naked_Candidates
        { 0,7,0,4,0,8,0,2,9 },
        { 0,0,2,0,0,0,0,0,4 },
        { 8,5,4,0,2,0,0,0,7 },
        { 0,0,8,3,7,4,2,0,0 },
        { 0,2,0,0,0,0,0,0,0 },
        { 0,0,3,2,6,1,7,0,0 },
        { 0,0,0,0,9,3,6,1,2 },
        { 2,0,0,0,0,0,4,0,3 },
        { 1,3,0,6,4,2,0,7,0 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 0].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 0].CanBe, 9);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 9);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 6].CanBe, 9);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 7].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 7].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 7].CanBe, 9);

    }

    [Test]
    public void NakedTripleTest2()
    {
      var sudoku = new[,]
      {
        // From: http://www.sudokuwiki.org/Naked_Candidates
        { 2,9,4,5,1,3,0,0,6 },
        { 6,0,0,8,4,2,3,1,9 },
        { 3,0,0,6,9,7,2,5,4 },
        { 0,0,0,0,5,6,0,0,0 },
        { 0,4,0,0,8,0,0,6,0 },
        { 0,0,0,4,7,0,0,0,0 },
        { 7,3,0,1,6,4,0,0,5 },
        { 9,0,0,7,3,5,0,0,1 },
        { 4,0,0,9,2,8,6,3,7 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 1].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 1].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 2].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 6].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 7].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[3, 7].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[4, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 1].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 1].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 1].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 2].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 2].CanBe, 5);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 6].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 7].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 7].CanBe, 8);
    }

    [Test]
    public void NakedTripleTest3()
    {
      var sudoku = new[,]
      {
        // From: http://hodoku.sourceforge.net/en/tech_naked.php, Naked Triple
        // http://www.sudokuwiki.org/sudoku.htm?bd=390000700000000650507000349049380506601054983853000400900800134002940865400000297
        { 3,9,0,0,0,0,7,0,0 },
        { 0,0,0,0,0,0,6,5,0 },
        { 5,0,7,0,0,0,3,4,9 },
        { 0,4,9,3,8,0,5,0,6 },
        { 6,0,1,0,5,4,9,8,3 },
        { 8,5,3,0,0,0,4,0,0 },
        { 9,0,0,8,0,0,1,3,4 },
        { 0,0,2,9,4,0,8,6,5 },
        { 4,0,0,0,0,0,2,9,7 }
      };

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      SolveUsingStrategy(sudokuPuzzle, _strategy);

      // {3/3/2} triple in row 1
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 2].CanBe, 8);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 3].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 3].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 4].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 4].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[1, 5].CanBe, 8);

      // {3/3/3} triple in block[0,1]
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 3].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[0, 5].CanBe, 6);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 5].CanBe, 1);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 5].CanBe, 2);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[2, 5].CanBe, 6);
    }
  }
}
