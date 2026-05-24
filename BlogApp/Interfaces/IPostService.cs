using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int? categoryId);

        Task<Post?> GetPostDetailAsync(int id);

        Task<Post?> GetPostByIdAsync(int id);

        Task CreatePostAsync(Post post);

        Task UpdatePostAsync(Post post);

        Task DeletePostAsync(Post post);

        Task AddCommentAsync(Comment comment);
    }
}