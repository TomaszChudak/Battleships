using Battleships.Logic.Helpers;
using FluentAssertions;
using Xunit;

namespace Battleships.Logic.TestsVerify.Helpers
{
    public class RandomWrapperTests
    {
        private readonly IRandomWrapper _sut;

        public RandomWrapperTests()
        {
            _sut = new RandomWrapper();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public void Next_GivenMaxValue_ReturnRightResult(int maxValue)
        {
            var result = _sut.Next(maxValue);

            result.Should().BeGreaterOrEqualTo(0);
            result.Should().BeLessThan(maxValue);
        }
    }
}