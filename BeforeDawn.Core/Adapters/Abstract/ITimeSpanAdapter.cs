using System;

namespace BeforeDawn.Core.Adapters.Abstract
{
    internal interface ITimeSpanAdapter
    {
        TimeSpan FromMinutes(double value);
    }
}