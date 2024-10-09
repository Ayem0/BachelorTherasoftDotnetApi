using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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
    public async Task<Response<MemberDto?>> CreateAsync(string workspaceId, string userId)
    {   
        var existingMember = await _memberRepository.GetByUserWorkspaceIds(userId, workspaceId);
        if (existingMember != null) return new Response<MemberDto?>(
            success: false, 
            errors: ["User is already a member."]);

        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new Response<MemberDto?>(success: false, errors: ["Workspace not found."]);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new Response<MemberDto?>(success: false, errors: ["User not found."]);

        var member = new Member(user, workspace) {
            Workspace = workspace,
            User = user
        };

        await _memberRepository.CreateAsync(member);
        return new Response<MemberDto?>(success: true, content: new MemberDto(member));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return new Response<string>(success: false, errors: ["Member not found."]);

        await _memberRepository.DeleteAsync(member);
        return new Response<string>(success: true, content: "Member successfully deleted.");
    }

    public async Task<Response<MemberDto?>> GetByIdAsync(string id)
    {
        var res = await _memberRepository.GetByIdAsync(id);
        if (res == null) return new Response<MemberDto?>(success: false, errors: ["Member not found."]);

        return new Response<MemberDto?>(success: true, content: new MemberDto(res));
    }
}
