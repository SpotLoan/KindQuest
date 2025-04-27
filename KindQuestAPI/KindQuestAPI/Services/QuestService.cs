using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KindQuestAPI.Services
{
    public class QuestService
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuestRepository _questRepository;
        private readonly ICreatureRepository _creatureRepository;
        private readonly ILogger<QuestService> _logger;
        private readonly Random _random = new Random();

        public QuestService(
            IUserRepository userRepository,
            IQuestRepository questRepository,
            ICreatureRepository creatureRepository,
            ILogger<QuestService> logger)
        {
            _userRepository = userRepository;
            _questRepository = questRepository;
            _creatureRepository = creatureRepository;
            _logger = logger;
        }

        // Assign daily quests to a specific user
        public async Task<bool> AssignDailyQuestsToUserAsync(string userId)
        {
            try
            {
                // Get the user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User not found with ID: {userId}");
                    return false;
                }

                // Remove expired quests
                var activeQuests = user.ActiveQuests.Where(q => 
                    !DateTime.TryParse(q.ExpiresAt, out var expiryDate) || 
                    expiryDate > DateTime.UtcNow).ToList();
                
                // Clear completed quests that are older than a day
                var recentCompletedQuests = activeQuests.Where(q => 
                    !q.IsCompleted || 
                    (DateTime.TryParse(q.CompletedAt, out var completedDate) && 
                     completedDate.AddDays(1) > DateTime.UtcNow)).ToList();

                user.ActiveQuests = recentCompletedQuests;

                // Get all user's creatures
                if (user.Creatures.Count == 0)
                {
                    _logger.LogInformation($"User {userId} has no creatures to assign quests for");
                    return true; // Nothing to do
                }

                // For each creature, try to assign one quest if the user doesn't have too many active quests
                int maxDailyQuests = Math.Min(user.Creatures.Count, 5); // Limit to 5 daily quests max
                int currentActiveQuests = user.ActiveQuests.Count(q => !q.IsCompleted);
                int questsToAssign = Math.Max(0, maxDailyQuests - currentActiveQuests);

                if (questsToAssign <= 0)
                {
                    _logger.LogInformation($"User {userId} already has maximum number of active quests");
                    return true; // User already has max quests
                }

                // Get all active quests
                var allActiveQuests = await _questRepository.GetActiveQuestsAsync();
                if (allActiveQuests.Count == 0)
                {
                    _logger.LogWarning("No active quests available in the system");
                    return false;
                }

                // Shuffle creatures to randomly select which ones get new quests
                var shuffledCreatures = user.Creatures.OrderBy(x => _random.Next()).ToList();

                foreach (var creature in shuffledCreatures.Take(questsToAssign))
                {
                    // Get the full creature details to access quest pool
                    var fullCreature = await _creatureRepository.GetByIdAsync(creature.CreatureId);
                    if (fullCreature == null) continue;

                    // Find quests in this creature's quest pool
                    var eligibleQuests = allActiveQuests
                        .Where(q => fullCreature.QuestsPool.Contains(q.Id))
                        .Where(q => !user.ActiveQuests.Any(uq => uq.QuestId == q.Id)) // Don't duplicate quests
                        .ToList();

                    if (eligibleQuests.Count == 0) continue;

                    // Randomly select a quest
                    var selectedQuest = eligibleQuests[_random.Next(eligibleQuests.Count)];

                    // Create user quest
                    var userQuest = new UserQuest
                    {
                        QuestId = selectedQuest.Id,
                        CreatureId = creature.CreatureId,
                        Progress = 0,
                        MaxProgress = selectedQuest.RequirementValue,
                        IsCompleted = false,
                        AssignedAt = DateTime.UtcNow.ToString("o"),
                        ExpiresAt = DateTime.UtcNow.AddDays(1).ToString("o"),
                        CompletedAt = string.Empty
                    };

                    // Add to user
                    user.ActiveQuests.Add(userQuest);
                }

                // Update user
                var result = await _userRepository.UpdateAsync(userId, user);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning daily quests to user {userId}");
                return false;
            }
        }

        // Assign daily quests to all users
        public async Task<int> AssignDailyQuestsToAllUsersAsync()
        {
            int successCount = 0;
            var users = await _userRepository.GetAllAsync();

            foreach (var user in users)
            {
                var success = await AssignDailyQuestsToUserAsync(user.Id);
                if (success) successCount++;
            }

            return successCount;
        }

        // Update quest progress for a user
        public async Task<bool> UpdateQuestProgressAsync(string userId, string questId, int progress)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                var questIndex = user.ActiveQuests.FindIndex(q => q.QuestId == questId && !q.IsCompleted);
                if (questIndex == -1) return false;

                var quest = user.ActiveQuests[questIndex];
                quest.Progress = Math.Min(quest.MaxProgress, quest.Progress + progress);

                // Check if quest is completed
                if (quest.Progress >= quest.MaxProgress)
                {
                    quest.IsCompleted = true;
                    quest.CompletedAt = DateTime.UtcNow.ToString("o");

                    // Update user stats
                    user.Stats.TotalQuestsCompleted++;
                    
                    // Get the quest to determine reward
                    var questDetails = await _questRepository.GetByIdAsync(questId);
                    if (questDetails != null)
                    {
                        // Award XP to user
                        user.Stats.TotalXpEarned += questDetails.Rewards.Xp;
                        
                        // Award XP to the creature that issued the quest
                        var creatureIndex = user.Creatures.FindIndex(c => c.CreatureId == quest.CreatureId);
                        if (creatureIndex != -1)
                        {
                            user.Creatures[creatureIndex].Xp += questDetails.Rewards.Xp;
                            
                            // Check if creature should level up (simplified logic)
                            int currentLevel = user.Creatures[creatureIndex].Level;
                            int xpThreshold = currentLevel * 100; // Simple XP threshold formula
                            
                            if (user.Creatures[creatureIndex].Xp >= xpThreshold)
                            {
                                user.Creatures[creatureIndex].Level++;
                            }
                        }
                    }
                }

                user.ActiveQuests[questIndex] = quest;
                return await _userRepository.UpdateAsync(userId, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating quest progress for user {userId}, quest {questId}");
                return false;
            }
        }
    }
}