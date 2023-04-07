using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTO.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class ClaimsController : Controller
    {
        public UserManager<User> manager { get; set; }
        public ClaimsController(UserManager<User> manager)
        {
            this.manager = manager;
        }
        public IActionResult Index()
        {
            return View(User.Claims);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            User user = await manager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                Claim cl = new Claim(claimType, claimValue, ClaimValueTypes.String);
                var res = await manager.AddClaimAsync(user, cl);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Errors(res);
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string claimsInfo)
        {
            string[] info = claimsInfo.Split(';');
            User user = await manager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            IEnumerable<Claim> claims = await manager.GetClaimsAsync(user);
            Claim claimfordelete = claims.FirstOrDefault(t => t.Type.ToString() == info[0].ToString() && t.Value.ToString() == info[1].ToString() && t.ValueType.ToString() == info[2].ToString());
            await manager.RemoveClaimAsync(user, claimfordelete);
            return RedirectToAction("Index");
        }
        public void Errors(IdentityResult res)
        {
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        //[Authorize(Roles = "admin,manager")]
        [Authorize(Policy = "FrameworkPolicy")]
        public IActionResult testPolicy1()
        {
            return View("Index", User.Claims);
        }
    }
}
