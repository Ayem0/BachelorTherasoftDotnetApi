using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRepetitionService
{
    DateTime IncrementDateTime(DateTime date, Interval repetitionInterval, int repetitionNumber);
    DateOnly IncrementDateOnly(DateOnly date, Interval repetitionInterval, int repetitionNumber);
}
