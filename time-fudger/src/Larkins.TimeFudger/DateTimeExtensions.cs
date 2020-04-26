using System;

namespace Larkins.TimeFudger
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Rounds to interval. The interval starts at the midnight of the provided day.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="interval">The interval.</param>
        /// <returns></returns>
        public static DateTime RoundToInterval(this DateTime dateTime, TimeSpan interval)
        {
            var intervalTicks = interval.Ticks;

            if (intervalTicks == 0)
            {
                return dateTime;
            }

            var ticksSinceMidnight = dateTime.TimeOfDay.Ticks;
            var ticksFromLastIntervalStep = ticksSinceMidnight % intervalTicks;

            if (ticksFromLastIntervalStep == 0)
            {
                return dateTime;
            }

            var ticksFromMidnightToClosestInterval = intervalTicks * (int)Math.Round(ticksSinceMidnight / (double)intervalTicks);
            var ticksToAdjustBy = ticksFromMidnightToClosestInterval - ticksSinceMidnight;

            return dateTime.AddTicks(ticksToAdjustBy);
        }
    }
}