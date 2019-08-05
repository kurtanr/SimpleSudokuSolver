using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleSudokuSolver.Model;

namespace SimpleSudokuSolver.Test
{
  [TestFixture]
  public class DefaultSolverTests
  {
    SudokuPuzzle _puzzleForSingleInRowColumnBlock;

    [SetUp]
    public void SetUp()
    {
      _puzzleForSingleInRowColumnBlock = new SudokuPuzzle(new int[,]
      {
        { 0,0,0,0,0,0,0,1,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,3,0 },
        { 0,0,0,0,0,0,0,4,0 },
        { 0,0,0,0,0,0,0,5,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 1,2,3,0,5,6,8,7,9 },
        { 4,5,9,0,0,0,0,8,0 },
        { 6,0,8,0,0,0,0,9,0 },
       });
    }

    [Test]
    public void SingleInRowTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[6, 3].Value, Is.EqualTo(4));
    }

    [Test]
    public void SingleInColumnTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[5, 7].Value, Is.EqualTo(6));
    }

    [Test]
    public void SingleInBlockTest()
    {
      var solver = new DefaultSolver();
      var puzzle = solver.Solve(_puzzleForSingleInRowColumnBlock.ToIntArray());

      Assert.That(puzzle.Cells[8, 1].Value, Is.EqualTo(7));
    }

    [Test]
    public void HiddenSingleTest()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,2,0,0,0 },
        { 1,0,3,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,2,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      ValidateResult(sudoku, 4, 1, 2, "Hidden Single");
    }

    [Test]
    public void HiddenSingleTest_RowEvaluationCorrectlyDone()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,0,3,0,0,8,6 },
        { 0,0,0,0,2,0,0,4,0 },
        { 0,0,0,0,7,8,5,2,0 },
        { 3,7,1,8,5,6,2,9,4 },
        { 9,0,0,1,4,2,3,7,5 },
        { 4,0,0,3,9,7,6,1,8 },
        { 2,0,0,7,0,3,8,5,9 },
        { 0,3,9,2,0,5,4,6,7 },
        { 7,0,0,9,0,4,1,3,2 },
       };

      ValidateResult(sudoku, 2, 1, 9, "Hidden Single");
    }

    [Test]
    public void NakedSingleTest()
    {
      var sudoku = new int[,]
      {
        { 0,0,0,1,0,2,0,0,0 },
        { 0,8,0,0,0,0,4,5,0 },
        { 0,0,0,0,0,9,0,0,0 },
        { 0,0,0,0,6,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,7,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      ValidateResult(sudoku, 1, 4, 3, "Naked Single");
    }

    [Test]
    public void LockedCandidatesInColumnTest()
    {
      var sudoku = new int[,]
      {
        { 0,1,2,0,0,0,0,0,0 },
        { 0,0,0,0,0,3,0,0,0 },
        { 0,5,6,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      var solver = new DefaultSolver();
      var sudokuPuzzle = solver.Solve(sudoku);

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
      var sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,1,2 },
        { 0,0,0,0,0,0,0,4,5 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,3,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
       };

      var solver = new DefaultSolver();
      var sudokuPuzzle = solver.Solve(sudoku);

      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 0].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 1].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 2].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 3].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 4].CanBe, 3);
      CollectionAssert.DoesNotContain(sudokuPuzzle.Cells[5, 5].CanBe, 3);
    }

    [Test]
    public void NakedPairInColumnTest()
    {
      var sudoku = new int[,]
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

      ValidateResult(sudoku, 4, 0, 8, "Naked Pair");
    }

    [Test]
    public void NakedPairInRowTest()
    {
      var sudoku = new int[,]
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

      ValidateResult(sudoku, 0, 4, 8, "Naked Pair");
    }

    [Test]
    public void NakedTripleInColumnTest()
    {
      // Example: http://www.sudokuwiki.org/naked_candidates
      var sudoku = new int[,]
      {
        { 1,0,0,0,0,0,0,0,0 },
        { 2,0,0,0,0,0,0,0,0 },
        { 3,0,0,0,0,0,1,2,3 },
        { 4,0,0,0,0,0,0,0,0 },
        { 5,0,0,0,0,0,0,0,0 },
        { 6,0,0,0,0,0,0,0,0 },
        { 0,0,1,0,0,0,0,0,0 },
        { 0,0,2,0,0,0,0,0,0 },
        { 0,0,3,0,0,0,0,4,5 }
       };

      ValidateResult(sudoku, 8, 1, 6, "Naked Triple");
    }

    [Test]
    public void NakedTripleInRowTest()
    {
      // Example: http://www.sudokuwiki.org/naked_candidates
      var sudoku = new int[,]
      {
        { 1,2,3,4,5,6,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,1,2,3 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,4 },
        { 0,0,0,0,0,0,0,0,5 }
       };

      ValidateResult(sudoku, 1, 8, 6, "Naked Triple");
    }

    [Test]
    public void NakedTripleInBlockTest()
    {
      // Example: http://www.sudokuwiki.org/naked_candidates
      var sudoku = new int[,]
      {
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

      ValidateResult(sudoku, 5, 7, 9, "Naked Triple");
    }

    private void ValidateResult(int[,] sudoku, int rowIndex, int columnIndex, int value, string strategyName)
    {
      var solver = new DefaultSolver();
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var steps = new List<SingleStepSolution>();

      SingleStepSolution singleStepSolution;
      do
      {
        singleStepSolution = solver.SolveSingleStep(sudokuPuzzle);
        sudokuPuzzle.ApplySingleStepSolution(singleStepSolution);
        steps.Add(singleStepSolution);
      } while (singleStepSolution?.Eliminations != null);

      Assert.NotNull(singleStepSolution, "Cannot find solution");

      var strategies = steps.Select(x => x.Strategy).ToArray();

      Assert.That(strategies.Contains(strategyName));
      Assert.That(singleStepSolution.Result.IndexOfRow, Is.EqualTo(rowIndex));
      Assert.That(singleStepSolution.Result.IndexOfColumn, Is.EqualTo(columnIndex));
      Assert.That(singleStepSolution.Result.Value, Is.EqualTo(value));
    }
  }
}