using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class RepetitionRequest
{
    public required Interval RepetitionInterval { get; set; }
    public required int RepetitionNumber { get; set; }
    public required DateOnly RepetitionEndDate { get; set; }
    // public List<DayOfWeek>? DayOfWeek { get; set; } // TODO a remettre 


}
