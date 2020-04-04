using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver.Strategy
{
  /// <summary>
  /// Strategy looks for two cells in the same row / column / block that have exactly the same two candidate values.
  /// If such two cells are found, then the two candidate values cannot be in any other cell in the same row / column / block.
  /// </summary>
  /// <remarks>
  /// See also:
  /// - https://sudoku9x9.com/naked_pair.html
  /// - http://www.sudokuwiki.org/Naked_Candidates
  /// </remarks>
  public class NakedPair : ISudokuSolverStrategy
  {
    public string StrategyName => "Naked Pair";

    public SingleStepSolution SolveSingleStep(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetNakedPairEliminations(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetNakedPairEliminations(column.Cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        eliminations.AddRange(GetNakedPairEliminations(block.Cells.OfType<Cell>(), sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.Distinct().ToArray(), StrategyName) :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetNakedPairEliminations(IEnumerable<Cell> cells, SudokuPuzzle sudokuPuzzle)
    {
      var cellsWithNoValue = cells.Where(x => !x.HasValue).ToArray();
      var nakedPairCandidates = cellsWithNoValue.Where(x => x.CanBe.Count == 2).ToArray();
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var nakedPairCandidate in nakedPairCandidates)
      {
        foreach (var otherNakedPairCandidate in nakedPairCandidates)
        {
          if (nakedPairCandidate == otherNakedPairCandidate)
            continue;

          if (nakedPairCandidate.CanBe.SequenceEqual(otherNakedPairCandidate.CanBe))
          {
            foreach (var cell in cellsWithNoValue)
            {
              if (cell == nakedPairCandidate || cell == otherNakedPairCandidate)
                continue;

              foreach (var candidate in nakedPairCandidate.CanBe)
              {
                if (cell.CanBe.Contains(candidate))
                {
                  eliminations.Add(new SingleStepSolution.Candidate(cell.RowIndex, cell.ColumnIndex, candidate));
                }
              }
            }
          }
        }
      }

      return eliminations;
    }
  }
}
