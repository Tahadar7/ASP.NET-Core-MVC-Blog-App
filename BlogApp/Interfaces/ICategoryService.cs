using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Interfaces
{
    public interface ICategoryService
    {
        Task<List<SelectListItem>> GetCategories();
    }
}