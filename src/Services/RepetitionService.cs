using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Services;

public static class StaticRepetitionService
{
    public static DateTime IncrementDateTime(DateTime date, Interval repetitionInterval, int repetitionNumber)
    {
        return repetitionInterval switch
        {
            Interval.Day => date.AddDays(repetitionNumber),
            Interval.Week => date.AddDays(repetitionNumber * 7),
            Interval.Month => date.AddMonths(repetitionNumber),
            _ => date.AddYears(repetitionNumber),
        };
    }
    public static DateOnly IncrementDateOnly(DateOnly date, Interval repetitionInterval, int repetitionNumber)
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
