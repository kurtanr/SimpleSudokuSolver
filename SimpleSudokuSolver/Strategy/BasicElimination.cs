using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// For each row, column and block of the puzzle, strategy eliminates candidate values from empty cells.
  /// Candidate values that are eliminated are the already known values in the row, column and block of the empty cell.
  /// </summary>
  public class BasicElimination : ISudokuSolverStrategy
  {
    public string StrategyName => "Basic Elimination";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        var cells = row.Cells;
        eliminations.AddRange(GetEliminations(cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cells = column.Cells;
        eliminations.AddRange(GetEliminations(cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var cells = block.Cells.OfType<Cell>().ToArray();
        eliminations.AddRange(GetEliminations(cells, sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.ToArray(), StrategyName) :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetEliminations(Cell[] cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithValue = cells.Where(x => x.HasValue).ToArray();
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();

      List<SingleStepSolution.Candidate> eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var cellWithNoValue in cellsWithNoValue)
      {
        foreach (var cellWithValue in cellsWithValue)
        {
          if (cellWithNoValue.CanBe.Contains(cellWithValue.Value))
          {
            var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cellWithNoValue);
            eliminations.Add(new SingleStepSolution.Candidate(RowIndex, ColumnIndex, cellWithValue.Value));
          }
        }
      }

      return eliminations;
    }
  }
}
