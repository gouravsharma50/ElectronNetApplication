using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesktopApplication.Database;
using DesktopApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace DesktopApplication.Controllers
{
     [Authorize(Roles = "ADMIN, BRANCH")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.Corporation)
                .Include(u => u.Branch)
                .Select(u => new UserModel
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    CreatedDate = u.CreatedDate,
                    Corporation = u.Corporation,
                    Branch = u.Branch
                })
                .ToList();

            var categories = _context.Categories
                .Include(c => c.Corporation)
                .Include(c => c.Branch)
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
                .ToList();

            var model = new Tuple<IEnumerable<UserModel>, IEnumerable<CategoryModel>>(users, categories);

            return View(model);
        }


        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Branch)
                .Include(u => u.Corporation)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Map the User entity to UserModel
            var userModel = new UserModel
            {
                UserId = user.UserId,
                Username = user.Username,
                CreatedDate = user.CreatedDate,
                Corporation = user.Corporation,
                Branch = user.Branch
            };

            // Return the mapped UserModel to the view
            return View(userModel);
        }


        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CorporationId,BranchId,Username")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                // Mapping from UserModel to User
                var user = new User
                {
                    Password= userModel.Password,
                    CorporationId = userModel.CorporationId,
                    BranchId = userModel.BranchId,
                    Username = userModel.Username,
                    Role = "USER", // Default role
                    CreatedDate = DateTime.UtcNow, // Default created date
                    IsSync = false // Default IsSync value
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", userModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", userModel.CorporationId);
            return View(userModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserModel
            {
                UserId = user.UserId,
                CorporationId = user.CorporationId,
                BranchId = user.BranchId,
                Username = user.Username,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                IsSync = user.IsSync
            };

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", userModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", userModel.CorporationId);
            return View(userModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,CorporationId,BranchId,Username,Role,CreatedDate,IsSync")] UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.CorporationId = userModel.CorporationId;
                    user.BranchId = userModel.BranchId;
                    user.Username = userModel.Username;
                    user.Role = userModel.Role; // Allow role editing
                    user.CreatedDate = userModel.CreatedDate; // Allow updating CreatedDate if needed
                    user.IsSync = userModel.IsSync; // Allow updating IsSync if needed

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userModel.UserId))
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

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", userModel.BranchId);
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", userModel.CorporationId);
            return View(userModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Branch)
                .Include(u => u.Corporation)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Map the User entity to UserModel
            var userModel = new UserModel
            {
                UserId = user.UserId,
                Username = user.Username,
                CreatedDate = user.CreatedDate,
                Corporation = user.Corporation,
                Branch = user.Branch
            };

            // Return the mapped UserModel to the view
            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
