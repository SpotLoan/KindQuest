using KindQuestAPI.DAL.Interfaces;
using KindQuestAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KindQuestAPI.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _posts;

        public PostRepository(IMongoDatabase database)
        {
            _posts = database.GetCollection<Post>("Posts");
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _posts.Find(_ => true).ToListAsync();
        }

        public async Task<Post> GetByIdAsync(string id)
        {
            return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Post>> GetByUserIdAsync(string userId)
        {
            return await _posts.Find(post => post.UserId == userId).ToListAsync();
        }

        public async Task<List<Post>> GetByTagsAsync(List<string> tags)
        {
            // Find posts that have at least one matching tag
            var filter = Builders<Post>.Filter.AnyIn(p => p.Tags, tags);
            return await _posts.Find(filter).ToListAsync();
        }

        public async Task<bool> CreateAsync(Post post)
        {
            try
            {
                await _posts.InsertOneAsync(post);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, Post post)
        {
            try
            {
                var result = await _posts.ReplaceOneAsync(p => p.Id == id, post);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var result = await _posts.DeleteOneAsync(post => post.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddCommentAsync(string postId, PostComment comment)
        {
            try
            {
                var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
                var update = Builders<Post>.Update.Push(p => p.Comments, comment);

                var result = await _posts.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCommentAsync(string postId, string userId, string createdAt)
        {
            try
            {
                // We need to identify comments by userId and createdAt since they don't have their own IDs
                var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
                var update = Builders<Post>.Update.PullFilter(
                    p => p.Comments,
                    c => c.UserId == userId && c.CreatedAt == createdAt);

                var result = await _posts.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddLikeAsync(string postId, string userId)
        {
            try
            {
                var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
                var update = Builders<Post>.Update.AddToSet(p => p.Likes, userId);

                var result = await _posts.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveLikeAsync(string postId, string userId)
        {
            try
            {
                var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
                var update = Builders<Post>.Update.Pull(p => p.Likes, userId);

                var result = await _posts.UpdateOneAsync(filter, update);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}