using BlogApp.Data;
using BlogApp.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class CategoryService(ApplicationDbContext context) : ICategoryService
    {

        public async Task<List<SelectListItem>> GetCategories()
        {
            return await context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }
                ).ToListAsync();
        }
    }
}