using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Larkins.TimeFudger.Tests.Unit
{
    public class TimeEntryAdjuster
    {
        [Fact]
        public void AdjustTimeEntries_EmptyTimeSheet_SameEntriesReturned()
        {
            // Arrange
            var timeSheet = new TimeSheet(); 
            var expected = new TimeSheet();

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_EntriesDontNeedAdjusting_SameEntriesReturned()
        {
            // Arrange
            var timePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 45, 0), (11, 45, 0)),
                CreateTimePeriod((11, 45, 0), (11, 55, 0)),
                CreateTimePeriod((11, 55, 0), (13, 10, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(timePeriods);
            var expected = CreateTimeSheetFromTimePeriods(timePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_ThreeEntriesToAdjust_TimeSheetWithCorrectAdjustments()
        {
            // Arrange
            var unadjustedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 42, 31), (11, 47, 30)),
                CreateTimePeriod((11, 45, 0), (11, 56, 0)),
                CreateTimePeriod((11, 59, 0), (13, 9, 0))
            };

            var expectedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 45, 0), (11, 45, 0)),
                CreateTimePeriod((11, 45, 0), (11, 55, 0)),
                CreateTimePeriod((11, 55, 0), (13, 10, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(unadjustedTimePeriods);
            var expected = CreateTimeSheetFromTimePeriods(expectedTimePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_EntriesAdjustedToNearest5Min_TimeSheetWithCorrectAdjustments()
        {
            // Arrange
            var unadjustedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 43, 0), (11, 47, 0)),
                CreateTimePeriod((11, 47, 0), (11, 56, 0))
            };

            var expectedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 45, 0), (11, 45, 0)),
                CreateTimePeriod((11, 45, 0), (11, 55, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(unadjustedTimePeriods);
            var expected = CreateTimeSheetFromTimePeriods(expectedTimePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_SingleEntryAdjustedToNearest5Min_TimeSheetWithCorrectAdjustments()
        {
            // Arrange
            var unadjustedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 43, 0), (11, 47, 0))
            };

            var expectedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((10, 45, 0), (11, 45, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(unadjustedTimePeriods);
            var expected = CreateTimeSheetFromTimePeriods(expectedTimePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_EndAndStartDontAdjustToSameValue_EndAndStartAdjustedToIntervalBetween()
        {
            // Arrange
            var unadjustedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((12, 0, 0), (12, 57, 0)),
                CreateTimePeriod((13, 1, 0), (14, 58, 0))
            };

            var expectedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((12, 0, 0), (13, 0, 0)),
                CreateTimePeriod((13, 0, 0), (15, 0, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(unadjustedTimePeriods);
            var expected = CreateTimeSheetFromTimePeriods(expectedTimePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void AdjustTimeEntries_EndAndStartDontAdjustToSameValue2_EndAndStartAdjustedToIntervalBetween()
        {
            // Arrange
            var unadjustedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((12, 0, 0), (12, 56, 0)),
                CreateTimePeriod((12, 59, 0), (14, 58, 0))
            };

            var expectedTimePeriods = new List<TimePeriod>
            {
                CreateTimePeriod((12, 0, 0), (12, 55, 0)),
                CreateTimePeriod((12, 55, 0), (15, 0, 0))
            };

            var timeSheet = CreateTimeSheetFromTimePeriods(unadjustedTimePeriods);
            var expected = CreateTimeSheetFromTimePeriods(expectedTimePeriods);

            var stepInterval = new TimeSpan(0, 0, 5, 0);
            var timeSheetAdjuster = new TimeSheetAdjuster(stepInterval, timeSheet);

            // Act
            var result = timeSheetAdjuster.GetAdjustedTimeSheet();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private TimeSheet CreateTimeSheetFromTimePeriods(List<TimePeriod> timePeriods)
        {
            var timeSheet = new TimeSheet();

            foreach (var timePeriod in timePeriods)
            {
                var timeEntry = new TimeEntry(timePeriod);

                timeSheet.TimeEntries.Add(timeEntry);
            }

            return timeSheet;
        }

        private TimePeriod CreateTimePeriod(
            (int h, int m, int s) start,
            (int h, int m, int s) end)
        {
            var startDate = CreateDateTimeOnSameDay(start.h, start.m, start.s);
            var endDate = CreateDateTimeOnSameDay(end.h, end.m, end.s);

            return new TimePeriod(startDate, endDate);
        }

        private DateTime CreateDateTimeOnSameDay(int hour, int minute, int second)
        {
            var year = 2019;
            var month = 7;
            var day = 29;

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}