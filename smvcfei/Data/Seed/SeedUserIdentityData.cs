using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smvcfei.Models;

namespace smvcfei.Data.Seed
{
    public static class SeedIdentityUserData
    {
        public static void SeedUserIdentityData(this ModelBuilder modelBuilder)
        {
            // Agregar el rol "Administrador" a la tabla AspNetRoles
            string AdministradorGeneralRoleId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = AdministradorGeneralRoleId,
                Name = "Administrador",
                NormalizedName = "Administrador".ToUpper()
            });

            // Agregar el rol "Administrador" a la tabla AspNetRoles
            string UsuarioGeneralRoleId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = UsuarioGeneralRoleId,
                Name = "Usuario general",
                NormalizedName = "Usuario general".ToUpper()
            });

            // Agregamos un usuario a la tabla AspNetUsers
            var UsuarioId = Guid.NewGuid().ToString();
            modelBuilder.Entity<CustomIdentityUser>().HasData(
                new CustomIdentityUser
                {
                    Id = UsuarioId, // primary key
                    UserName = "gvera@uv.mx",
                    Email = "gvera@uv.mx",
                    NormalizedEmail = "gvera@uv.mx".ToUpper(),
                    Nombrecompleto = "Guillermo Humberto Vera Amaro",
                    NormalizedUserName = "gvera@uv.mx".ToUpper(),
                    PasswordHash = new PasswordHasher<CustomIdentityUser>().HashPassword(null, "patito")
                }
            );

            // Aplicamos la relación entre el usuario y el rol en la tabla AspNetUserRoles
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = AdministradorGeneralRoleId,
                    UserId = UsuarioId
                }
            );
        }
    }
}
