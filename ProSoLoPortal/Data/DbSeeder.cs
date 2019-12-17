using Microsoft.AspNetCore.Identity;
using ProSoLoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProSoLoPortal.Helpers;

namespace ProSoLoPortal.Data
{
    public class DbSeeder
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;

        public DbSeeder(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        public async Task SeedData()
        {
            await SeedRolesAsync();
            await SeedUsers();
        }

        public async Task SeedRolesAsync()
        {
            // Create the Admin role
            if (!(await roleManager.RoleExistsAsync("Admin")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.Admin.ToString();

                // We could make a customer class to add custom properties like the "ApplicationUser" -MH

                //role.Description = "Perform all the operations.";

                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Employee role
            if (!(await roleManager.RoleExistsAsync("Employee")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.Employee.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Manufacturer role
            if (!(await roleManager.RoleExistsAsync("Manufacturer")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.Manufacturer.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Customer role
            if (!(await roleManager.RoleExistsAsync("Customer")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.Customer.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }
        }

        // Password for all seed users are set to 1 for ez logins.

        public async Task SeedUsers()
        {
            // Creates an Admin for the system
            if ((await userManager.FindByNameAsync("admin@admin.com")) == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                user.FirstName = "Admin";
                user.LastName = "User";


                IdentityResult result = await userManager.CreateAsync(user, "1");

                if (result.Succeeded)
                {
                    user.RoleName = "Admin";
                    await userManager.AddToRoleAsync(user, user.RoleName);
                }
            }
        }

    }
}
