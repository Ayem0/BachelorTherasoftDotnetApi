using System;

namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseEntity
{
    string Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
}
