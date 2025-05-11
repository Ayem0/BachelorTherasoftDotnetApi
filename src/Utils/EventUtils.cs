using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Utils;

public static class EventUtils
{
    public static bool IsInRange(Event e, DateTime start, DateTime end)
    {
        // event with same date or starting before and ending after
        return e.StartDate <= start && e.EndDate >= end ||
            // event starting after and ending before
            e.StartDate > start && e.EndDate < end ||
            // event starting before and ending before
            e.StartDate < start && e.EndDate > start && e.EndDate < end ||
            // event starting after and ending after
            e.StartDate > start && e.EndDate > end && e.StartDate < end;
    }
}
