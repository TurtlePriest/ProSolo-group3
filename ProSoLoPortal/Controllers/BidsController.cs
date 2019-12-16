using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;

namespace ProSoLoPortal.Controllers
{
    [Authorize(Roles = "Admin, Manufacturer, Customer, Employee")]
    public class BidsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> UserManager;

        public BidsController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager)
        {
            this.UserManager = UserManager;
            _context = context;
        }

        // GET: Bids
        public async Task<IActionResult> Index(int? id, bool rateClick)
        {
            ViewBag.rateClick = rateClick;
            var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
            if (CurrentUser.RoleName.Equals("Customer") && rateClick)
            {
                var bids = from b in _context.Bids
                           where b.RatedByCus == false
                           select b;
                bids = bids.Where(s => s.Case.CustomerId.Equals(CurrentUser.Id) && s.Case.IsFinished && s.Case.IsLocked);
                return View(bids);
            }
            if (CurrentUser.RoleName.Equals("Customer") || CurrentUser.RoleName.Equals("Employee") && !rateClick)
            {
                var bids = from b in _context.Bids
                           select b;
                bids = bids.Where(s => s.CaseRefId.Equals(id));
                return View(bids);
            }
            if (CurrentUser.RoleName.Equals("Manufacturer") && !rateClick)
            {
                var bids = from b in _context.Bids
                           select b;
                bids = bids.Where(s => s.UserRefId.Equals(CurrentUser.Id) && !s.Case.IsLocked && !s.Case.IsFinished);
                return View(bids);
            }
            if (CurrentUser.RoleName.Equals("Manufacturer") && rateClick)
            {
                var bids = from b in _context.Bids
                           select b;
                var cases = from c in _context.Case
                            select c;
                bids = bids.Where(s => s.UserRefId.Equals(CurrentUser.Id) && s.Case.IsFinished && !s.RatedByMan);
                foreach (Bids b in bids)
                {
                    cases = cases.Where(s => s.CaseId.Equals(b.CaseRefId));
                }
                return View(bids);
            }
            return View();
        }

        // GET: Bids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bids = await _context.Bids
                .Include(b => b.Case)
                .FirstOrDefaultAsync(m => m.BidId == id);
            if (bids == null)
            {
                return NotFound();
            }

            return View(bids);
        }

        // GET: Bids/Create
        public IActionResult Create(bool isFlexible)
        {
            ViewBag.Flexible = isFlexible;
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId");
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, bool isFlexible, [Bind("BidId,ProposedTimeFrame,BidPrice")] Bids bids)
        {
            if (ModelState.IsValid)
            {
                var case1 = await _context.Case.FindAsync(id);
                if (!isFlexible) {
                    bids.ProposedTimeFrame = case1.TimeFrame;
                }
                var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
                bids.UserRefId = CurrentUser.Id;
                var profiles = from p in _context.Profile
                              select p;
                profiles = profiles.Where(s => s.UserRefId.Equals(CurrentUser.Id));
                bids.ProfileRefId = profiles.FirstOrDefault().ProfileId;
                bids.CaseRefId = id;
                bids.CaseName = case1.Name;
                _context.Add(bids);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "Case");
            }
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId", bids.CaseRefId);
            return RedirectToAction("index", "Case");
        }

        // GET: Bids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bids = await _context.Bids.FindAsync(id);
            if (bids == null)
            {
                return NotFound();
            }
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId", bids.CaseRefId);
            return View(bids);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BidId,CaseRefId,UserRefId,ProposedTimeFrame,BidPrice")] Bids bids)
        {
            if (id != bids.BidId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bids);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidsExists(bids.BidId))
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
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId", bids.CaseRefId);
            return View(bids);
        }

        // GET: Bids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bids = await _context.Bids
                .Include(b => b.Case)
                .FirstOrDefaultAsync(m => m.BidId == id);
            if (bids == null)
            {
                return NotFound();
            }

            return View(bids);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bids = await _context.Bids.FindAsync(id);
            _context.Bids.Remove(bids);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidsExists(int id)
        {
            return _context.Bids.Any(e => e.BidId == id);
        }
    }
}