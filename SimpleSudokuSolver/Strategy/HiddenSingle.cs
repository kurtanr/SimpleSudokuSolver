using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy runs after <see cref="BasicElimination"/> when we have already reduced number of candidates in each empty cell.
  /// For each row, column and block of the puzzle, strategy:
  /// - iterates through all the possible values of that row, column and block
  /// - looks for a case where the possible value can only be in one cell
  ///   (only one cell's list of candidates contains this possible value)
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/hidden_single.html
  /// - http://www.sudokuwiki.org/Getting_Started (Last Remaining Cell in a Box/Row/Column)
  /// </remarks>
  public class HiddenSingle : ISudokuSolverStrategy
  {
    public string StrategyName => "Hidden Single";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var rowSolution = GetHiddenSingle(sudokuPuzzle, row.Cells);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = GetHiddenSingle(sudokuPuzzle, column.Cells);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = GetHiddenSingle(sudokuPuzzle, block.Cells.OfType<Cell>().ToArray());
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution GetHiddenSingle(SudokuPuzzle sudokuPuzzle, Cell[] cells)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var usedValues = cells.Where(x => x.HasValue).Select(x => x.Value).ToArray();
      var possibleValues = sudokuPuzzle.PossibleCellValues.Except(usedValues).ToArray();

      // if a possible value can only be in one cell, then it must be in that cell
      foreach (var possibleValue in possibleValues)
      {
        var candidateCells = cellsWithNoValue.Where(x => x.CanBe.Contains(possibleValue)).ToArray();
        if (candidateCells.Length == 1)
        {
          var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(candidateCells[0]);
          return new SingleStepSolution(RowIndex, ColumnIndex, possibleValue, StrategyName);
        }
      }

      return null;
    }
  }
}
