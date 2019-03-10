using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
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
  }
}
