using SimpleSudokuSolver.Model;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution NakedPair(SudokuPuzzle sudokuPuzzle)
    {
      var eliminations = new List<SingleStepSolution.Candidate>();

      foreach (var row in sudokuPuzzle.Rows)
      {
        eliminations.AddRange(GetEliminationsPair(row.Cells, sudokuPuzzle));
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        eliminations.AddRange(GetEliminationsPair(column.Cells, sudokuPuzzle));
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        eliminations.AddRange(GetEliminationsPair(block.Cells.OfType<Cell>().ToArray(), sudokuPuzzle));
      }

      return eliminations.Count > 0 ?
        new SingleStepSolution(eliminations.ToArray(), "Naked Pair") :
        null;
    }

    private IEnumerable<SingleStepSolution.Candidate> GetEliminationsPair(Cell[] cells, SudokuPuzzle sudokuPuzzle)
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
                  cell.CanBe.Remove(candidate);
                  var (RowIndex, ColumnIndex) = sudokuPuzzle.GetCellIndex(cell);
                  eliminations.Add(new SingleStepSolution.Candidate(RowIndex, ColumnIndex, candidate));
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
