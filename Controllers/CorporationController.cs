using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Database;
using DesktopApplication.Models;

namespace DesktopApplication.Controllers
{
    public class CorporationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CorporationController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Corporations.ToListAsync());
        }

  
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporation = await _context.Corporations
                .FirstOrDefaultAsync(m => m.CorporationId == id);
            if (corporation == null)
            {
                return NotFound();
            }

            return View(corporation);
        }

 
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CorporationName,IsSync")] Corporation corporation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(corporation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(corporation);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporation = await _context.Corporations.FindAsync(id);
            if (corporation == null)
            {
                return NotFound();
            }
            return View(corporation);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CorporationId,CorporationName,IsSync")] Corporation corporation)
        {
            if (id != corporation.CorporationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(corporation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorporationExists(corporation.CorporationId))
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
            return View(corporation);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporation = await _context.Corporations
                .FirstOrDefaultAsync(m => m.CorporationId == id);
            if (corporation == null)
            {
                return NotFound();
            }

            return View(corporation);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var corporation = await _context.Corporations.FindAsync(id);
            if (corporation != null)
            {
                _context.Corporations.Remove(corporation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorporationExists(int id)
        {
            return _context.Corporations.Any(e => e.CorporationId == id);
        }
    }
}
