using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface ISlotRepository : IBaseRepository<Slot>
{
    Task<List<Slot>> GetRepetitionsById(string id);
    Task<List<Slot>> GetByWorkpaceIdAsync(string id);
}
