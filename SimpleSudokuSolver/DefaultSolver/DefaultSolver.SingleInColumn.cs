using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
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
              return new SingleStepSolution(i, column.ColumnIndex, value, "Single in Column");
          }
        }
      }

      return null;
    }
  }
}
