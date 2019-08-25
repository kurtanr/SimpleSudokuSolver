using NUnit.Framework;
using System;

namespace SimpleSudokuSolver.Tests
{
  [TestFixture]
  public class SingleStepSolutionTests
  {
    [Test]
    public void CandidateEqualityTest()
    {
      var candidate = new SingleStepSolution.Candidate(1, 2, 3);
      var otherCandidate = new SingleStepSolution.Candidate(1, 2, 3);

      Assert.That(candidate != null);
      Assert.That(null != candidate);
      Assert.That(candidate == otherCandidate);
      Assert.That(candidate.Equals(otherCandidate));
      Assert.That(candidate.Equals((object)otherCandidate));
      Assert.That(candidate.GetHashCode() == otherCandidate.GetHashCode());

      var rowDiffers = new SingleStepSolution.Candidate(0, 2, 3);
      Assert.That(candidate != rowDiffers);

      var columnDiffers = new SingleStepSolution.Candidate(1, 0, 3);
      Assert.That(candidate != columnDiffers);

      var valueDiffers = new SingleStepSolution.Candidate(1, 2, 0);
      Assert.That(candidate != valueDiffers);

      candidate = null;
      otherCandidate = null;
      Assert.That(candidate == otherCandidate);
    }

    [Test]
    public void CandidateToStringTest()
    {
      var candidate = new SingleStepSolution.Candidate(1, 2, 3);
      Assert.That(candidate.ToString(), Is.EqualTo("[1,2] - 3"));
    }

    [Test]
    public void SingleStepSolutionToStringTest()
    {
      var resultSolution = new SingleStepSolution(1, 2, 3, "test");
      var expectedResultText = "Row 2 Column 3 Value 3 [test]";
      Assert.That(resultSolution.ToString(), Is.EqualTo(expectedResultText));
      Assert.That(resultSolution.SolutionDescription, Is.EqualTo(expectedResultText));

      var eliminationSolution = new SingleStepSolution(new[]
      {
        new SingleStepSolution.Candidate(1, 2, 3),
        new SingleStepSolution.Candidate(1, 2, 4)
      }, "test");
      var expectedEliminationText = $"Eliminated candidates [test]:{Environment.NewLine}- Row 2 Column 3 Values 3,4";
      Assert.That(eliminationSolution.ToString(), Is.EqualTo(expectedEliminationText));
      Assert.That(eliminationSolution.SolutionDescription, Is.EqualTo(expectedEliminationText));
    }
  }
}
