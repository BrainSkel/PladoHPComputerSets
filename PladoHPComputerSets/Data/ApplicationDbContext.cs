using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PladoHPComputerSets.Models;

namespace PladoHPComputerSets.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PladoHPComputerSets.Models.ComputerOrder> ComputerOrder { get; set; }
    }
}