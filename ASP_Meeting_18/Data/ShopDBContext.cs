using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASP_Meeting_18.Models.DTO.UserDTOs;

namespace ASP_Meeting_18.Data
{
    public class ShopDBContext : IdentityDbContext<User>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ShopDBContext(DbContextOptions<ShopDBContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ASP_Meeting_18.Models.DTO.UserDTOs.EditUserDTO> EditUserDTO { get; set; }
        public DbSet<ASP_Meeting_18.Models.DTO.UserDTOs.ChangePasswordDTO> ChangePasswordDTO { get; set; }
        public DbSet<ASP_Meeting_18.Models.DTO.UserDTOs.UserDTO> UserDTO { get; set; }
    }
}
