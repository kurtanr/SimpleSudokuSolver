using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Model
{
  public class SudokuPuzzle
  {
    public Cell[,] Cells { get; }
    public Row[] Rows { get; }
    public Column[] Columns { get; }
    public Block[,] Blocks { get; }
    public int[] PossibleCellValues { get; }
    public SingleStepSolution[] Steps => _steps.Select(x => x.Item1).ToArray();

    public int NumberOfRowsOrColumnsInPuzzle { get; }
    public int NumberOfRowsOrColumnsInBlock { get; }

    private readonly List<Tuple<SingleStepSolution, int[]>> _steps =
      new List<Tuple<SingleStepSolution, int[]>>();

    public SudokuPuzzle(int[,] sudoku)
    {
      Validation.ValidateSudoku(sudoku);

      NumberOfRowsOrColumnsInPuzzle = sudoku.GetLength(0);

      // We assume a square puzzle.
      NumberOfRowsOrColumnsInBlock = (int)System.Math.Sqrt(NumberOfRowsOrColumnsInPuzzle);

      Cells = new Cell[NumberOfRowsOrColumnsInPuzzle, NumberOfRowsOrColumnsInPuzzle];
      Rows = new Row[NumberOfRowsOrColumnsInPuzzle];
      Columns = new Column[NumberOfRowsOrColumnsInPuzzle];
      Blocks = new Block[NumberOfRowsOrColumnsInBlock, NumberOfRowsOrColumnsInBlock];
      PossibleCellValues = Enumerable.Range(1, NumberOfRowsOrColumnsInPuzzle).ToArray();

      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        Rows[i] = new Row(i, NumberOfRowsOrColumnsInPuzzle);

        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (i == 0)
          {
            Columns[j] = new Column(j, NumberOfRowsOrColumnsInPuzzle);
          }

          var cell = new Cell(sudoku[i, j]);
          if (!cell.HasValue)
          {
            cell.CanBe.AddRange(PossibleCellValues);
          }

          Cells[i, j] = cell;
          Columns[j].Cells[i] = Cells[i, j];
          Rows[i].Cells[j] = Cells[i, j];

          var blockRowIndex = i / NumberOfRowsOrColumnsInBlock;
          var blockColumnIndex = j / NumberOfRowsOrColumnsInBlock;

          if (Blocks[blockRowIndex, blockColumnIndex] == null)
          {
            Blocks[blockRowIndex, blockColumnIndex] = new Block(
              blockRowIndex, blockColumnIndex, NumberOfRowsOrColumnsInBlock);
          }

          Blocks[blockRowIndex, blockColumnIndex].Cells[i % NumberOfRowsOrColumnsInBlock, j % NumberOfRowsOrColumnsInBlock] = Cells[i, j];
        }
      }
    }

    /// <summary>
    /// Applies <paramref name="singleStepSolution"/> to the puzzle.
    /// </summary>
    /// <param name="singleStepSolution">Solution which is applied to the puzzle.</param>
    public void ApplySingleStepSolution(SingleStepSolution singleStepSolution)
    {
      if (singleStepSolution.Result == null &&
        (singleStepSolution.Eliminations == null || singleStepSolution.Eliminations.Length == 0))
        return;

      int[] oldCanBe = new int[] { };

      if (singleStepSolution.Result != null)
      {
        var cell = Cells[singleStepSolution.Result.IndexOfRow, singleStepSolution.Result.IndexOfColumn];
        oldCanBe = cell.CanBe.ToArray();
        cell.Value = singleStepSolution.Result.Value;
      }
      if (singleStepSolution.Eliminations != null)
      {
        foreach (var elimination in singleStepSolution.Eliminations)
        {
          Cells[elimination.IndexOfRow, elimination.IndexOfColumn].CanBe.Remove(elimination.Value);
        }
      }

      _steps.Add(new Tuple<SingleStepSolution, int[]>(singleStepSolution, oldCanBe.ToArray()));
    }

    /// <summary>
    /// Undoes the last applied <see cref="SingleStepSolution"/>.
    /// </summary>
    /// <returns><see cref="SingleStepSolution"/> which was undone, or null if nothing was undone.</returns>
    public SingleStepSolution UndoLastSingleStepSolution()
    {
      if (_steps.Count == 0)
        return null;

      var step = _steps.Last();
      var singleStepSolution = step.Item1;

      if (singleStepSolution.Result == null &&
        (singleStepSolution.Eliminations == null || singleStepSolution.Eliminations.Length == 0))
        return null;

      if (singleStepSolution.Result != null)
      {
        var cell = Cells[singleStepSolution.Result.IndexOfRow, singleStepSolution.Result.IndexOfColumn];
        cell.Value = 0;
        cell.CanBe.AddRange(step.Item2);
      }
      if (singleStepSolution.Eliminations != null)
      {
        foreach (var elimination in singleStepSolution.Eliminations)
        {
          Cells[elimination.IndexOfRow, elimination.IndexOfColumn].CanBe.Add(elimination.Value);
        }

        // cell.CanBe is now no longer sorted
        foreach(var cell in Cells.OfType<Cell>())
        {
          cell.CanBe.Sort();
        }
      }

      _steps.Remove(step);

      return singleStepSolution;
    }

    /// <summary>
    /// Converts puzzle into a 2D integer array.
    /// </summary>
    /// <returns>2D integer array where values represent values of cells in the puzzle.</returns>
    public int[,] ToIntArray()
    {
      var result = new int[NumberOfRowsOrColumnsInPuzzle, NumberOfRowsOrColumnsInPuzzle];

      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          result[i, j] = Cells[i, j].Value;
        }
      }
      return result;
    }

    /// <summary>
    /// Returns zero-based row and column index of the <paramref name="cell"/>.
    /// Returns -1 for both row and column index if <paramref name="cell"/> is not part of the puzzle.
    /// </summary>
    public (int RowIndex, int ColumnIndex) GetCellIndex(Cell cell)
    {
      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (Cells[i, j] == cell)
            return (i, j);
        }
      }

      return (-1, -1);
    }

    /// <summary>
    /// Returns zero-based row and column index of the block which contains the <paramref name="cell"/>.
    /// Returns -1 for both row and column index if <paramref name="cell"/> is not part of the puzzle.
    /// </summary>
    public (int RowIndex, int ColumnIndex) GetBlockIndex(Cell cell)
    {
      for (int i = 0; i < NumberOfRowsOrColumnsInPuzzle; i++)
      {
        for (int j = 0; j < NumberOfRowsOrColumnsInPuzzle; j++)
        {
          if (Cells[i, j] == cell)
            return (i / NumberOfRowsOrColumnsInBlock, j / NumberOfRowsOrColumnsInBlock);
        }
      }

      return (-1, -1);
    }

    public override string ToString() => Formatter.PuzzleToString(this);
  }
}
