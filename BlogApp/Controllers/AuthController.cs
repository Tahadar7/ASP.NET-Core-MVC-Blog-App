using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogApp.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Controllers
{
    public class AuthController(UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    SignInManager<IdentityUser> signInManager) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Create Identity User object 
                var user = new IdentityUser
                 { 
                    UserName = model.Email, 
                    Email = model.Email 
                };

                var result = await userManager.CreateAsync(user, model.Password);

                //If User Ceated Successfully
                if (result.Succeeded)
                {
                    //If the User Role exist in data base
                    if (!await roleManager.RoleExistsAsync("User"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, true);

                    return RedirectToAction("Index", "Post");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email not found");
                    return View(model);
                }

                var signInresult = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (!signInresult.Succeeded)
                {
                    ModelState.AddModelError("", "Email or Password is Incorrect");
                    return View(model);
                }

                return RedirectToAction("Index", "Post");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Post");
        }
    }
}





