using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Database;
using DesktopApplication.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace DesktopApplication.Controllers
{
    [Authorize(Roles = "ADMIN, BRANCH, USER")]
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
                    User = c.User,
                    ParentCategoryName = c.ParentCategoryId != null
                                        ? _context.Categories
                                            .Where(p => p.CategoryId == c.ParentCategoryId)
                                            .Select(p => p.CategoryName)
                                            .FirstOrDefault()
                                        : null
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
                User = category.User,
                ParentCategoryName = category.ParentCategoryId != null
                                        ? _context.Categories
                                            .Where(p => p.CategoryId == category.ParentCategoryId)
                                            .Select(p => p.CategoryName)
                                            .FirstOrDefault()
                                        : null
            };

            return View(categoryModel);
        }

        public IActionResult Create()
        {
            var currentUserName = User.Identity.Name;

            var currentUser = _context.Users
                .Include(u => u.Corporation)
                .Include(u => u.Branch)
                .FirstOrDefault(u => u.Username == currentUserName);

            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            // Populate Parent Category dropdown
            ViewBag.ParentCategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            var currentUserName = User.Identity.Name;
            var currentUser = _context.Users
                .Include(u => u.Corporation)
                .Include(u => u.Branch)
                .FirstOrDefault(u => u.Username == currentUserName);

            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CorporationId = (int)currentUser.CorporationId,
                    BranchId = (int)currentUser.BranchId,
                    CreatedByUserId = currentUser.UserId,
                    CategoryName = model.CategoryName,
                    ParentCategoryId = model.ParentCategoryId,
                    CreatedDate = DateTime.UtcNow,
                    IsSync = model.IsSync,
                };

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentCategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.ParentCategoryName);

            return View(model);
        }
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
                IsSync = category.IsSync,
                ParentCategoryId = category.ParentCategoryId  
            };
            ViewBag.ParentCategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName", category.ParentCategoryId);
            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryName,ParentCategoryId")] CategoryModel categoryModel)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.CategoryName = categoryModel.CategoryName;
                    category.ParentCategoryId = categoryModel.ParentCategoryId;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
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
           
            ViewBag.ParentCategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName", category.ParentCategoryId);
            

            return View(category);
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
