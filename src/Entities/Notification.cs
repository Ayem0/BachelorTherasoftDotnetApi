using BachelorTherasoftDotnetApi.src.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Notification : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
}
