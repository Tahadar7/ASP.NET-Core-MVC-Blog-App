using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlogApp.Models;

namespace BlogApp.ViewModels
{
public class PostDeleteViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
}
}