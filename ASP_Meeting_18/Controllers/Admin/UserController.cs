using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTO.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.Controllers.Admin
{
    public class UserController : Controller
    {
        public UserManager<User> manager { get; set; }
        public IActionResult Index()
        {
            var users = manager.Users;
            IEnumerable<UserDTO> Usersdto = users.Select(t => new UserDTO { Id = t.Id, Login = t.UserName, Email = t.Email, YearOfBirth = t.YearOfBirth }).ToList();
            return View(Usersdto);
        }
        public UserController(UserManager<User> manager)
        {
            this.manager = manager;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO user)
        {
            if (ModelState.IsValid)
            {
                User User = new User()
                {
                    Email = user.Email,
                    UserName = user.Login,
                    YearOfBirth = user.YearOfBirth
                };
                var result = await manager.CreateAsync(User, user.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(user);

        }
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();
            var user = await manager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var userdto = new EditUserDTO() { Id = id, Email = user.Email, Login = user.UserName, YearOfBirth = user.YearOfBirth };
            return View(userdto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await manager.FindByIdAsync(dto.Id);
                if (user == null)
                    return NotFound();
                user.UserName = dto.Login;
                user.YearOfBirth = dto.YearOfBirth;
                user.Email = dto.Email;
                IdentityResult result = await manager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(dto);
        }
        public async Task<IActionResult> ChangePassword(string? id)
        {
            if (id == null) return NotFound();
            var user = await manager.FindByIdAsync(id);
            if (user == null) return NotFound();
            ChangePasswordDTO dto = new ChangePasswordDTO() { Email = user.Email, Id = id };
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await manager.FindByIdAsync(dto.Id);
                if (user == null) return NotFound();
                var passwordValidator = HttpContext.RequestServices.GetRequiredService<IPasswordValidator<User>>() as IPasswordValidator<User>;
                var passwordHasher = HttpContext.RequestServices.GetRequiredService<IPasswordHasher<User>>() as IPasswordHasher<User>;
                var IdentityResult = await passwordValidator.ValidateAsync(manager, user, dto.NewPassword);
                if (IdentityResult.Succeeded)
                {
                    string hashedpassword = passwordHasher.HashPassword(user, dto.NewPassword);
                    user.PasswordHash = hashedpassword;
                    await manager.UpdateAsync(user);
                    return RedirectToAction("index", "User");
                }
                else
                {
                    foreach (var item in IdentityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }

            }
            return View(dto);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                    return NotFound();
                var user = await manager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();
                var result = await manager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return RedirectToAction("Index");

        }
    }
}
