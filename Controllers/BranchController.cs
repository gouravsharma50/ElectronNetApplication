using DesktopApplication.Database;
using DesktopApplication.Models;
using DesktopApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace DesktopApplication.Controllers
{
    [Authorize(Roles = "ADMIN, CORPORATION")]
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BusinessService _businessService;
        public BranchController(ApplicationDbContext context, BusinessService businessService)
        {
            _context = context;
            _businessService = businessService;
        }

        // GET: Branch
        public async Task<IActionResult> Index()
        {
            var branches = await _context.Branches
                .Include(b => b.Corporation)
                .Select(b => new BranchModel
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    CorporationId = b.CorporationId,
                    BranchCreatedDate = b.BranchCreatedDate,
                    Corporation = b.Corporation
                })
                .ToListAsync();

            return View(branches);
        }

        // GET: Branch/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Corporation)
                .FirstOrDefaultAsync(m => m.BranchId == id);

            if (branch == null)
            {
                return NotFound();
            }

            var branchModel = new BranchModel
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                CorporationId = branch.CorporationId,
                BranchCreatedDate = branch.BranchCreatedDate,
                Corporation = branch.Corporation
            };

            return View(branchModel);
        }

        // GET: Branch/Create
        public IActionResult Create()
        {
            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName");
            return View();
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,BranchName,CorporationId")] BranchModel branchModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = branchModel.BranchName,
                    Password = "123456",
                    Role = "BRANCH"
                };

                var createdUser = _businessService.CreateUser(user);
                var branch = new Branch
                {
                    BranchName = branchModel.BranchName,
                    CorporationId = branchModel.CorporationId,
                    BranchCreatedDate = DateTime.UtcNow,
                    IsSync = false
                };

                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", branchModel.CorporationId);
            return View(branchModel);
        }

        // GET: Branch/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            var branchModel = new BranchModel
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                CorporationId = branch.CorporationId,
                // BranchCreatedDate and IsSync are handled by the database
            };

            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", branch.CorporationId);
            return View(branchModel);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchId,BranchName,CorporationId")] BranchModel branchModel)
        {
            if (id != branchModel.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var branch = await _context.Branches.FindAsync(id);

                if (branch == null)
                {
                    return NotFound();
                }

                branch.BranchName = branchModel.BranchName;
                branch.CorporationId = branchModel.CorporationId;
                // Do not update BranchCreatedDate or IsSync; these are managed on the database level

                _context.Update(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CorporationId"] = new SelectList(_context.Corporations, "CorporationId", "CorporationName", branchModel.CorporationId);
            return View(branchModel);
        }

        // GET: Branch/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Corporation)
                .FirstOrDefaultAsync(m => m.BranchId == id);

            if (branch == null)
            {
                return NotFound();
            }

            var branchModel = new BranchModel
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                CorporationId = branch.CorporationId,
                BranchCreatedDate = branch.BranchCreatedDate,
                Corporation = branch.Corporation
            };

            return View(branchModel);
        }

        // POST: Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branches.Any(e => e.BranchId == id);
        }
    }
}