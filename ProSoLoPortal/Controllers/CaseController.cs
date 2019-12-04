using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;
using ProSoLoPortal.ViewModels.CaseViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace ProSoLoPortal.Controllers
{
    public class CaseController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public CaseController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> SignInManager)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee, Customer, Manufacturer")]
        // GET: Case
        public async Task<IActionResult> Index()
        {
            var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);

            if (CurrentUser.RoleName.Equals("Customer"))
            {
                var cases = from c in _context.Case
                            select c;
                cases = cases.Where(s => s.CustomerId.Equals(CurrentUser.Id));
                return View(cases);
            }
            if (CurrentUser.RoleName.Equals("Employee"))
            {
                var cases = from c in _context.Case
                            select c;
                cases = cases.Where(s => s.EmployeeId.Equals(CurrentUser.Id));
                return View(cases);
            }
            if (CurrentUser.RoleName.Equals("Manufacturer"))
            {
                var cases = from c in _context.Case
                            select c;
                var bids = from b in _context.Bids
                           select b;
                bids = bids.Where(s => s.UserRefId.Equals(CurrentUser.Id));
                cases = cases.Where(s => s.IsLocked.Equals(false) && s.IsFinished.Equals(false));
                foreach (Bids b in bids)
                {
                    cases = cases.Where(s => !s.CaseId.Equals(b.CaseRefId));
                }
                return View(cases);
            }
            return View(await _context.Case.ToListAsync());
        }
        [Authorize(Roles = "Admin, Employee, Customer, Manufacturer")]
        // GET: Case/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Case/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Case/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string CustomerId, [Bind("CaseId,Name,TimeFrame,NumberOfProducts,Seller,ProposedPrice,IsLocked,IsFinished,TimeFrameFexible")] Case @case)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
                @case.EmployeeId = CurrentUser.Id;
                var customer = await UserManager.FindByNameAsync(CustomerId);
                @case.CustomerId = customer.Id;
                _context.Add(@case);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@case);
        }
        [Authorize(Roles = "Admin, Manufacturer")]
        public async Task<IActionResult> Bid(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            return View();
        }
        [Authorize(Roles = "Admin, Manufacturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bid(int id, [Bind("CaseId")] Case @case, BidViewModel model)
        {
            if (id != @case.CaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
                    ViewBag.Name = "Bad";

                    Bids bid = new Bids
                    {
                        ProposedTimeFrame = model.ProposedTimeFrame,
                        BidPrice = model.BidPrice,
                        CaseRefId = @case.CaseId,
                        UserRefId = CurrentUser.Id,
                        CaseName = @case.Name
                    };
                    _context.Bids.Update(bid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseId))
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
            return View(model);
        }

        [Authorize(Roles = "Admin, Employee")]

        // GET: Case/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            return View(@case);
        }

        // POST: Case/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseId,CustomerId,EmployeeId,Name,TimeFrame,NumberOfProducts,Seller,ProposedPrice,UserRefId,IsLocked,IsFinished,TimeFrameFexible")] Case @case)
        {
            if (id != @case.CaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseId))
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
            return View(@case);
        }

        [Authorize(Roles = "Admin, Employee")]

        // GET: Case/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // POST: Case/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @case = await _context.Case.FindAsync(id);
            _context.Case.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
            return _context.Case.Any(e => e.CaseId == id);
        }
    }
}