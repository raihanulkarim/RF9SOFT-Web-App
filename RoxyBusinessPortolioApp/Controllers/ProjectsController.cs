using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoxyBusinessPortolioApp.Data;
using RoxyBusinessPortolioApp.Models;

namespace RoxyBusinessPortolioApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ProjectsController(IWebHostEnvironment webHostEnvironment,  ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: ProjectModels
        public async Task<IActionResult> Index()
        {
              return _context.ProjectModel != null ? 
                          View(await _context.ProjectModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ProjectModel'  is null.");
        }

        // GET: ProjectModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // GET: ProjectModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Sales,Price,Link,Image")] ProjectModel projectModel, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null) {
                    projectModel.Image = UploadImage(ImageFile);
                }
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: ProjectModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        // POST: ProjectModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Sales,Price,Link,Image")] ProjectModel updatedProject, IFormFile? ImageFile)
        {
            if (id != updatedProject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing project from the database
                    var existingProject = await _context.ProjectModel.FindAsync(id);

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

                    // Update the existing project with the new data
                    existingProject.Title = updatedProject.Title;
                    existingProject.Description = updatedProject.Description;
                    existingProject.Sales = updatedProject.Sales;
                    existingProject.Price = updatedProject.Price;
                    existingProject.Link = updatedProject.Link;

                    // Handle image update or removal
                    if (ImageFile != null)
                    {
                        if (existingProject.Image != null) {
                            var imgePath = $"Images/Projects/{existingProject.Image}";

                            // Delete the existing image file
                            var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imgePath.TrimStart('/'));
                            if (System.IO.File.Exists(existingImagePath))
                            {
                                System.IO.File.Delete(existingImagePath);
                            }
                        }

                        // Upload the new image
                        existingProject.Image = UploadImage(ImageFile);
                    }

                    // Attach the existing project to the context and mark it as modified
                    _context.Attach(existingProject).State = EntityState.Modified;

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(updatedProject.Id))
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

            return View(updatedProject);
        }


        // GET: ProjectModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectModel == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: ProjectModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProjectModel'  is null.");
            }
            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel != null)
            {
                if (projectModel.Image != null)
                {
                    var imgePath = $"Images/Projects/{projectModel.Image}";

                    // Delete the existing image file
                    var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imgePath.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }
                }
                _context.ProjectModel.Remove(projectModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
          return (_context.ProjectModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // Method to handle image upload
        private string UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Define a unique file name (you may use Guid.NewGuid() or other strategies)
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageFile.FileName)}";

                // Get the physical path to the wwwroot/Images folder
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Projects/", fileName);

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
