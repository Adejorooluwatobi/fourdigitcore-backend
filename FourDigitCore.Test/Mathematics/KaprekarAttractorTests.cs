using FluentAssertions;
using Xunit;

namespace FourDigitCore.Test.Mathematics;

public class KaprekarAttractorTests
{
    private const int KaprekarConstant = 6174;

    private static (int next, string stepFormula) KaprekarStep(int num)
    {
        var digits = num.ToString("D4").ToCharArray();
        Array.Sort(digits);
        var ascStr = new string(digits);
        Array.Reverse(digits);
        var descStr = new string(digits);

        var desc = int.Parse(descStr);
        var asc = int.Parse(ascStr);
        var result = desc - asc;

        return (result, $"{descStr} - {ascStr} = {result:D4}");
    }

    [Fact]
    public void AttractorInvariant_6174MinusItself_ShouldEqual6174()
    {
        // Act
        var (next, formula) = KaprekarStep(KaprekarConstant);

        // Assert
        next.Should().Be(KaprekarConstant);
        formula.Should().Be("7641 - 1467 = 6174");
    }

    [Theory]
    [InlineData(3524, 3)] // 3524 -> 5432 - 2345 = 3087 -> 8730 - 0378 = 8352 -> 8532 - 2358 = 6174
    [InlineData(1234, 3)]
    [InlineData(5432, 3)]
    [InlineData(9831, 7)] // max iterations example
    [InlineData(1000, 5)]
    [InlineData(2111, 5)]
    public void ConvergenceRoutine_ShouldReach6174_Within7Steps(int startingNumber, int expectedSteps)
    {
        // Act
        var current = startingNumber;
        var steps = 0;
        var history = new List<string>();

        while (current != KaprekarConstant && steps < 8)
        {
            var (next, formula) = KaprekarStep(current);
            history.Add(formula);
            current = next;
            steps++;
        }

        // Assert
        current.Should().Be(KaprekarConstant);
        steps.Should().Be(expectedSteps);
        steps.Should().BeLessThanOrEqualTo(7, "Kaprekar's theorem guarantees convergence in at most 7 steps");
    }
}
