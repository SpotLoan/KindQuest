// SelfQuest/Services/IUserService.cs
using SelfQuest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using KindQuestAPI.Models;

namespace SelfQuest.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(string id, UserDto userDto);
        Task<bool> DeleteUserAsync(string id);

        Task<bool> AssignCreatureToUserAsync(string userId, UserCreatureDto creatureDto);
        Task<bool> AssignQuestToUserAsync(string userId, UserQuestDto questDto);
        Task<bool> CompleteUserQuestAsync(string userId, string questId);
    }
}