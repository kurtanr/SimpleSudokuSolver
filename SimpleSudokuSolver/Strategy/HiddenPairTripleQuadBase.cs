using System.Collections.Generic;
using System.Linq;
using SimpleSudokuSolver.Model;

namespace SimpleSudokuSolver.Strategy
{
  public abstract class HiddenPairTripleQuadBase
  {
    protected abstract IEnumerable<SingleStepSolution.Candidate> GetHiddenEliminations(
      IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle);

    protected SingleStepSolution GetSingleStepSolution(SudokuPuzzle sudokuPuzzle, string strategyName)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetHiddenEliminations(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetHiddenEliminations(column.Cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        eliminations.AddRange(GetHiddenEliminations(block.Cells.OfType<Cell>(), sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), strategyName) :
        null;
    }

    /// <summary>
    /// Returns a dictionary where key is one of <see cref="SudokuPuzzle.PossibleCellValues"/> 
    /// and value is the collection of cells containing that value, but only if the <paramref name="cellsWithNoValue"/>
    /// contains a certain number of such cells (<paramref name="numberOfCellsContainingValue"/>).
    /// </summary>
    /// <param name="cellsWithNoValue">Empty cells of a single row/column/block.</param>
    /// <param name="sudokuPuzzle">Sudoku puzzle.</param>
    /// <param name="numberOfCellsContainingValue">Tells how many cells containing a value we are looking for.</param>
    /// <returns>See summary.</returns>
    protected IDictionary<int, Cell[]> GetHiddenCandidates(Cell[] cellsWithNoValue, SudokuPuzzle sudokuPuzzle,
      params int[] numberOfCellsContainingValue)
    {
      var candidates = new Dictionary<int, Cell[]>();

      foreach (var cellValue in sudokuPuzzle.PossibleCellValues)
      {
        var valueInCells = cellsWithNoValue.Where(x => x.CanBe.Contains(cellValue)).ToArray();
        if (numberOfCellsContainingValue.Contains(valueInCells.Length))
          candidates.Add(cellValue, valueInCells);
      }

      return candidates;
    }

    /// <summary>
    /// For each member of<paramref name="cell"/>'s <see cref="Cell.CanBe"/>:
    /// - if member is part of <paramref name="valuesToExclude"/>, ignore it
    /// - if member is not part of <paramref name="valuesToExclude"/> return it as an elimination
    /// </summary>
    /// <param name="cell">Cell which is analyzed for eliminations.</param>
    /// <param name="valuesToExclude">Values which are not elimination candidates.</param>
    /// <returns>See summary.</returns>
    protected IEnumerable<SingleStepSolution.Candidate> GetEliminations(Cell cell, params int[] valuesToExclude)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();
      var eliminatedValues = cell.CanBe.Except(valuesToExclude);
      foreach (var eliminatedValue in eliminatedValues)
      {
        eliminations.Add(new SingleStepSolution.Candidate(cell.RowIndex, cell.ColumnIndex, eliminatedValue));
      }

      return eliminations;
    }
  }
}
