using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution SingleInBlock(SudokuPuzzle sudokuPuzzle)
    {
      foreach (var block in sudokuPuzzle.Blocks)
      {
        var blockCells = block.Cells.OfType<Cell>();
        var blocksWithNoValue = blockCells.Where(x => !x.HasValue).ToArray();

        // If only single cell in the block does not have a value
        if (blocksWithNoValue.Length == 1)
        {
          var knownValues = blockCells.Where(x => x.HasValue).Select(x => x.Value);
          var value = sudokuPuzzle.PossibleCellValues.Except(knownValues).Single();
          var cellWithNoValue = blockCells.Where(x => !x.HasValue).Single();

          var cellIndex = sudokuPuzzle.GetCellIndex(cellWithNoValue);
          return new SingleStepSolution(cellIndex.RowIndex, cellIndex.ColumnIndex, value,
            $"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {value} [Single in Block]");
        }
      }

      return null;
    }
  }
}
