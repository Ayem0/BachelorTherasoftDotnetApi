using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateAreaRequest
{
    public required string Name { get; set; }
    public required string LocationId { get; set; }
}
