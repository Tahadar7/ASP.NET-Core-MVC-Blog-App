using BlogApp.Data;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Interfaces;
using BlogApp.Models;
using BlogApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Mvc;
namespace BlogApp.Controllers
{
    public class PostController(
        IPostService postService,
        IFileService fileService, 
        ICategoryService categoryService)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var posts = await postService.GetAllPostsAsync(categoryId);

            var categories = await categoryService.GetCategories();

            var viewModel = new PostIndexViewModel
            {
                Posts = posts,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await postService.GetPostDetailAsync(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            var viewModel = new PostDetailViewModel 
            {
                Post = post
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var postViewModel = new PostCreateViewModel
            {
                Categories = await categoryService.GetCategories()
            };

            return View(postViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PostCreateViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                postViewModel.Categories = await categoryService.GetCategories();

                return View(postViewModel);
            }

            if (postViewModel.FeatureImage == null)
            {
                ModelState.AddModelError("", "Feature image is required");

                postViewModel.Categories = await categoryService.GetCategories();

                return View(postViewModel);
            }

            postViewModel.Post.FeatureImagePath =
                await fileService.UploadFileAsync(postViewModel.FeatureImage);

            await postService.CreatePostAsync(postViewModel.Post);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postFromDb = await postService.GetPostByIdAsync(id.Value);

            if (postFromDb == null)
            {
                return NotFound();
            }

            var editViewModel = new PostEditViewModel
            {
                Post = postFromDb,
                Categories = await categoryService.GetCategories()
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(PostEditViewModel editViewModel)
        {
            if (!ModelState.IsValid)
            {
                editViewModel.Categories = await categoryService.GetCategories();

                return View(editViewModel);
            }

            var postFromDb =
                await postService.GetPostByIdAsync(editViewModel.Post.Id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            // map changes onto the already tracked postFromDb object

            postFromDb.Title = editViewModel.Post.Title;
            postFromDb.Content = editViewModel.Post.Content;
            postFromDb.Author = editViewModel.Post.Author;
            postFromDb.CategoryId = editViewModel.Post.CategoryId;

            // if user uploaded a new image, delete the old one and upload the new one

            if (editViewModel.FeatureImage != null)
            {
                fileService.DeleteFile(postFromDb.FeatureImagePath);

                postFromDb.FeatureImagePath = 
                    await fileService.UploadFileAsync(editViewModel.FeatureImage);
            }

            await postService.UpdatePostAsync(postFromDb);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var postFromDb = await postService.GetPostByIdAsync(id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            var viewModel = new PostDeleteViewModel
            {
                Id = postFromDb.Id,
                Title = postFromDb.Title
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var postFromDb = await postService.GetPostByIdAsync(id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            fileService.DeleteFile(postFromDb.FeatureImagePath);

            await postService.DeletePostAsync(postFromDb);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> AddComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return Json( new
                {
                    success = false
                });
            }

            await postService.AddCommentAsync(comment);

            return Json(new
            {
                username = comment.UserName,
                commentDate = comment.CommentDate.ToString("MMMM dd, yyyy"),
                content = comment.Content
            });
        }
    }
}