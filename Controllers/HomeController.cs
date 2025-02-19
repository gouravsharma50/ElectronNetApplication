using System.Diagnostics;
using DesktopApplication.Database;
using DesktopApplication.Models;
using DesktopApplication.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly BusinessService _businessService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, BusinessService businessService)
        {
            _logger = logger;
            _context = context;
            _businessService = businessService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and Password are required.";
                return View();
            }

            var user = await _context.Users
                .Where(u => u.Username == userName && u.Password == password)
                .Select(u => new { u.Username, u.Role })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                ViewBag.Error = "Invalid Username or Password.";
                return View();
            }

            // Store user details in session (optional)
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role);
            if (user.Role == "ADMIN")
                return RedirectToAction("Index", "Corporation");
            else if (user.Role == "CORPORATION")
                return RedirectToAction("Index", "Branch");
            else if (user.Role == "BRANCH")
                return RedirectToAction("Index", "User");
            else if (user.Role == "USER")
                return RedirectToAction("Index", "Category");
            else
                return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // ✅ Clear the session
            HttpContext.Session.Clear();
            return RedirectToAction("Login"); // Redirect back to login page
        }
        public IActionResult Privacy()
        {
            return View();
        } 
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
