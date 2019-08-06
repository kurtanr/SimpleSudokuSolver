using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy runs after <see cref="BasicElimination"/> when we have already reduced number of candidates in each empty cell.
  /// For each row, column and block of the puzzle, strategy:
  /// - iterates through all the empty cells of that row, column and block
  /// - looks for a case where a cell can contain only one value
  ///   (cell's list of candidates contains only one value)
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/naked_single.html
  /// - http://www.sudokuwiki.org/Getting_Started (The Last Possible Number)
  /// </remarks>
  public class NakedSingle : ISudokuSolverStrategy
  {
    public string StrategyName => "Naked Single";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var rowSolution = GetNakedSingle(sudokuPuzzle, row.Cells);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = GetNakedSingle(sudokuPuzzle, column.Cells);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = GetNakedSingle(sudokuPuzzle, block.Cells.OfType<Cell>());
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution GetNakedSingle(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      // if cell can contain only one value, then it must contain that value
      foreach (var cell in cellsWithNoValue)
      {
        if (cell.CanBe.Count == 1)
        {
          var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cell);
          return new SingleStepSolution(RowIndex, ColumnIndex, cell.CanBe.Single(), StrategyName);
        }
      }

      return null;
    }
  }
}
