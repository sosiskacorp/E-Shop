using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using E_Shop.Constants;

namespace ClothingStore.Data
{
    public class StoreDbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            // Добавление ролей в базу данных
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Создание пользователя администратора

            var admin = new IdentityUser
            {
                UserName = "storeadmin@example.com",
                Email = "storeadmin@example.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "StoreAdmin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
