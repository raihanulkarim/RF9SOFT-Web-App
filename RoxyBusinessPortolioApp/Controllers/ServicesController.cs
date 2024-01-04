using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoxyBusinessPortolioApp.Data;
using RoxyBusinessPortolioApp.Models;

namespace RoxyBusinessPortolioApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServicesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
              return _context.Service != null ? 
                          View(await _context.Service.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Service'  is null.");
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Service == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ShortDesc,Image")] Service service, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                if(formFile != null)
                {
                    service.Image = UploadImage(formFile);
                }
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Service == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ShortDesc,Image")] Service service, IFormFile? formFile)
        {
            if (id != service.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProject = await _context.Service.FindAsync(id);
                    if(existingProject == null)
                    {
                        return NotFound();
                    }
                    // Handle image update or removal
                    if (formFile != null)
                    {
                        if (existingProject.Image != null)
                        {
                            var imgePath = $"Images/Services/{existingProject.Image}";
                            // Delete the existing image file
                            var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imgePath.TrimStart('/'));
                            if (System.IO.File.Exists(existingImagePath))
                            {
                                System.IO.File.Delete(existingImagePath);
                            }

                        }
                        // Upload the new image
                        existingProject.Image = UploadImage(formFile);
                    }
                    _context.Attach(existingProject).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ID))
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
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Service == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Service == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Service'  is null.");
            }
            var service = await _context.Service.FindAsync(id);
            if (service != null)
            {
                if (service.Image != null)
                {
                    var imgePath = $"Images/Services/{service.Image}";
                    // Delete the existing image file
                    var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imgePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }

                }
                _context.Service.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Service?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        private string UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Define a unique file name (you may use Guid.NewGuid() or other strategies)
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageFile.FileName)}";

                // Get the physical path to the wwwroot/Images folder
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Services/", fileName);

                // Save the file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Return the relative file path (you may save the absolute path if needed)
                return fileName;
            }

            return null; // No file was uploaded
        }
    }
}
