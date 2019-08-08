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
    public string StrategyName { get; private set; }

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var row in sudokuPuzzle.Rows)
      {
        StrategyName = "Single in Row";
        var rowSolution = GetSingleInCells(sudokuPuzzle, row.Cells);
        if (rowSolution != null)
          return rowSolution;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        StrategyName = "Single in Column";
        var columnSolution = GetSingleInCells(sudokuPuzzle, column.Cells);
        if (columnSolution != null)
          return columnSolution;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        StrategyName = "Single in Block";
        var blockSolution = GetSingleInCells(sudokuPuzzle, block.Cells.OfType<Cell>());
        if (blockSolution != null)
          return blockSolution;
      }

      return null;
    }

    private SingleStepSolution GetSingleInCells(SudokuPuzzle sudokuPuzzle, IEnumerable<Cell> cells)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      // If only single cell in the group does not have a value
      if (cellsWithNoValue.Length == 1)
      {
        var knownValues = cells.Where(x => x.HasValue).Select(x => x.Value);
        var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();

        var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cellsWithNoValue[0]);
        return new SingleStepSolution(RowIndex, ColumnIndex, value, StrategyName);
      }

      return null;
    }
  }
}
