using DesktopApplication.Database;
using DesktopApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace DesktopApplication.Service
{
    public class BusinessService
    {
        private readonly ApplicationDbContext _context;

        public BusinessService(ApplicationDbContext context)
        {
            _context = context;
        }
        public User CreateUser(User user)
        {
            var profile = new User
            {
                Password = user.Password,
                Username = user.Username,
                Role = user.Role, 
                CreatedDate = DateTime.UtcNow,
                IsSync = false 
            };
            _context.Add(user);
            _context.SaveChanges();
            return profile;
        }
    }
}
