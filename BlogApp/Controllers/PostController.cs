using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class PostController(
        IPostService postService,
        IFileService fileService,
        ApplicationDbContext context, 
        ICategoryService categoryService)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var posts = await postService.GetAllPostsAsync(categoryId);

            ViewBag.Categories = await categoryService.GetCategories();

            return View(posts);
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

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var postViewModel = new PostViewModel
            {
                Categories = await categoryService.GetCategories()
            };

            return View(postViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
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

            var editViewModel = new EditViewModel
            {
                Post = postFromDb,
                Categories = await categoryService.GetCategories()
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel editViewModel)
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

            if (editViewModel.FeatureImage != null)
            {
                fileService.DeleteFile(postFromDb.FeatureImagePath);

                editViewModel.Post.FeatureImagePath =
                    await fileService.UploadFileAsync(editViewModel.FeatureImage);
            }
            else
            {
                editViewModel.Post.FeatureImagePath =
                    postFromDb.FeatureImagePath;
            }

            await postService.UpdatePostAsync(editViewModel.Post);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var postFromDb = await postService.GetPostByIdAsync(id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            return View(postFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<JsonResult> AddComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
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