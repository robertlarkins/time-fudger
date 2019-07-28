using System;

namespace Larkins.TimeFudger
{
    public class TimeEntry
    {
        public TimeEntry(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
    }
}
