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
    [Authorize(Roles = "Admin")]
    public class AdminstrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ApplicationDbContext _context;

        public AdminstrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> Index(string role, string searchString)
        {
            // Use LINQ to get list of roles.
            IQueryable<string> RoleQuery = from r in _context.Users
                                           orderby r.RoleName
                                           select r.RoleName;

            var users = from u in _context.Users
                        select u;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FirstName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(x => x.RoleName == role);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adminstration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, UserName, Email, RoleName, PasswordHash")] ApplicationUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        user = new ApplicationUser
        //        {
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            RoleName = user.RoleName
        //        };
        //        var result = await userManager.CreateAsync(user, user.PasswordHash);
        //        userManager.AddToRoleAsync(user, user.RoleName).Wait();

        //        //_context.Add(user);

        //        //await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    RoleName = model.RoleName
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, user.RoleName).Wait();
                    return RedirectToAction(nameof(Index));
                }


                //_context.Add(user);

                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Adminstration/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Adminstration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName, UserName, Email")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Adminstration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var movie = await _context.Users.FindAsync(id);
            _context.Users.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

