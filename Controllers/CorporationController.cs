using DesktopApplication.Database;
using DesktopApplication.Models;
using DesktopApplication.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class CorporationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly BusinessService _businessService;
    public CorporationController(ApplicationDbContext context, BusinessService businessService)
    {
        _context = context;
        _businessService = businessService;
    }

    public async Task<IActionResult> Index()
    {
        var corporations = await _context.Corporations.ToListAsync();

        var viewModel = corporations.Select(c => new CorporationModel
        {
            CorporationId = c.CorporationId,
            CorporationName = c.CorporationName,
            CorporationCreatedOn = c.CorporationCreatedOn
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var corporation = await _context.Corporations.FindAsync(id);
        if (corporation == null) return NotFound();

        var viewModel = new CorporationModel
        {
            CorporationId = corporation.CorporationId,
            CorporationName = corporation.CorporationName,
            CorporationCreatedOn = corporation.CorporationCreatedOn
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CorporationName")] CorporationModel corporationModel)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Username = corporationModel.CorporationName,
                Password = "123456",
                Role = "CORPORATION"
            };

            var createdUser = _businessService.CreateUser(user);
            var corporation = new Corporation
            {
                CorporationName = corporationModel.CorporationName,
                IsSync = false
            };

            _context.Add(corporation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(corporationModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var corporation = await _context.Corporations.FindAsync(id);
        if (corporation == null) return NotFound();

        var viewModel = new CorporationModel
        {
            CorporationId = corporation.CorporationId,
            CorporationName = corporation.CorporationName,
            CorporationCreatedOn = corporation.CorporationCreatedOn
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CorporationId,CorporationName")] CorporationModel corporationModel)
    {
        if (id != corporationModel.CorporationId) return NotFound();

        if (ModelState.IsValid)
        {
            var corporation = await _context.Corporations.FindAsync(id);
            if (corporation == null) return NotFound();

            corporation.CorporationName = corporationModel.CorporationName;
            corporation.IsSync = false;

            _context.Update(corporation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(corporationModel);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var corporation = await _context.Corporations.FindAsync(id);
        if (corporation == null) return NotFound();

        var viewModel = new CorporationModel
        {
            CorporationId = corporation.CorporationId,
            CorporationName = corporation.CorporationName,
            CorporationCreatedOn = corporation.CorporationCreatedOn
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var corporation = await _context.Corporations.FindAsync(id);
        if (corporation != null)
        {
            _context.Corporations.Remove(corporation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
