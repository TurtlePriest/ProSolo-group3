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
    public class ImagebankController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagebankController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Imagebanks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Imagebank.ToListAsync());
        }

        // GET: Imagebanks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagebank = await _context.Imagebank
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (imagebank == null)
            {
                return NotFound();
            }

            return View(imagebank);
        }

        // GET: Imagebanks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Imagebanks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoID,PhotoDescription")] Imagebank imagebank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imagebank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imagebank);
        }

        // GET: Imagebanks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagebank = await _context.Imagebank.FindAsync(id);
            if (imagebank == null)
            {
                return NotFound();
            }
            return View(imagebank);
        }

        // POST: Imagebanks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoID,PhotoDescription")] Imagebank imagebank)
        {
            if (id != imagebank.PhotoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagebank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagebankExists(imagebank.PhotoID))
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
            return View(imagebank);
        }

        // GET: Imagebanks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagebank = await _context.Imagebank
                .FirstOrDefaultAsync(m => m.PhotoID == id);
            if (imagebank == null)
            {
                return NotFound();
            }

            return View(imagebank);
        }

        // POST: Imagebanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagebank = await _context.Imagebank.FindAsync(id);
            _context.Imagebank.Remove(imagebank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagebankExists(int id)
        {
            return _context.Imagebank.Any(e => e.PhotoID == id);
        }
    }
}
