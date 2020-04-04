using SimpleSudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy is iterating through each row:
  /// - it is looking for a value which can be present only in two cells of the row
  /// If such a value is found, and there is another row where situation is the same (same value, same two columns),
  /// then all other candidates for this value in the two columns can be eliminated.
  /// Same is true when iterating through columns instead of rows (then we are eliminating candidates in rows).
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/x_wing.html
  /// - http://www.sudokuwiki.org/X_Wing_Strategy
  /// </remarks>
  public class XWing : ISudokuSolverStrategy
  {
    public string StrategyName => "X-Wing";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      var xWingMembersPerRow = GetXWingMembers(sudokuPuzzle, true);
      eliminations.AddRange(GetEliminations(sudokuPuzzle, xWingMembersPerRow, true));

      var xWingMembersPerColumn = GetXWingMembers(sudokuPuzzle, false);
      eliminations.AddRange(GetEliminations(sudokuPuzzle, xWingMembersPerColumn, false));

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), StrategyName) :
        null;
    }

    /// <summary>
    /// Iterates the entire puzzle either per row or per columns and returns all found XWings.
    /// </summary>
    /// <param name="sudokuPuzzle">Sudoku puzzle.</param>
    /// <param name="perRow">Determines if the method is iterating per row or per column.</param>
    /// <returns>
    /// Tuple where:
    /// - Item1 is the value which is in the XWing
    /// - Item2, Item3, Item4, Item5 are four cells that are members of the XWing
    /// </returns>
    private Tuple<int, Cell, Cell, Cell, Cell>[] GetXWingMembers(SudokuPuzzle sudokuPuzzle, bool perRow)
    {
      var candidatesPerRow = new List<Tuple<int, int, int>[]>();
      var candidatesPerColumn = new List<Tuple<int, int, int>[]>();

      Tuple<int, int, int>[] ToCandidatesWithRowOrCellIndex(Tuple<int, Cell, Cell>[] candidates)
      {
        return candidates.Select(
          x =>
          {
            var cell1 = x.Item2;
            var cell2 = x.Item3;
            return new Tuple<int, int, int>(x.Item1, 
              perRow ? cell1.ColumnIndex : cell1.RowIndex,
              perRow ? cell2.ColumnIndex : cell2.RowIndex);
          }
        ).ToArray();
      }

      var xWingMembers = new List<Tuple<int, Cell, Cell, Cell, Cell>>();

      if (perRow)
      {
        for (int i = 0; i < sudokuPuzzle.Rows.Length; i++)
        {
          var candidates = GetCandidates(sudokuPuzzle.Rows[i].Cells, sudokuPuzzle.PossibleCellValues);
          var candidatesWithColumnIndex = ToCandidatesWithRowOrCellIndex(candidates);
          candidatesPerRow.Add(candidatesWithColumnIndex);
        }

        for (int i = 0; i < sudokuPuzzle.Rows.Length - 1; i++)
        {
          var row1 = candidatesPerRow[i];

          for (int j = i + 1; j < sudokuPuzzle.Rows.Length; j++)
          {
            var row2 = candidatesPerRow[j];

            var intersect = row1.Intersect(row2).ToArray();
            if (intersect.Length > 0)
            {
              foreach (var intersectInstance in intersect)
              {
                var value = intersectInstance.Item1;
                var cell1 = sudokuPuzzle.Cells[i, intersectInstance.Item2];
                var cell2 = sudokuPuzzle.Cells[i, intersectInstance.Item3];
                var cell3 = sudokuPuzzle.Cells[j, intersectInstance.Item2];
                var cell4 = sudokuPuzzle.Cells[j, intersectInstance.Item3];
                xWingMembers.Add(new Tuple<int, Cell, Cell, Cell, Cell>(value, cell1, cell2, cell3, cell4));
              }
            }
          }
        }
      }
      else
      {
        for (int i = 0; i < sudokuPuzzle.Columns.Length; i++)
        {
          var candidates = GetCandidates(sudokuPuzzle.Columns[i].Cells, sudokuPuzzle.PossibleCellValues);
          var candidatesWithRowIndex = ToCandidatesWithRowOrCellIndex(candidates);
          candidatesPerColumn.Add(candidatesWithRowIndex);
        }

        for (int i = 0; i < sudokuPuzzle.Columns.Length - 1; i++)
        {
          var column1 = candidatesPerColumn[i];

          for (int j = i + 1; j < sudokuPuzzle.Columns.Length; j++)
          {
            var column2 = candidatesPerColumn[j];

            var intersect = column1.Intersect(column2).ToArray();
            if (intersect.Length > 0)
            {
              foreach (var intersectInstance in intersect)
              {
                var value = intersectInstance.Item1;
                var cell1 = sudokuPuzzle.Cells[intersectInstance.Item2, i];
                var cell2 = sudokuPuzzle.Cells[intersectInstance.Item3, i];
                var cell3 = sudokuPuzzle.Cells[intersectInstance.Item2, j];
                var cell4 = sudokuPuzzle.Cells[intersectInstance.Item3, j];
                xWingMembers.Add(new Tuple<int, Cell, Cell, Cell, Cell>(value, cell1, cell2, cell3, cell4));
              }
            }
          }
        }
      }

      return xWingMembers.ToArray();
    }

    /// <summary>
    /// Returns all the candidates that can be eliminated using the XWings.
    /// </summary>
    private SingleStepSolution.Candidate[] GetEliminations(SudokuPuzzle sudokuPuzzle, Tuple<int, Cell, Cell, Cell, Cell>[] xWingMembers, bool perRow)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var xWingMember in xWingMembers)
      {
        var value = xWingMember.Item1;

        // diagonal cells
        var firstCell = xWingMember.Item2;
        var secondCell = xWingMember.Item5;

        if (perRow)
        {
          var column1 = sudokuPuzzle.Columns[firstCell.ColumnIndex];
          var column2 = sudokuPuzzle.Columns[secondCell.ColumnIndex];
          var row1Index = firstCell.RowIndex;
          var row2Index = secondCell.RowIndex;

          // eliminations are those cell that:
          // - are in 'column1' or 'column2'
          // - 'value' is a possible value for that cell
          // - are not in row1Index or row2Index
          foreach (var cell in column1.Cells.Union(column2.Cells))
          {
            if (cell.CanBe.Contains(value) && cell.RowIndex != row1Index && cell.RowIndex != row2Index)
              eliminations.Add(new SingleStepSolution.Candidate(cell.RowIndex, cell.ColumnIndex, value));
          }
        }
        else
        {
          var row1 = sudokuPuzzle.Rows[firstCell.RowIndex];
          var row2 = sudokuPuzzle.Rows[secondCell.RowIndex];
          var column1Index = firstCell.ColumnIndex;
          var column2Index = secondCell.ColumnIndex;

          // eliminations are those cell that:
          // - are in 'row1' or 'row2'
          // - 'value' is a possible value for that cell
          // - are not in column1Index or column2Index
          foreach (var cell in row1.Cells.Union(row2.Cells))
          {
            if (cell.CanBe.Contains(value) && cell.ColumnIndex != column1Index && cell.ColumnIndex != column2Index)
              eliminations.Add(new SingleStepSolution.Candidate(cell.RowIndex, cell.ColumnIndex, value));
          }
        }
      }


      return eliminations.ToArray();
    }

    /// <summary>
    /// For each value in <paramref name="possibleCellValues"/> method iterates through all the <paramref name="cells"/> 
    /// and is looking for a pair of cells that are the only cells that can contain the possible value.
    /// </summary>
    /// <param name="cells">Cells of a row or column.</param>
    /// <param name="possibleCellValues"><see cref="SudokuPuzzle.PossibleCellValues"/></param>
    /// <returns>
    /// Tuple where:
    /// - Item1 is possible value
    /// - Item2 is first cell containing possible value
    /// - Item3 is second cell containing possible value
    /// </returns>
    private Tuple<int, Cell, Cell>[] GetCandidates(Cell[] cells, int[] possibleCellValues)
    {
      var result = new List<Tuple<int, Cell, Cell>>();

      foreach (var possibleCellValue in possibleCellValues)
      {
        var cellsContaingValue = cells.Where(x => x.CanBe.Contains(possibleCellValue)).ToArray();
        if (cellsContaingValue.Length == 2)
        {
          result.Add(new Tuple<int, Cell, Cell>(possibleCellValue, cellsContaingValue[0], cellsContaingValue[1]));
        }
      }

      return result.ToArray();
    }
  }
}
