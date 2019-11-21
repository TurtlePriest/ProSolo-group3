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
                IdentityRole role = new IdentityRole
                {
                    Name = RolesNames.Admin.ToString()
                };

                // We could make a custom role class to add custom properties like the "ApplicationUser" have other attributes -MH
                //role.Description = "Perform all the operations.";

                await roleManager.CreateAsync(role);
            }

            // Create the Employee role
            if (!(await roleManager.RoleExistsAsync("Employee")))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = RolesNames.Employee.ToString()
                };
                await roleManager.CreateAsync(role);
            }

            // Create the Manufacturer role
            if (!(await roleManager.RoleExistsAsync("Manufacturer")))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = RolesNames.Manufacturer.ToString()
                };
                await roleManager.CreateAsync(role);
            }

            // Create the Customer role
            if (!(await roleManager.RoleExistsAsync("Customer")))
            {
                IdentityRole role = new IdentityRole
                {
                    Name = RolesNames.Customer.ToString()
                };
                await roleManager.CreateAsync(role);
            }

            // Maybe this should be deleted?
            // Create the TEST role
            if (!(await roleManager.RoleExistsAsync("TEST")))
            {
                var role = new IdentityRole
                {
                    Name = RolesNames.TEST.ToString()
                };
                await roleManager.CreateAsync(role);
            }

            // Maybe this should be deleted?
            // Create the TEST2 role
            if (!(await roleManager.RoleExistsAsync("TEST2")))
            {
                var role = new IdentityRole
                {
                    Name = RolesNames.TEST2.ToString()
                };
                await roleManager.CreateAsync(role);
            }
        }

        // Password for all seed users are set to 1 for ez logins.

        public async Task SeedUsers()
        {
            // Creates an Admin for the system
            if ((await userManager.FindByNameAsync("admin@admin.com")) == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "User"
                };


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
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Emp@email.com",
                    Email = "Emp@email.com",
                    FirstName = "Employee",
                    LastName = "User"
                };


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
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Man@email.com",
                    Email = "Man@email.com",
                    FirstName = "Manufacturer",
                    LastName = "User"
                };


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
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Cus@email.com",
                    Email = "Cus@email.com",
                    FirstName = "Customer",
                    LastName = "User"
                };

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
