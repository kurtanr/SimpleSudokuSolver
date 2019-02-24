using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  /// <summary>
  /// Default sudoku solver.
  /// </summary>
  public class DefaultSolver : ISudokuSolver
  {
    #region ISudokuSolver interface implementation

    /// <inheritdoc />
    public SudokuPuzzle Solve(int[,] sudoku)
    {
      return Solve(sudoku, out _);
    }

    /// <inheritdoc />
    public SudokuPuzzle Solve(int[,] sudoku, out SingleStepSolution[] steps)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      var stepsList = new List<SingleStepSolution>();

      while (!IsSolved(sudokuPuzzle))
      {
        var solution = SolveSingleStep(sudoku);
        if (solution == null)
        {
          // Sudoku cannot be solved :(
          break;
        }
        else
        {
          sudoku[solution.IndexOfRow, solution.IndexOfColumn] = solution.Value;
          stepsList.Add(solution);
          sudokuPuzzle = new SudokuPuzzle(sudoku);
        }
      }

      steps = stepsList.ToArray();
      return sudokuPuzzle;
    }

    /// <inheritdoc />
    public SingleStepSolution SolveSingleStep(int[,] sudoku)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      if (IsSolved(sudokuPuzzle))
        return null;

      var solvingTechniques = new Func<SudokuPuzzle, SingleStepSolution>[]
      {
        // Ordered from the simples to the most complex
        SingleInRow,
        SingleInColumn,
        SingleInBlock,
        HiddenSingle,
        NakedSingle
      };

      foreach (var technique in solvingTechniques)
      {
        var solution = technique(sudokuPuzzle);
        if (solution != null)
          return solution;
      }

      return null;
    }

    /// <inheritdoc />
    public bool IsSolved(int[,] sudoku)
    {
      var sudokuPuzzle = new SudokuPuzzle(sudoku);
      return IsSolved(sudokuPuzzle);
    }

    private bool IsSolved(SudokuPuzzle sudokuPuzzle)
    {
      return Validation.IsPuzzleSolved(sudokuPuzzle);
    }

    #endregion

    #region Sudoku Solving Techniques

    private SingleStepSolution SingleInRow(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();

        // If only single cell in the row does not have a value
        if (cellsWithNoValue.Length == 1)
        {
          var knownValues = row.Cells.Where(x => x.HasValue).Select(x => x.Value);
          var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();

          for (int i = 0; i < row.Cells.Length; i++)
          {
            if (row.Cells[i] == cellsWithNoValue[0])
              return new SingleStepSolution(row.RowIndex, i, value, $"Row {row.RowIndex + 1} Column {i + 1} Value {value} [Single in Row]");
          }
        }
      }

      return null;
    }

    private SingleStepSolution SingleInColumn(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();

        // If only single cell in the column does not have a value
        if (cellsWithNoValue.Length == 1)
        {
          var knownValues = column.Cells.Where(x => x.HasValue).Select(x => x.Value);
          var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();

          for (int i = 0; i < column.Cells.Length; i++)
          {
            if (column.Cells[i] == cellsWithNoValue[0])
              return new SingleStepSolution(i, column.ColumnIndex, value, $"Row {i + 1} Column {column.ColumnIndex + 1} Value {value} [Single in Column]");
          }
        }
      }

      return null;
    }

    private SingleStepSolution SingleInBlock(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blocksWithNoValue = block.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();

        // If only single cell in the block does not have a value
        if (blocksWithNoValue.Length == 1)
        {
          var knownValues = block.Cells.OfType<Cell>().Where(x => x.HasValue).Select(x => x.Value);
          var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();
          var cellWithNoValue = block.Cells.OfType<Cell>().Where(x => !x.HasValue).Single();

          var cellIndex = sudokuPuzzle.GetCellIndex(cellWithNoValue);
          return new SingleStepSolution(cellIndex.RowIndex, cellIndex.ColumnIndex, value,
            $"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {value} [Single in Block]");
        }
      }

      return null;
    }

    private SingleStepSolution HiddenSingle(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithValue = row.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          cell.CannotBe.AddRange(cellsWithValue.Select(x => x.Value));
        }
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithValue = column.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var forbiddenValues = cell.CannotBe.Union(cellsWithValue.Select(x => x.Value)).ToArray();
          cell.CannotBe.Clear();
          cell.CannotBe.AddRange(forbiddenValues);
        }
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var cellsWithValue = block.Cells.OfType<Cell>().Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = block.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var forbiddenValues = cell.CannotBe.Union(cellsWithValue.Select(x => x.Value)).OrderBy(x => x).ToArray();
          cell.CannotBe.Clear();
          cell.CannotBe.AddRange(forbiddenValues);
        }

        foreach (var cellWithNoValue in cellsWithNoValue)
        {
          // Possible values in cellWithNoValue
          var possibleValues = sudokuPuzzle.PossibleCellValues.Except(cellWithNoValue.CannotBe).ToArray();

          // If a possible value cannot be in any other empty cell in the block, it must be in this cellWithNoValue
          foreach (var possibleValue in possibleValues)
          {
            var allOtherCells = cellsWithNoValue.Except(new[] { cellWithNoValue }).ToArray();
            if (allOtherCells.All(x => x.CannotBe.Contains(possibleValue)))
            {
              var cellIndex = sudokuPuzzle.GetCellIndex(cellWithNoValue);
              return new SingleStepSolution(cellIndex.RowIndex, cellIndex.ColumnIndex, possibleValue,
                $"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {possibleValue} [HiddenSingle]");
            }
          }
        }
      }

      return null;
    }

    private SingleStepSolution NakedSingle(SudokuPuzzle sudokuPuzzle)
    {
      int[] possibleCellValuesBasedOnRow, possibleCellValuesBasedOnColumn, possibleCellValuesBasedOnBlock;

      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithValue = row.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnRow = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          cell.CanBe.AddRange(possibleCellValuesBasedOnRow);
        }
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithValue = column.Cells.Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnColumn = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var possibleValues = cell.CanBe.Intersect(possibleCellValuesBasedOnColumn).ToArray();
          cell.CanBe.Clear();
          cell.CanBe.AddRange(possibleValues);
        }
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var cellsWithValue = block.Cells.OfType<Cell>().Where(x => x.HasValue).ToArray();
        var cellsWithNoValue = block.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();
        possibleCellValuesBasedOnBlock = sudokuPuzzle.PossibleCellValues.Except(cellsWithValue.Select(x => x.Value)).ToArray();

        foreach (var cell in cellsWithNoValue)
        {
          var possibleValues = cell.CanBe.Intersect(possibleCellValuesBasedOnBlock).ToArray();
          cell.CanBe.Clear();
          cell.CanBe.AddRange(possibleValues);
        }
      }

      for (int i = 0; i < sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < sudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (sudokuPuzzle.Cells[i, j].CanBe.Count == 1)
            return new SingleStepSolution(i, j, sudokuPuzzle.Cells[i, j].CanBe[0],
              $"Row {i + 1} Column {j + 1} Value {sudokuPuzzle.Cells[i, j].CanBe[0]} [NakedSingle]");
        }
      }

      return null;
    }

    #endregion
  }
}
