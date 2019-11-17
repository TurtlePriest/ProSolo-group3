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
                role.Name = RolesNames.ROLE_ADMIN.ToString();

                // We could make a customer class to add custom properties like the "ApplicationUser" -MH

                //role.Description = "Perform all the operations.";

                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Employee role
            if (!(await roleManager.RoleExistsAsync("Employee")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.ROLE_EMPLOYEE.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Manufacturer role
            if (!(await roleManager.RoleExistsAsync("Manufacturer")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.ROLE_MANUFACTURER.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            // Create the Customer role
            if (!(await roleManager.RoleExistsAsync("Customer")))
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesNames.ROLE_CUSTOMER.ToString();
                IdentityResult roleResult = await roleManager.CreateAsync(role);
            }

            if (!(await roleManager.RoleExistsAsync("TEST")))
            {
                var role = new IdentityRole();
                role.Name = RolesNames.ROLE_TEST.ToString();
                await roleManager.CreateAsync(role);
            }

            if (!(await roleManager.RoleExistsAsync("TEST2")))
            {
                var role = new IdentityRole();
                role.Name = RolesNames.ROLE_TEST2.ToString();
                await roleManager.CreateAsync(role);
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

            // Creates a test Employee
            if ((await userManager.FindByNameAsync("Emp@email.com")) == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Emp@email.com";
                user.Email = "Emp@email.com";
                user.FirstName = "Employee";
                user.LastName = "User";


                IdentityResult result = await userManager.CreateAsync(user, "1");

                if (result.Succeeded)
                {
                    user.RoleName = "Employee";
                    await userManager.AddToRoleAsync(user, user.RoleName);
                }
            }

            // Creates a test Manufacturer
            if ((await userManager.FindByNameAsync("Man@email.com")) == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Man@email.com";
                user.Email = "Man@email.com";
                user.FirstName = "Manufacturer";
                user.LastName = "User";


                IdentityResult result = await userManager.CreateAsync(user, "1");

                if (result.Succeeded)
                {
                    user.RoleName = "Manufacturer";
                    await userManager.AddToRoleAsync(user, user.RoleName);
                }
            }

            // Creates a test Customer
            if ((await userManager.FindByNameAsync("Cus@email.com")) == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Cus@email.com";
                user.Email = "Cus@email.com";
                user.FirstName = "Customer";
                user.LastName = "User";

                IdentityResult result = await userManager.CreateAsync(user, "1");

                if (result.Succeeded)
                {
                    user.RoleName = "Customer";
                    await userManager.AddToRoleAsync(user, user.RoleName);
                }
            }
        }

    }
}
