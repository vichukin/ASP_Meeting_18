using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.AccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class AccountController : Controller
    {
        public UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignInManager { get; set; }
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Email = vm.Email,
                    UserName = vm.Login,
                    YearOfBirth = vm.YearOfBirth
                };
                var result = await UserManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(vm);
        }
        public IActionResult Login(string? returnURL = null)
        {
            LoginViewModel vm = new LoginViewModel() { ReturnUrl = returnURL };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(vm.Login, vm.Password, vm.IsPersistent, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Password or login are wrong!");

            }
            return View(vm);
        }
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }
        [AllowAnonymous]
        public IActionResult GoogleAuth()
        {
            string redirecturl = Url.Action("GoogleRedirect", "Account");
            var properties = SignInManager.ConfigureExternalAuthenticationProperties("Google", redirecturl);
            var chall = new ChallengeResult("Google", properties);
            return chall;

        }
        [AllowAnonymous]
        public IActionResult FbAuth()
        {
            string redirecturl = Url.Action("GoogleRedirect", "Account");
            var properties = SignInManager.ConfigureExternalAuthenticationProperties("Facebook", redirecturl);
            return new ChallengeResult("Facebook", properties);

        }
        public async Task<IActionResult> GoogleRedirect()
        {
            ExternalLoginInfo loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login");

            var res = await SignInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);

            string[] userinfo =
            {
                loginInfo.Principal.FindFirst(ClaimTypes.Name)?.Value,
                loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value
            };
            if (res.Succeeded)
            {
                return View(userinfo);

            }
            User user = new User { UserName = Transliteration.Front(userinfo[0]), Email = userinfo[1], };
            //if (userinfo[1].Contains("@"))
            //{
            //    user.UserName = userinfo[1].Substring(0, userinfo[1].IndexOf("@"));
            //}

            //var result = await UserManager.AddLoginAsync(user, loginInfo);
            //if (result.Succeeded)
            //{
            //    if (result.Succeeded)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false);

            //    }
            //    return View(userinfo);
            //}
            //else
            //{
            //    result = await UserManager.CreateAsync(user);
            //    if (result.Succeeded)
            //    {
            //        result = await UserManager.AddLoginAsync(user, loginInfo);
            //        if (result.Succeeded)
            //        {
            //            await SignInManager.SignInAsync(user, isPersistent: false);

            //        }
            //    }
            //}
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user, loginInfo);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return View(userinfo);
                }
            }
            else
            {
                User? findedUser =
                await UserManager.Users.FirstOrDefaultAsync(t => t.NormalizedEmail == user.Email!.ToUpper());
                if (findedUser != null)
                    await UserManager.AddLoginAsync(findedUser!, loginInfo);
            }
            //else if(result.Errors.FirstOrDefault(t=>t.Code== "DuplicateUserName")!=null)
            //{
            //    while(result.Errors.FirstOrDefault(t => t.Code == "DuplicateUserName") != null)
            //    {
            //        Random rnd = new Random();
            //        user.UserName += rnd.Next(1,1000000).ToString();
            //        result = await UserManager.CreateAsync(user);
            //        if (result.Succeeded)
            //        {
            //            result = await UserManager.AddLoginAsync(user, loginInfo);
            //            if (result.Succeeded)
            //            {
            //                await SignInManager.SignInAsync(user, isPersistent: false);
            //                return View(userinfo);
            //            }
            //        }
            //    }

            //}
            return RedirectToAction(nameof(AccessDenied));
        }

        public async Task<IActionResult> FacebookRedirect()
        {
            ExternalLoginInfo loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login");
            if (loginInfo.LoginProvider.Contains("@"))
                loginInfo.LoginProvider = loginInfo.LoginProvider.Substring(0, loginInfo.LoginProvider.IndexOf("@"));
            var res = await SignInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
            string[] userinfo =
            {
                loginInfo.Principal.FindFirst(ClaimTypes.Name)?.Value,
                loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value
            };
            if (res.Succeeded)
            {
                return View(userinfo);
            }
            User user = new User { UserName = userinfo[1], Email = userinfo[1] };
            //var result = await UserManager.AddLoginAsync(user, loginInfo);
            //if (result.Succeeded)
            //{
            //    if (result.Succeeded)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false);

            //    }
            //    return View(userinfo);
            //}
            //else
            //{
            //    result = await UserManager.CreateAsync(user);
            //    if (result.Succeeded)
            //    {
            //        result = await UserManager.AddLoginAsync(user, loginInfo);
            //        if (result.Succeeded)
            //        {
            //            await SignInManager.SignInAsync(user, isPersistent: false);

            //        }
            //    }
            //}
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user, loginInfo);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return View(userinfo);
                }
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
