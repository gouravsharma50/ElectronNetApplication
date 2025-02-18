using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Database;
using DesktopApplication.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesktopApplication.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Branch)
                .Include(c => c.Corporation)
                .Include(c => c.User)
                .Select(c => new CategoryModel
                {
                    CategoryId = c.CategoryId,
                    CorporationId = c.CorporationId,
                    BranchId = c.BranchId,
                    CreatedByUserId = c.CreatedByUserId,
                    CategoryName = c.CategoryName,
                    CreatedDate = c.CreatedDate,
                    IsSync = c.IsSync,
                    Corporation = c.Corporation,
                    Branch = c.Branch,
                    User = c.User
                })
                .ToListAsync();

            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Branch)
                .Include(c => c.Corporation)
                .Include(c => c.User)
                .Where(m => m.CategoryId == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryModel
            {
                CategoryId = category.CategoryId,
                CorporationId = category.CorporationId,
                BranchId = category.BranchId,
                CreatedByUserId = category.CreatedByUserId,
                CategoryName = category.CategoryName,
                CreatedDate = category.CreatedDate,
                IsSync = category.IsSync,
                Corporation = category.Corporation,
                Branch = category.Branch,
                User = category.User
            };

            return View(categoryModel);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName");
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "UserId", "Username");
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CorporationId,BranchId,CreatedByUserId,CategoryName,IsSync")] CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CorporationId = categoryModel.CorporationId,
                    BranchId = categoryModel.BranchId,
                    CreatedByUserId = categoryModel.CreatedByUserId,
                    CategoryName = categoryModel.CategoryName,
                    CreatedDate = DateTime.UtcNow, // Default value for CreatedDate
                    IsSync = categoryModel.IsSync // Default value for IsSync
                };

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", categoryModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", categoryModel.CorporationId);
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "UserId", "Role", categoryModel.CreatedByUserId);
            return View(categoryModel);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryModel
            {
                CategoryId = category.CategoryId,
                CorporationId = category.CorporationId,
                BranchId = category.BranchId,
                CreatedByUserId = category.CreatedByUserId,
                CategoryName = category.CategoryName,
                CreatedDate = category.CreatedDate,
                IsSync = category.IsSync
            };

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", categoryModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", categoryModel.CorporationId);
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "UserId", "Role", categoryModel.CreatedByUserId);
            return View(categoryModel);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CorporationId,BranchId,CreatedByUserId,CategoryName,CreatedDate,IsSync")] CategoryModel categoryModel)
        {
            if (id != categoryModel.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _context.Categories.FindAsync(id);
                    if (category != null)
                    {
                        category.CorporationId = categoryModel.CorporationId;
                        category.BranchId = categoryModel.BranchId;
                        category.CreatedByUserId = categoryModel.CreatedByUserId;
                        category.CategoryName = categoryModel.CategoryName;
                        category.CreatedDate = categoryModel.CreatedDate; // Use existing CreatedDate if needed
                        category.IsSync = categoryModel.IsSync;

                        _context.Update(category);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryModel.CategoryId))
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

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", categoryModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", categoryModel.CorporationId);
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "UserId", "Role", categoryModel.CreatedByUserId);
            return View(categoryModel);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Branch)
                .Include(c => c.Corporation)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryModel = new CategoryModel
            {
                CategoryId = category.CategoryId,
                CorporationId = category.CorporationId,
                BranchId = category.BranchId,
                CreatedByUserId = category.CreatedByUserId,
                CategoryName = category.CategoryName,
                CreatedDate = category.CreatedDate,
                IsSync = category.IsSync
            };

            return View(categoryModel);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
