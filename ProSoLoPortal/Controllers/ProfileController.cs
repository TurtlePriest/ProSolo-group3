using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;

namespace ProSoLoPortal.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> UserManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager)
        {
            this.UserManager = UserManager;
            _context = context;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
            if (CurrentUser.RoleName.Equals("Customer") || CurrentUser.RoleName.Equals("Manufacturer"))
            {
                var profiles = from p in _context.Profile
                               where p.UserRefId == CurrentUser.Id
                               select p;
                ViewBag.FromProfile = "notnull";
                return View("Details", profiles.FirstOrDefault());
            }
            return View(await _context.Profile.ToListAsync());
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(int? id, int caseId)
        {
            if (id == null && caseId == 0)
            {
                return NotFound();
            }

            if (caseId == 0)
            {
                var profile = await _context.Profile
               .Include(p => p.Employee)
               .FirstOrDefaultAsync(m => m.ProfileId == id);
                if (profile == null)
                {
                    return NotFound();
                }
                return View(profile);
            }
            else
            {
                var cases = from c in _context.Case
                               select c;
                cases = cases.Where(s => s.CaseId.Equals(caseId));

                var profiles = from p in _context.Profile
                            select p;
                profiles = profiles.Where(s => s.UserRefId.Equals(cases.FirstOrDefault().CustomerId));
                if (profiles.FirstOrDefault() == null)
                {
                    return NotFound();
                }
                return View(profiles.FirstOrDefault());
            }
        }

        // GET: Profile/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Profile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string UserRefId, Profile profile)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
                profile.EmployeeId = CurrentUser.Id;
                var customer = await UserManager.FindByNameAsync(UserRefId);
                profile.UserRefId = customer.Id;
                profile.UserName = customer.UserName;
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", profile.EmployeeId);
            return View(profile);
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", profile.EmployeeId);
            return View(profile);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfileId,UserRefId,EmployeeId,Rating,ProfileText")] Profile profile)
        {
            if (id != profile.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var profiles = from p in _context.Profile
                                   where p.ProfileId == id
                                   select p;
                    Profile profile1 = profiles.FirstOrDefault();
                    profile1.ProfileText = profile.ProfileText;
                    _context.Update(profile1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "Id", profile.EmployeeId);
            return View(profile);
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profile.FindAsync(id);
            _context.Profile.Remove(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.ProfileId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveRating(int rating, int id, int caseId)
        {
            var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
            var profile = new Profile();
            if (CurrentUser.RoleName.Equals("Customer"))
            {
                profile = await _context.Profile.FindAsync(id);
            } else if (CurrentUser.RoleName.Equals("Manufacturer"))
            {
                var cases = from c in _context.Case
                            where c.CaseId == caseId
                            select c;
                string userId = cases.FirstOrDefault().CustomerId;

                var profiles = from p in _context.Profile
                               where p.UserRefId == userId
                               select p;
                Debug.WriteLine("id før " + id);
                id = profiles.FirstOrDefault().ProfileId;
                Debug.WriteLine("id efter " + id);
                profile = await _context.Profile.FindAsync(id);
            }
            Rating rating1 = new Rating { ProfileRefId = id, RatingNum = rating };
            _context.Rating.Add(rating1);
            await _context.SaveChangesAsync();
            var ratings = from r in _context.Rating
                       select r;
            ratings = ratings.Where(s => s.ProfileRefId.Equals(id));
            double counter = 0;
            double sum = 0;
            foreach(var r in ratings)
            {
                sum += r.RatingNum;
                counter++;
            }
            profile.Rating = Convert.ToDouble(sum / counter);
            _context.Profile.Update(profile);
            await _context.SaveChangesAsync();
            var bids = from b in _context.Bids
                          select b;
            bids = bids.Where(s => s.CaseRefId.Equals(caseId));
            if(CurrentUser.RoleName.Equals("Customer"))
            {
                bids.FirstOrDefault().RatedByCus = true;
            } else if (CurrentUser.RoleName.Equals("Manufacturer"))
            {
                bids.FirstOrDefault().RatedByMan = true;
            }
            _context.Bids.Update(bids.FirstOrDefault());
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Case");
        }
    }
}