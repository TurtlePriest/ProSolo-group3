using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET: Imagebank
        public async Task<IActionResult> Index()
        {
            //var images = from i in _context.Imagebank
            //             select i;
            //string[] ImageStrings = new string[images.Count()];
            //int counter = 0;
            //foreach (var i in images)
            //{
            //    byte[] imageByteData = i.ImageByte;
            //    string imageBase64Data = Convert.ToBase64String(imageByteData);
            //    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            //    ImageStrings[counter] = imageDataURL;
            //    counter++;
            //}

            //ViewBag.ImageData = ImageStrings;
            return View(await _context.Imagebank.ToListAsync());
        }
        public String ConvertByteArrayToBase64(int id)
        {
            var images = from i in _context.Imagebank
                         select i;
            images = images.Where(s => s.PhotoID.Equals(id));
            byte[] imageByteData = images.FirstOrDefault().ImageByte;
            string imageBase64Data = Convert.ToBase64String(imageByteData);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            return (imageDataURL);
        }

        // GET: Imagebank/Details/5
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

        // GET: Imagebank/Create
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Imagebank/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IList<IFormFile> files, Imagebank @imagebank)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var reader = new MemoryStream())
                        {
                            //file.CopyTo(reader);
                            //var fileBytes = reader.ToArray();
                            byte[] fileBytes = { 0, 0, 0 };
                            @imagebank.ImageByte = fileBytes;
                        }
                    }
                }
                _context.Add(@imagebank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@imagebank);
        }

        // GET: Imagebank/Edit/5
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

        // POST: Imagebank/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoID,PhotoDescription,ImageByte")] Imagebank imagebank)
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

        // GET: Imagebank/Delete/5
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

        // POST: Imagebank/Delete/5
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