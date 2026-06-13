using BlogApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Interfaces
{
    public interface ICategoryService
    {
        Task<List<SelectListItem>> GetCategories();
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}