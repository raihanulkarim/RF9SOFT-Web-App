using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoxyBusinessPortolioApp.Data;
using RoxyBusinessPortolioApp.Models;

public class PortfolioController : Controller
{
    private readonly ApplicationDbContext _context;

    public PortfolioController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Portfolio
    public async Task<IActionResult> Index()
    {
        var portfolios = await _context.Portfolios.Include(p => p.PorfolioSkills).ToListAsync();
        return View(portfolios);
    }

    // GET: Portfolio/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var portfolio = await _context.Portfolios
            .Include(p => p.PortfolioSkills)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (portfolio == null)
        {
            return NotFound();
        }

        return View(portfolio);
    }

    // GET: Portfolio/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ShortDesc,Image,Link,Skills")] Portfolio portfolio)
    {
        if (ModelState.IsValid)
        {
            // Save the project to the database
            _context.Add(portfolio);

            // Save skills to the database
            if (portfolio.PorfolioSkills != null && portfolio.PorfolioSkills.Any())
            {
                foreach (var skillName in portfolio.PorfolioSkills)
                {
                    var skillEntity = new PortfolioSkill { SkillName = skillName };
                    _context.Add(skillEntity);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(portfolio);
    }


    // GET: Portfolio/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var portfolio = await _context.Portfolios
            .Include(p => p.PortfolioSkills)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (portfolio == null)
        {
            return NotFound();
        }

        return View(portfolio);
    }

    // POST: Portfolio/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ShortDesc,Image,Link")] Portfolio portfolio, string[] skills)
    {
        if (id != portfolio.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingPortfolio = await _context.Portfolios
                    .Include(p => p.PortfolioSkills)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existingPortfolio == null)
                {
                    return NotFound();
                }

                existingPortfolio.Title = portfolio.Title;
                existingPortfolio.ShortDesc = portfolio.ShortDesc;
                existingPortfolio.Image = portfolio.Image;
                existingPortfolio.Link = portfolio.Link;

                // Update skills
                if (skills != null)
                {
                    existingPortfolio.PortfolioSkills = skills.Select(skill => new PortfolioSkill { SkillName = skill }).ToList();
                }

                _context.Update(existingPortfolio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortfolioExists(portfolio.Id))
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
        return View(portfolio);
    }

    // GET: Portfolio/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var portfolio = await _context.Portfolios
            .Include(p => p.PortfolioSkills)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (portfolio == null)
        {
            return NotFound();
        }

        return View(portfolio);
    }

    // POST: Portfolio/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);
        _context.Portfolios.Remove(portfolio);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PortfolioExists(int? id)
    {
        return _context.Portfolios.Any(e => e.Id == id);
    }
}
