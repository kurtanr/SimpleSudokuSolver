using NUnit.Framework;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.Strategy;
using System;
using System.Linq;

namespace SimpleSudokuSolver.Tests.Model
{
  [TestFixture]
  public class SudokuPuzzleTests
  {
    private readonly int[,] _sudoku = new int[,]
      {
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,1,2,3 },
        { 0,0,0,0,0,0,4,5,6 },
        { 0,0,0,0,0,0,7,8,9 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0 },
      };

    [Test]
    public void SudokuPuzzleModelTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);

      // Block is correctly set
      var block = sudokuPuzzle.Blocks[1, 2];
      Assert.That(block.Cells[0, 0].Value, Is.EqualTo(1));
      Assert.That(block.Cells[0, 1].Value, Is.EqualTo(2));
      Assert.That(block.Cells[0, 2].Value, Is.EqualTo(3));
      Assert.That(block.Cells[1, 0].Value, Is.EqualTo(4));
      Assert.That(block.Cells[1, 1].Value, Is.EqualTo(5));
      Assert.That(block.Cells[1, 2].Value, Is.EqualTo(6));
      Assert.That(block.Cells[2, 0].Value, Is.EqualTo(7));
      Assert.That(block.Cells[2, 1].Value, Is.EqualTo(8));
      Assert.That(block.Cells[2, 2].Value, Is.EqualTo(9));

      // Rows are correctly set
      var row1 = sudokuPuzzle.Rows[3];
      Assert.That(row1.Cells[6].Value, Is.EqualTo(1));
      Assert.That(row1.Cells[7].Value, Is.EqualTo(2));
      Assert.That(row1.Cells[8].Value, Is.EqualTo(3));

      var row2 = sudokuPuzzle.Rows[4];
      Assert.That(row2.Cells[6].Value, Is.EqualTo(4));
      Assert.That(row2.Cells[7].Value, Is.EqualTo(5));
      Assert.That(row2.Cells[8].Value, Is.EqualTo(6));

      var row3 = sudokuPuzzle.Rows[5];
      Assert.That(row3.Cells[6].Value, Is.EqualTo(7));
      Assert.That(row3.Cells[7].Value, Is.EqualTo(8));
      Assert.That(row3.Cells[8].Value, Is.EqualTo(9));

      // Columns are correctly set
      var column1 = sudokuPuzzle.Columns[6];
      Assert.That(column1.Cells[3].Value, Is.EqualTo(1));
      Assert.That(column1.Cells[4].Value, Is.EqualTo(4));
      Assert.That(column1.Cells[5].Value, Is.EqualTo(7));

      var column2 = sudokuPuzzle.Columns[7];
      Assert.That(column2.Cells[3].Value, Is.EqualTo(2));
      Assert.That(column2.Cells[4].Value, Is.EqualTo(5));
      Assert.That(column2.Cells[5].Value, Is.EqualTo(8));

      var column3 = sudokuPuzzle.Columns[8];
      Assert.That(column3.Cells[3].Value, Is.EqualTo(3));
      Assert.That(column3.Cells[4].Value, Is.EqualTo(6));
      Assert.That(column3.Cells[5].Value, Is.EqualTo(9));

      // Cells are correctly set
      Assert.That(sudokuPuzzle.Cells[3, 6].Value, Is.EqualTo(1));
      Assert.That(sudokuPuzzle.Cells[3, 7].Value, Is.EqualTo(2));
      Assert.That(sudokuPuzzle.Cells[3, 8].Value, Is.EqualTo(3));
      Assert.That(sudokuPuzzle.Cells[4, 6].Value, Is.EqualTo(4));
      Assert.That(sudokuPuzzle.Cells[4, 7].Value, Is.EqualTo(5));
      Assert.That(sudokuPuzzle.Cells[4, 8].Value, Is.EqualTo(6));
      Assert.That(sudokuPuzzle.Cells[5, 6].Value, Is.EqualTo(7));
      Assert.That(sudokuPuzzle.Cells[5, 7].Value, Is.EqualTo(8));
      Assert.That(sudokuPuzzle.Cells[5, 8].Value, Is.EqualTo(9));
    }

    [Test]
    public void InvalidSudokuPuzzleTest()
    {
      Assert.That(() => new SudokuPuzzle(new int[8, 9]), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new SudokuPuzzle(new int[9, 8]), Throws.InstanceOf<ArgumentException>());
      Assert.That(() => new SudokuPuzzle(new int[9, 9]), Throws.Nothing);
    }

    [Test]
    public void SudokuPuzzleModelModificationTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cell = sudokuPuzzle.Cells[4, 7];
      Assert.That(cell.Value, Is.EqualTo(5));

      // Modification through puzzle modifies cell
      sudokuPuzzle.Cells[4, 7].Value = 1;
      Assert.That(cell.Value, Is.EqualTo(1));

      // Modification through block modifies cell
      sudokuPuzzle.Blocks[1, 2].Cells[1, 1].Value = 2;
      Assert.That(cell.Value, Is.EqualTo(2));

      // Modification through row modifies cell
      sudokuPuzzle.Rows[4].Cells[7].Value = 3;
      Assert.That(cell.Value, Is.EqualTo(3));

      // Modification through column modifies cell
      sudokuPuzzle.Columns[7].Cells[4].Value = 4;
      Assert.That(cell.Value, Is.EqualTo(4));
    }

    [Test]
    public void ToIntArrayTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var result = sudokuPuzzle.ToIntArray();
      CollectionAssert.AreEqual(result, _sudoku);
    }

    [Test]
    public void GetCellIndexTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cell = sudokuPuzzle.Cells[4, 7];
      Assert.That(cell.Value, Is.EqualTo(5));

      var cellIndex = sudokuPuzzle.GetCellIndex(cell);

      Assert.That(cellIndex.RowIndex, Is.EqualTo(4));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(7));
    }

    [Test]
    public void GetCellIndexForNullTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cellIndex = sudokuPuzzle.GetCellIndex(null);

      Assert.That(cellIndex.RowIndex, Is.EqualTo(-1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(-1));
    }

    [Test]
    public void GetCellIndexForNonExistingCellTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cellIndex = sudokuPuzzle.GetCellIndex(new Cell(5));

      Assert.That(cellIndex.RowIndex, Is.EqualTo(-1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(-1));
    }

    [Test]
    public void GetBlockIndexTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cell = sudokuPuzzle.Cells[4, 7];
      Assert.That(cell.Value, Is.EqualTo(5));

      var cellIndex = sudokuPuzzle.GetBlockIndex(cell);

      Assert.That(cellIndex.RowIndex, Is.EqualTo(1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(2));
    }

    [Test]
    public void GetBlockIndexForNullTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cellIndex = sudokuPuzzle.GetBlockIndex(null);

      Assert.That(cellIndex.RowIndex, Is.EqualTo(-1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(-1));
    }

    [Test]
    public void GetBlockIndexForNonExistingCellTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      var cellIndex = sudokuPuzzle.GetBlockIndex(new Cell(5));

      Assert.That(cellIndex.RowIndex, Is.EqualTo(-1));
      Assert.That(cellIndex.ColumnIndex, Is.EqualTo(-1));
    }

    [Test]
    public void SudokuPuzzleInitialStepsTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      Assert.That(sudokuPuzzle.Steps, Is.Empty);
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.Zero);
    }

    [Test]
    public void ApplyInvalidSingleStepSolutionTest()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      Assert.That(() => sudokuPuzzle.ApplySingleStepSolution(null), Throws.Nothing);
      Assert.That(sudokuPuzzle.Steps, Is.Empty);
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.Zero);

      var solutionWithNoEliminations = new SingleStepSolution(new SingleStepSolution.Candidate[0], "test");
      Assert.That(() => sudokuPuzzle.ApplySingleStepSolution(solutionWithNoEliminations), Throws.Nothing);
      Assert.That(sudokuPuzzle.Steps, Is.Empty);
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.Zero);
    }

    [Test]
    public void ApplyValidSingleStepSolutionTest()
    {
      var sudoku = (int[,])_sudoku.Clone();
      sudoku[4, 7] = 0;

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var strategyWhichFindsNothing = new NakedTriple();
      var strategyWhichReducesCandidates = new BasicElimination();
      var strategyWhichFindsResult = new HiddenSingle();

      sudokuPuzzle.ApplySingleStepSolution(strategyWhichFindsNothing.SolveSingleStep(sudokuPuzzle));
      Assert.That(sudokuPuzzle.Steps, Is.Empty);
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.Zero);

      sudokuPuzzle.ApplySingleStepSolution(strategyWhichReducesCandidates.SolveSingleStep(sudokuPuzzle));
      Assert.That(sudokuPuzzle.Steps.Count, Is.EqualTo(1));
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.EqualTo(1));
      Assert.That(sudokuPuzzle.Steps.Last().Strategy, Is.EqualTo(strategyWhichReducesCandidates.StrategyName));
      Assert.That(sudokuPuzzle.Steps.Last().SolutionDescription, Is.Not.Empty);

      sudokuPuzzle.ApplySingleStepSolution(strategyWhichFindsResult.SolveSingleStep(sudokuPuzzle));
      Assert.That(sudokuPuzzle.Steps.Count, Is.EqualTo(2));
      Assert.That(sudokuPuzzle.NumberOfSteps, Is.EqualTo(2));
      Assert.That(sudokuPuzzle.Steps.Last().Strategy, Is.EqualTo(strategyWhichFindsResult.StrategyName));
      Assert.That(sudokuPuzzle.Cells[4, 7].Value, Is.EqualTo(5));
      Assert.That(sudokuPuzzle.Steps.Last().SolutionDescription, Is.Not.Empty);
    }

    [Test]
    public void UndoLastSingleStepSolution_WhenNoSolutionWasApplied_ReturnsNull_Test()
    {
      var sudokuPuzzle = new SudokuPuzzle(_sudoku);
      Assert.That(sudokuPuzzle.UndoLastSingleStepSolution(), Is.Null);
    }

    [Test]
    public void UndoSingleStepSolutionsWhichReturnsResult_UndoesResult_Test()
    {
      var sudoku = (int[,])_sudoku.Clone();
      sudoku[4, 7] = 0;

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var strategyWhichFindsResult = new HiddenSingle();

      sudokuPuzzle.ApplySingleStepSolution(strategyWhichFindsResult.SolveSingleStep(sudokuPuzzle));
      Assert.That(sudokuPuzzle.Cells[4, 7].Value, Is.EqualTo(5));

      var step = sudokuPuzzle.UndoLastSingleStepSolution();
      Assert.That(step.Eliminations, Is.Null);
      Assert.That(step.Result, Is.Not.Null);
      Assert.That(step.Result.IndexOfRow, Is.EqualTo(4));
      Assert.That(step.Result.IndexOfColumn, Is.EqualTo(7));
      Assert.That(step.Strategy, Is.EqualTo(strategyWhichFindsResult.StrategyName));
      Assert.That(step.SolutionDescription, Is.Not.Empty);
      Assert.That(sudokuPuzzle.Cells[4, 7].Value, Is.EqualTo(0));
    }

    [Test]
    public void UndoSingleStepSolutionsWhichReturnsEliminations_UndoesEliminations_Test()
    {
      var sudoku = (int[,])_sudoku.Clone();
      sudoku[4, 7] = 0;

      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var strategyWhichReducesCandidates = new BasicElimination();

      sudokuPuzzle.ApplySingleStepSolution(strategyWhichReducesCandidates.SolveSingleStep(sudokuPuzzle));
      Assert.That(sudokuPuzzle.Cells[4, 7].CanBe.Single(), Is.EqualTo(5));

      var step = sudokuPuzzle.UndoLastSingleStepSolution();
      Assert.That(step.Eliminations, Is.Not.Empty);
      Assert.That(step.Result, Is.Null);
      Assert.That(step.Strategy, Is.EqualTo(strategyWhichReducesCandidates.StrategyName));
      Assert.That(step.SolutionDescription, Is.Not.Empty);
      CollectionAssert.AreEqual(sudokuPuzzle.Cells[4, 7].CanBe, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }

    [Test]
    public void ToStringTest()
    {
      string newLine = Environment.NewLine;

      var sudokuPuzzle = new SudokuPuzzle(new int[9, 9]);
      var toString = sudokuPuzzle.ToString();
      var emptyPuzzleToString = string.Concat(Enumerable.Repeat($"0 0 0 0 0 0 0 0 0{newLine}", 9));
      Assert.That(toString, Is.EqualTo(emptyPuzzleToString));

      sudokuPuzzle.Cells[1, 2].Value = 5;
      toString = sudokuPuzzle.ToString();
      var expectedStringRepresentation = emptyPuzzleToString.Remove(23, 1).Insert(23, "5");
      Assert.That(toString, Is.EqualTo(expectedStringRepresentation));
    }
  }
}
