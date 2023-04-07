using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTO.UserDTOs;
using ASP_Meeting_18.Models.ViewModels.RolesViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.Controllers.Admin
{
    [Authorize(Policy = "AdminAndManagerOnly")]
    public class RolesController : Controller
    {
        public UserManager<User> manager { get; set; }
        public RoleManager<IdentityRole> roleManager { get; set; }
        public IMapper mapper { get; set; }
        public RolesController(UserManager<User> manager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.manager = manager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var res = await roleManager.CreateAsync(new IdentityRole(role.Name));
                if (res.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var res = await roleManager.DeleteAsync(role);
            if (!res.Succeeded)
                return NotFound();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserList()
        {
            var users = mapper.Map<IEnumerable<UserDTO>>(await manager.Users.ToListAsync());
            return View(users);
        }
        public async Task<IActionResult> ChangeUserRoles(string? id)
        {
            if (id == null)
                return NotFound();
            var user = await manager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var UserRoles = await manager.GetRolesAsync(user);
            var allroles = await roleManager.Roles.ToListAsync();
            ChangeRolesViewModel vm = new ChangeRolesViewModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRoles = UserRoles,
                AllRoles = allroles

            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRoles(string? id, List<string> roles)
        {
            if (id == null) return NotFound();
            var user = await manager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userroles = await manager.GetRolesAsync(user);
            var allroles = await roleManager.Roles.Select(t => t.Name).ToListAsync();
            var addedroles = roles.Except(userroles);
            var deletedroles = userroles.Except(roles);
            await manager.AddToRolesAsync(user, addedroles);
            await manager.RemoveFromRolesAsync(user, deletedroles);
            return RedirectToAction("UserList","Roles");

        }
        //public async Task<IActionResult> GetChildCategories(string? parentid)
        //{
        //    var user
        //    SelectList sl = new
        //}
    }
}
