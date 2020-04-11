using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Implements brute-force search to find a solution for the puzzle.
  /// </remarks>
  /// <remarks>
  /// See also:
  /// - https://en.wikipedia.org/wiki/Sudoku_solving_algorithms#Backtracking
  /// </remarks>
  public class Backtracking : ISudokuSolverStrategy
  {
    public string StrategyName => "Backtracking (brute-force)";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var sudokuPuzzleCopy = GetDeepCopy(sudokuPuzzle);
      bool isSolved = SolvePuzzle(sudokuPuzzleCopy);
      if (!isSolved)
        return null;

      // find first cell with no value
      var cellWithNoValue = sudokuPuzzle.Cells.OfType<Cell>().First(x => !x.HasValue);

      // return value of that cell
      return new SingleStepSolution(cellWithNoValue.RowIndex, cellWithNoValue.ColumnIndex,
        sudokuPuzzleCopy.Cells[cellWithNoValue.RowIndex, cellWithNoValue.ColumnIndex].Value, StrategyName);
    }

    private SudokuPuzzle GetDeepCopy(SudokuPuzzle sudokuPuzzle)
    {
      var copy = new SudokuPuzzle(sudokuPuzzle.ToIntArray());
      foreach (var cell in sudokuPuzzle.Cells)
      {
        if (!cell.HasValue)
        {
          var cellInCopy = copy.Cells[cell.RowIndex, cell.ColumnIndex];
          cellInCopy.CanBe.Clear();
          cellInCopy.CanBe.AddRange(cell.CanBe);
        }
      }

      return copy;
    }

    private bool SolvePuzzle(SudokuPuzzle sudokuPuzzle)
    {
      if (sudokuPuzzle.IsSolved())
        return true;

      var unsolvedCells = sudokuPuzzle.Cells.OfType<Cell>().Where(x => !x.HasValue).OrderBy(x => x.CanBe.Count).ToArray();

      // for each cell
      foreach (var cell in unsolvedCells)
      {
        var row = sudokuPuzzle.Rows[cell.RowIndex];
        var column = sudokuPuzzle.Columns[cell.ColumnIndex];
        var blockIndex = sudokuPuzzle.GetBlockIndex(cell);
        var block = sudokuPuzzle.Blocks[blockIndex.RowIndex, blockIndex.ColumnIndex];

        var potentialCellValues = new List<int>(cell.CanBe);

        // for each potential value
        foreach (var value in potentialCellValues)
        {
          var usedInRow = row.Cells.Count(x => x.Value == value) > 0;
          var usedInColumn = column.Cells.Count(x => x.Value == value) > 0;
          var usedInBlock = block.Cells.OfType<Cell>().Count(x => x.Value == value) > 0;

          // if potential value already used in row, column or block, skip it
          if (usedInRow || usedInColumn || usedInBlock)
            continue;

          // try to recursively solve the puzzle
          cell.Value = value;
          if (SolvePuzzle(sudokuPuzzle))
          {
            return true;
          }
          // if puzzle cannot be solve, revert the value
          else
          {
            cell.Value = 0;
            cell.CanBe.AddRange(potentialCellValues);
          }
        }
        return false;
      }
      return false;
    }
  }
}
