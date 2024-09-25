﻿using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

internal class Comment : BaseModel
{
    public required string MemberId { get; set; }
    public required string TaskId { get; set; }
    public required string Content { get; set; }
}
