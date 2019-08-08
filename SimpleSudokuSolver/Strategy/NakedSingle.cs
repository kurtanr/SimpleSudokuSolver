using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy runs after <see cref="BasicElimination"/> when we have already reduced number of candidates in each empty cell.
  /// Strategy iterates through all the empty cells of the puzzle and looks for cells that can contain only one value
  /// (cell's list of candidates contains only one value).
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
      var cellsWithNoValue = sudokuPuzzle.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();

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
