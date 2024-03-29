﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;
using ProSoLoPortal.ViewModels.CaseViewModels;

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
        public async Task<IActionResult> Index(int caseId, int bidId, bool locked, bool showFinished)
        {
            var cases = from c in _context.Case
                        select c;
            // pick this manufacturer
            if (caseId != 0 && bidId != 0)
            {
                cases = cases.Where(s => s.CaseId.Equals(caseId));

                var bids = from b in _context.Bids
                           select b;
                bids = bids.Where(s => s.BidId.Equals(bidId));
                var @case = cases.FirstOrDefault();
                @case.IsLocked = true;
                var @bid = bids.FirstOrDefault();
                @case.ManufacturerId = @bid.UserRefId;
                _context.Case.Update(@case);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);

            if (CurrentUser.RoleName.Equals("Customer") && !locked)
            {
                cases = cases.Where(s => s.CustomerId.Equals(CurrentUser.Id) && !s.IsFinished && !s.IsLocked);
                return View(cases);
            }
            else if (CurrentUser.RoleName.Equals("Customer") && locked)
            {
                cases = cases.Where(s => s.CustomerId.Equals(CurrentUser.Id) && s.IsLocked && !s.IsFinished);
                return View(cases);
            }
            if (showFinished && CurrentUser.RoleName.Equals("Employee"))
            {
                cases = cases.Where(s => s.EmployeeId.Equals(CurrentUser.Id) && s.IsFinished && s.IsLocked);
                return View(cases);
            }
            if (CurrentUser.RoleName.Equals("Employee") && !locked)
            {
                cases = cases.Where(s => s.EmployeeId.Equals(CurrentUser.Id) && !s.IsFinished && !s.IsLocked);
                return View(cases);
            } else if (CurrentUser.RoleName.Equals("Employee") && locked)
            {
                cases = cases.Where(s => s.EmployeeId.Equals(CurrentUser.Id) && s.IsLocked && !s.IsFinished);
                return View(cases);
            }
            if (CurrentUser.RoleName.Equals("Manufacturer") && !locked)
            {
                var bids = from b in _context.Bids
                           where b.UserRefId == CurrentUser.Id
                           select b;
                cases = cases.Where(s => s.IsLocked.Equals(false) && s.IsFinished.Equals(false));
                foreach (Bids b in bids)
                {
                    cases = cases.Where(s => !s.CaseId.Equals(b.CaseRefId));
                }
                return View(cases);
            } else if (CurrentUser.RoleName.Equals("Manufacturer") && locked)
            {
                cases = cases.Where(s => s.IsLocked.Equals(true) && s.IsFinished.Equals(false) && s.ManufacturerId.Equals(CurrentUser.Id));
                return View(cases);
            }
            return View(await _context.Case.ToListAsync());
        }
        [Authorize(Roles = "Admin, Employee, Customer, Manufacturer")]
        // GET: Case/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //byte[] imageByteData = System.IO.File.ReadAllBytes("C:/Users/Andreas Ibsen Cor/Pictures/zru2C3e0_400x400.jpg");
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

            byte[] imageByteData = @case.ImagePath;
            string imageBase64Data = Convert.ToBase64String(imageByteData);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageData = imageDataURL;

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
        public async Task<IActionResult> Create(string CustomerId, IList<IFormFile> files, Case @case)
        {
            if (ModelState.IsValid)
            {
                //@case.ImagePath = System.IO.File.ReadAllBytes(file.ToString());
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var reader = new MemoryStream())
                        {
                            file.CopyTo(reader);
                            var fileBytes = reader.ToArray();
                            @case.ImagePath = fileBytes;
                        }
                    }
                }

                var CurrentUser = await UserManager.GetUserAsync(HttpContext.User);
                @case.EmployeeId = CurrentUser.Id;
                var customer = await UserManager.FindByNameAsync(CustomerId);
                @case.CustomerId = customer.Id;
                var profiles = from p in _context.Profile
                               select p;
                profiles = profiles.Where(s => s.UserRefId.Equals(customer.Id));
                @case.ProfileRefId = profiles.FirstOrDefault().ProfileId;
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
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
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
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int caseId)
        {
            var @case = await _context.Case.FindAsync(caseId);
            @case.IsFinished = true;
            _context.Update(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}