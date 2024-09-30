using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    public MemberService(IMemberRepository memberRepository,  IWorkspaceRepository workspaceRepository, UserManager<User> userManager)
    {
        _memberRepository = memberRepository;
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
    }
    public async Task<MemberDto?> CreateAsync(string workspaceId, string userId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var member = new Member(user, workspace) {
            Workspace = workspace,
            User = user
        };

        await _memberRepository.CreateAsync(member);
        return new MemberDto(member);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return false;

        await _memberRepository.DeleteAsync(member);
        return true;
    }

    public async Task<MemberDto?> GetByIdAsync(string id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        return member != null ? new MemberDto(member) : null;
    }

    public async Task<MemberDto?> UpdateAsync(string id, Status? newStatus)
    {
        var member = await _memberRepository.GetByIdAsync(id);  
        if (member == null || newStatus == null) return null;

        member.Status = (Status)newStatus;
        await _memberRepository.UpdateAsync(member);
        return new MemberDto(member);
    }
}
