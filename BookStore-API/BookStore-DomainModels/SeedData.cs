using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_DomainModels
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
             if(await userManager.FindByEmailAsync("admin@bookstore.com")== null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@bookstore.com"
                };
                var result= await userManager.CreateAsync(user, "3902185Neduet@");
                if (result.Succeeded)
                {
                   await userManager.AddToRoleAsync(user, "Administrator");
                }
            }

            //Creating Sample Customer Records
            if (await userManager.FindByEmailAsync("shahzaib@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Shahzaib",
                    Email = "shahzaib@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "3902185Neduet@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }
        private async  static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (! await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
