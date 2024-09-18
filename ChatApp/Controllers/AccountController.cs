using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApp.ViewModel;
using System.Net;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> User;
        private readonly SignInManager<ApplicationUser> sign;
        public AccountController(UserManager<ApplicationUser> user, SignInManager<ApplicationUser> sign)
        {
            User = user;
            this.sign = sign;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // بنعمل viewmodel لو عايزين نزود proprty وكمان عشان لو عايزين نزود validation مش هنعرف نحط validation فى built in model
        public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                // asyn => لازم await عشان تستنى لحد متخلص متروحش للى بعدها 
                // هى هنا مستنيه applicationuser (model)
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUserVM.UserName;
                user.PasswordHash = newUserVM.Password;

                // save in db
                IdentityResult result = await User.CreateAsync(user, newUserVM.Password); // عشان check على password وينفذ validation
                if (result.Succeeded)
                {
                
                    // create cookie
                    // sign.SignInAsync(user , false); // strore  ID , NAME , ROLE
            
                    await sign.SignInAsync(user, false);  // extra info // true => remember me => in login not registeration
                                                          // Cookie لتخزين اسم المستخدم
                    CookieOptions option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)// تحديد مدة الصلاحية بناءً على RememberMe
                    };
                    Response.Cookies.Append("Username", user.UserName, option);
                    return RedirectToAction("Index", "Home");

                }
                else // عشان ممكن يحصل مشاكل فى الباسورد او غيره
                {
                    foreach (var item in result.Errors) // icollections
                    {
                        ModelState.AddModelError("password", item.Description);
                    }
                }
            }
            return View(newUserVM);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM login)
        {
            if (ModelState.IsValid)
            {
                // check db
                // لازم نعمل check على خطوتين مره للاسم ومره للباسورد
                ApplicationUser usermodel = await User.FindByNameAsync(login.UserName);
                if (usermodel != null)
                {
                    bool found = await User.CheckPasswordAsync(usermodel, login.Password);
                    if (found)
                    {
                        // create cookie
                        await sign.SignInAsync(usermodel, login.RememberMe);

                        // Cookie لتخزين اسم المستخدم
                        CookieOptions option = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30)  // تحديد مدة الصلاحية بناءً على RememberMe
                        };
                        Response.Cookies.Append("Username", usermodel.UserName, option);
                        return RedirectToAction("Index", "Home");
                    }

                }
                // فى حالتين else بتوع two if
                ModelState.AddModelError("", "user name or password wrong");
            }
            return View(login);
        }
        public IActionResult Logout()
        {
            sign.SignOutAsync();
            return RedirectToAction("Register");
        }
       
    }
}
