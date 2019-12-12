using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProSoLoPortal.ViewModels;
using ProSoLoPortal.ViewModels.AdministrationViewModels;

namespace ProSoLoPortal.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ApplicationDbContext _context;

        public AdministrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;

            _context = context;

        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Adminstration
        [HttpGet]
        public async Task<IActionResult> Index(string Role, string SearchString)
        {
            // Use LINQ to get list of roles.
            IQueryable<string> RoleQuery = from r in _context.Users
                                           orderby r.RoleName
                                           select r.RoleName;

            var users = from u in _context.Users
                        select u;



            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(s => s.FirstName.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(Role))
            {
                users = users.Where(x => x.RoleName == Role);
            }
            
            var userVM = new UserViewModel
            {
                RoleNameList = new SelectList(await RoleQuery.Distinct().ToListAsync()),
                Users = await users.ToListAsync()
            };

            return View(userVM);
        }

        // GET: Adminstration/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Adminstration/Create
        public IActionResult Create(bool isAdmin)
        {
            ViewBag.isAdmin = isAdmin;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model, string ProfileText, bool isAdmin)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                if (isAdmin)
                {
                    user = new ApplicationUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        RoleName = Enum.GetName(typeof(ProSoLoPortal.Helpers.RolesNames), Int32.Parse(model.RoleName))
                    };
                }
                else
                {
                    user = new ApplicationUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        RoleName = model.RoleName
                    };
                }
                var result = await userManager.CreateAsync(user, model.Password);
                var CurrentUser = await userManager.GetUserAsync(HttpContext.User);
                if(user.RoleName.Equals("Customer") || user.RoleName.Equals("Manufacturer"))
                {
                    var profile = new Profile
                    {
                        UserName = model.Email,
                        UserRefId = user.Id,
                        Rating = 0,
                        ProfileText = ProfileText,
                        EmployeeId = CurrentUser.Id
                    };
                    _context.Profile.Add(profile);
                }
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, user.RoleName).Wait();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Adminstration/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            // Find the user with that Id
            var user = await _context.Users.FindAsync(id);
            EditUserViewModel editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                RoleName = user.RoleName
            };
            return View(editUserViewModel);
        }

        // POST: Adminstration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _context.Users.FindAsync(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Adminstration/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Adminstration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            var profiles = from p in _context.Profile
                           where p.UserRefId == id
                           select p;
            _context.Profile.Remove(profiles.FirstOrDefault());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

