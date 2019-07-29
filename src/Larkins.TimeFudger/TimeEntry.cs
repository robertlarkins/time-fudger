using System;

namespace Larkins.TimeFudger
{
    public class TimeEntry
    {
        public TimeEntry(TimePeriod timePeriod)
        {
            TimePeriod = timePeriod;
        }

        public TimePeriod TimePeriod { get; }
    }
}
