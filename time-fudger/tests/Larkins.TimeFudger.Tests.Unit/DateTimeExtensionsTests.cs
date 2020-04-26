using System;
using FluentAssertions;
using Xunit;

namespace Larkins.TimeFudger.Tests.Unit
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void RoundToInterval_AdjustToNearest5Min_DateTimeAdjustedUp()
        {
            // Arrange
            var initialDateTime = new DateTime(2019, 7, 29, 13, 17, 30);
            var timeSpan = new TimeSpan(0, 0, 5, 0);
            var expected = new DateTime(2019, 7, 29, 13, 20, 0);

            // Act
            var result = initialDateTime.RoundToInterval(timeSpan);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void RoundToInterval_AdjustToNearest5Min_DateTimeAdjustedDown()
        {
            // Arrange
            var initialDateTime = new DateTime(2019, 7, 29, 13, 17, 29);
            var timeSpan = new TimeSpan(0, 0, 5, 0);
            var expected = new DateTime(2019, 7, 29, 13, 15, 0);

            // Act
            var result = initialDateTime.RoundToInterval(timeSpan);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void RoundToInterval_NoInterval_NoChangeToDateTime()
        {
            // Arrange
            var initialDateTime = new DateTime(2019, 7, 29, 13, 17, 32);
            var timeSpan = new TimeSpan(0, 0, 0, 0);
            var expected = new DateTime(2019, 7, 29, 13, 17, 32);

            // Act
            var result = initialDateTime.RoundToInterval(timeSpan);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void RoundToInterval_AdjustToNearest5Min_NoChangeToDateTime()
        {
            // Arrange
            var initialDateTime = new DateTime(2019, 7, 29, 13, 20, 0);
            var timeSpan = new TimeSpan(0, 0, 5, 0);
            var expected = new DateTime(2019, 7, 29, 13, 20, 0);

            // Act
            var result = initialDateTime.RoundToInterval(timeSpan);

            // Assert
            result.Should().Be(expected);
        }
    }
}