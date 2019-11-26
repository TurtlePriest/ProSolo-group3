using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Data;
using ProSoLoPortal.Models;

namespace ProSoLoPortal.Controllers
{
    public class BidsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BidsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bids
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bids.Include(b => b.Case);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId");
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BidId,CaseRefId,UserRefId,ProposedTimeFrame,BidPrice")] Bids bids)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bids);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseRefId"] = new SelectList(_context.Case, "CaseId", "CaseId", bids.CaseRefId);
            return View(bids);
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
