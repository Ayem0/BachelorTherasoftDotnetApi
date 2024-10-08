using System;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RepetitionService : IRepetitionService
{
    public DateTime IncrementDateTime(DateTime date, Interval repetitionInterval, int repetitionNumber)
    {
        return repetitionInterval switch
        {
            Interval.Day => date.AddDays(repetitionNumber),
            Interval.Week => date.AddDays(repetitionNumber * 7),
            Interval.Month => date.AddMonths(repetitionNumber),
            _ => date.AddYears(repetitionNumber),
        };
    }
    public DateOnly IncrementDateOnly(DateOnly date, Interval repetitionInterval, int repetitionNumber)
    {
        return repetitionInterval switch
        {
            Interval.Day => date.AddDays(repetitionNumber),
            Interval.Week => date.AddDays(repetitionNumber * 7),
            Interval.Month => date.AddMonths(repetitionNumber),
            _ => date.AddYears(repetitionNumber),
        };
    }
}
