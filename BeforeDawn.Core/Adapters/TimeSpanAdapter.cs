using System;
using BeforeDawn.Core.Adapters.Abstract;

namespace BeforeDawn.Core.Adapters
{
    class TimeSpanAdapter : ITimeSpanAdapter
    {
        public TimeSpan FromMinutes(double value)
        {
            return TimeSpan.FromMinutes(value);
        }
    }
}
