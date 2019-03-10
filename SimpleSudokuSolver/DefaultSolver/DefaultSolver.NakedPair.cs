using SimpleSudokuSolver.Model;
using System.Linq;

namespace SimpleSudokuSolver
{
  public partial class DefaultSolver
  {
    private SingleStepSolution NakedPair(SudokuPuzzle sudokuPuzzle)
    {
      int reducedNumberOfCandidates = 0;

      foreach (var row in sudokuPuzzle.Rows)
      {
        var cellsWithNoValue = row.Cells.Where(x => !x.HasValue).ToArray();
        if (NakedPairCore(cellsWithNoValue))
          reducedNumberOfCandidates++;
      }

      foreach (var column in sudokuPuzzle.Columns)
      {
        var cellsWithNoValue = column.Cells.Where(x => !x.HasValue).ToArray();
        if (NakedPairCore(cellsWithNoValue))
          reducedNumberOfCandidates++;
      }

      foreach (var block in sudokuPuzzle.Blocks)
      {
        var cellsWithNoValue = block.Cells.OfType<Cell>().Where(x => !x.HasValue).ToArray();
        if (NakedPairCore(cellsWithNoValue))
          reducedNumberOfCandidates++;
      }

      if (reducedNumberOfCandidates == 0)
        return null;

      return
        HiddenSingleCore(sudokuPuzzle, "HiddenSingle+NakedPair") ??
        NakedSingleCore(sudokuPuzzle, "NakedSingle+NakedPair");
    }

    private bool NakedPairCore(Cell[] cellsWithNoValue)
    {
      bool reducedNumberOfCandidates = false;
      var nakedPairCandidates = cellsWithNoValue.Where(x => x.CanBe.Count == 2).ToArray();

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
                  reducedNumberOfCandidates = true;
                }
                if (!cell.CannotBe.Contains(candidate))
                {
                  cell.CannotBe.Add(candidate);
                  reducedNumberOfCandidates = true;
                }
              }
            }
          }
        }
      }

      return reducedNumberOfCandidates;
    }
  }
}
