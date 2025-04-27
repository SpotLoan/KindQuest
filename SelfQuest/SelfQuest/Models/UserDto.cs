using SelfQuest.Models;

namespace KindQuestAPI.Models;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public List<string> Followers { get; set; } = new List<string>();
    public List<string> Following { get; set; } = new List<string>();
    public List<UserCreatureDto> Creatures { get; set; } = new List<UserCreatureDto>();
    public List<UserQuestDto> ActiveQuests { get; set; } = new List<UserQuestDto>();
    public UserStatsDto Stats { get; set; } = new UserStatsDto();
}