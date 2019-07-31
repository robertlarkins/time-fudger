using System;
using System.Collections.Generic;
using System.Linq;

namespace Larkins.TimeFudger
{
    public class TimeSheetAdjuster
    {
        private readonly TimeSpan _stepInterval;
        private readonly TimeSheet _timeSheet;
        private readonly List<TimePeriod> _timePeriods;

        private DateTime _nextStartTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSheetAdjuster"/> class.
        /// </summary>
        /// <param name="stepInterval">The nearest step that time entries are adjusted to.</param>
        public TimeSheetAdjuster(TimeSpan stepInterval, TimeSheet timeSheet)
        {
            _stepInterval = stepInterval;
            _timeSheet = timeSheet;

            var sortedTimeEntries = timeSheet.GetSortedTimeEntries();

            _timePeriods = sortedTimeEntries.Select(x => x.TimePeriod).ToList();
        }

        /// <summary>
        /// Adjusts the time sheet.
        /// </summary>
        /// <returns>
        /// A new TimeSheet with the time entries adjusted.
        /// </returns>
        public TimeSheet GetAdjustedTimeSheet()
        {
            var timePeriods = ProcessTimeEntries();

            var newTimeSheet = new TimeSheet();

            foreach (var timePeriod in timePeriods)
            {
                var timeEntry = new TimeEntry(timePeriod);
                newTimeSheet.TimeEntries.Add(timeEntry);
            }

            return newTimeSheet;
        }

        private List<TimePeriod> ProcessTimeEntries()
        {
            var adjustedTimeEntries = new List<TimePeriod>();

            if (_timePeriods.Count == 0)
            {
                return adjustedTimeEntries;
            }

            var currentTimePeriod = _timePeriods.First();
            _nextStartTime = RoundTimeToInterval(currentTimePeriod.Start);

            for (var i = 0; i < _timePeriods.Count - 1; i++)
            {
                currentTimePeriod = _timePeriods[i];
                var nextTimePeriod = _timePeriods[i + 1];

                var adjustedTimeEntry = AdjustNonEndTimeEntry(currentTimePeriod, nextTimePeriod);

                adjustedTimeEntries.Add(adjustedTimeEntry);
            }

            // Add last Time entry
            var newLastTimeEntry = AdjustLastTimeEntry();
            adjustedTimeEntries.Add(newLastTimeEntry);

            return adjustedTimeEntries;
        }

        private TimePeriod AdjustNonEndTimeEntry(TimePeriod currentTimePeriod, TimePeriod nextTimePeriod)
        {
            var newEndTime = GetAdjustedEndTime(currentTimePeriod.End, nextTimePeriod.Start);
            var adjustedTimeEntry = new TimePeriod(_nextStartTime, newEndTime);

            _nextStartTime = newEndTime;

            return adjustedTimeEntry;
        }

        private TimePeriod AdjustLastTimeEntry()
        {
            var currentTimePeriod = _timePeriods.Last();
            var newLastEndTime = RoundTimeToInterval(currentTimePeriod.End);

            return new TimePeriod(_nextStartTime, newLastEndTime);
        }

        private DateTime GetAdjustedEndTime(
            DateTime currentEndTime,
            DateTime nextTimeStart)
        {
            try
            {
                return GetIntervalBetweenTwoDateTimes(currentEndTime, nextTimeStart);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private DateTime RoundTimeToInterval(DateTime dateTime)
        {
            return dateTime.RoundToInterval(_stepInterval);
        }

        private DateTime GetIntervalBetweenTwoDateTimes(DateTime dateTime1, DateTime dateTime2)
        {
            var timeDifference = (dateTime1 - dateTime2).Duration();

            if (timeDifference >= _stepInterval)
            {
                throw new ArgumentException("Difference between DateTimes is greater than the step interval");
            }

            if (dateTime1 > dateTime2)
            {
                var temp = dateTime1;
                dateTime1 = dateTime2;
                dateTime2 = temp;
            }

            var roundedDateTime1 = RoundTimeToInterval(dateTime1);
            var roundedDateTime2 = RoundTimeToInterval(dateTime2);

            if (roundedDateTime1 == roundedDateTime2)
            {
                return roundedDateTime1;
            }

            var totalTimeToInterval1 = AccumulatedTimeToInterval(dateTime1, dateTime2, roundedDateTime1);
            var totalTimeToInterval2 = AccumulatedTimeToInterval(dateTime1, dateTime2, roundedDateTime2);

            // If both have the same distance then take the first one
            if (totalTimeToInterval1 <= totalTimeToInterval2)
            {
                return roundedDateTime1;
            }

            return roundedDateTime2;
        }

        private TimeSpan AccumulatedTimeToInterval(
            DateTime dateTime1,
            DateTime dateTime2,
            DateTime interval)
        {
            var timeToInterval1 = (dateTime1 - interval).Duration();
            var timeToInterval2 = (dateTime2 - interval).Duration();

            return timeToInterval1 + timeToInterval2;
        }
    }
}