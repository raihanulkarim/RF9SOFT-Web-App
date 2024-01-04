using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoxyBusinessPortolioApp.Models;

namespace RoxyBusinessPortolioApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RoxyBusinessPortolioApp.Models.ProjectModel>? ProjectModel { get; set; }
        public DbSet<RoxyBusinessPortolioApp.Models.Service>? Service { get; set; }
        public DbSet<RoxyBusinessPortolioApp.Models.PortfolioSkill>? Portfolioskill { get; set; }
        public DbSet<RoxyBusinessPortolioApp.Models.Portfolio>? Portfolios { get; set; }
    }
}