// using AutoMapper;
// using BachelorTherasoftDotnetApi.src.Dtos.Models;
// using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
// using BachelorTherasoftDotnetApi.src.Interfaces.Services;
// using BachelorTherasoftDotnetApi.src.Models;
// using Microsoft.AspNetCore.Identity;

// namespace BachelorTherasoftDotnetApi.src.Services;
 
// // TODO voir comment récupérer les user facilement car pas opti actuellement

// public class ConversationService : IConversationService
// {
//     private readonly IConversationRepository _conversationRepository;
//     private readonly UserManager<User> _userManager;
//     private readonly IMapper _mapper;
//     public ConversationService(IConversationRepository conversationRepository, UserManager<User> userManager, IMapper mapper)
//     {
//         _conversationRepository = conversationRepository;
//         _userManager = userManager;
//         _mapper = mapper;
//     }
//     public async Task<ConversationDto?> CreateAsync(List<string> userIds, string? name)
//     {
//         List<User> users = [];
//         foreach (string userId in userIds) 
//         {
//             var user = await _userManager.FindByIdAsync(userId);
//             if (user == null) return null;
//             users.Add(user);
//         }

//         var conversation = new Conversation(userIds, name);
//         await _conversationRepository.CreateAsync(conversation);

//         return _mapper.Map<ConversationDto>(conversation);
//     }

//     public Task<bool> DeleteAsync(string id)
//     {
//         throw new NotImplementedException();
//     }
    
//     public async Task<ConversationDto?> GetByIdAsync(string id)
//     {
//         var conversation = await _conversationRepository.GetByIdAsync<ConversationDto>(id);
//         if (conversation == null) return null;

//         return conversation;
//     }

//     public async Task<List<ConversationDto>?> GetByUserIdAsync(string userId)
//     {
//         var conversations = await _conversationRepository.GetByUserIdAsync(userId);

//         return  conversations.Count > 0 ? conversations.Select(c => _mapper.Map<ConversationDto>(c)).ToList() : null;
//     }

//     public Task<ConversationDto?> UpdateAsync(string id, string? newName, List<string>? newUsers)
//     {
//         throw new NotImplementedException();
//     }
// }
