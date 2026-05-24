using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class PostService(ApplicationDbContext context) : IPostService
    {
        public async Task<List<Post>> GetAllPostsAsync(int? categoryId)
        {
            var query = context.Posts.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }

        public async Task<Post?> GetPostDetailAsync(int id)
        {
            return await context.Posts.Include(p => p.Category).Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await context.Posts
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePostAsync(Post post)
        {
            await context.Posts.AddAsync(post);

            await context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            context.Posts.Update(post);

            await context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            context.Posts.Remove(post);

            await context.SaveChangesAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            comment.CommentDate = DateTime.Now;

            await context.Comments.AddAsync(comment);

            await context.SaveChangesAsync();
        }
    }
}