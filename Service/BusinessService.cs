using DesktopApplication.Database;
using DesktopApplication.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;


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
                BranchId=user.BranchId,
                CorporationId=user.CorporationId,
                Password = user.Password,
                Username = user.Username,
                Role = user.Role, 
                CreatedDate = DateTime.UtcNow,
                IsSync = false 
            };
            _context.Add(profile);
            _context.SaveChanges();
            return profile;
        }
    }
}
