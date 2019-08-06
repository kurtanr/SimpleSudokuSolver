using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// For each row, column and block of the puzzle, strategy is looking for a row/column/block that has only one empty cell.
  /// </summary>
  public class SingleInCells : ISudokuSolverStrategy
  {
    public string StrategyName => "Single in Cells";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        var rowSolution = GetSingleInCells(sudokuPuzzle, row.Cells, "Single in Row");
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var columnSolution = GetSingleInCells(sudokuPuzzle, column.Cells, "Single in Column");
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockSolution = GetSingleInCells(sudokuPuzzle, block.Cells.OfType<Cell>(), "Single in Block");
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution GetSingleInCells(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells, string strategyName)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      // If only single cell in the group does not have a value
      if (cellsWithNoValue.Length == 1)
      {
        var knownValues = cells.Where(x => x.HasValue).Select(x => x.Value);
        var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();

        var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cellsWithNoValue[0]);
        return new SingleStepSolution(RowIndex, ColumnIndex, value, "Single in Row");
      }

      return null;
    }
  }
}
