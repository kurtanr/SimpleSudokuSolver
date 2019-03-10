using NUnit.Framework;
using System.Collections.Generic;

namespace SimpleSudokuSolver.Tests
{
  [TestFixture]
  public class SampleSudokuTests
  {
    private readonly List<int[,]> _samplePuzzles = new List<int[,]>
    {
      new int[,]
      {
        // uses LockedCandidates
        { 0,0,7,0,0,0,3,0,2 },
        { 2,0,0,0,0,5,0,1,0 },
        { 0,0,0,8,0,1,4,0,0 },
        { 0,1,0,0,9,6,0,0,8 },
        { 7,6,0,0,0,0,0,4,9 },
        { 0,0,0,0,0,0,0,0,0 },
        { 0,0,0,1,0,3,0,0,0 },
        { 8,0,1,0,6,0,0,0,0 },
        { 0,0,0,7,0,0,0,6,3 }
      },
      new int[,]
      {
        // uses NakedPair
        { 9,7,1,2,4,3,8,6,5 },
        { 0,0,8,7,9,6,0,0,3 },
        { 6,0,3,1,5,8,7,0,9 },
        { 3,1,0,0,8,0,0,0,7 },
        { 7,0,0,3,1,0,0,5,8 },
        { 0,8,0,0,6,7,2,3,1 },
        { 8,3,4,6,7,5,0,0,2 },
        { 1,0,0,8,2,4,3,7,6 },
        { 2,6,7,9,3,1,5,8,4 }
      },
      /*new int[,]
      {
        // uses LockedCandidates, NakedPair
        { 0,0,0,0,0,0,0,8,0 },
        { 0,9,0,2,0,0,7,0,0 },
        { 2,1,0,5,0,0,0,0,3 },
        { 0,8,5,0,0,4,0,0,7 },
        { 0,0,0,1,6,0,0,0,0 },
        { 0,0,0,0,7,0,5,0,4 },
        { 5,3,1,4,0,9,0,7,0 },
        { 6,4,0,0,0,2,0,0,0 },
        { 8,0,0,0,0,0,9,0,0 }
      }*/
    };

    [Test]
    public void SolverCanSolveSamplesTest()
    {
      var solver = new DefaultSolver();

      foreach (var sample in _samplePuzzles)
      {
        var sudokuPuzzle = solver.Solve(sample);
        Assert.That(solver.IsSolved(sudokuPuzzle.ToIntArray()));
      }
    }
  }
}
