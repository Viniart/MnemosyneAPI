using Microsoft.EntityFrameworkCore;
using MnemosyneAPI.Model;

namespace MnemosyneAPI.Context
{
    public class MemoryDbContext : DbContext
    {
        public MemoryDbContext(DbContextOptions<MemoryDbContext> options) : base(options)
        {
            
        }

        public DbSet<Memory> Memories => Set<Memory>();
    }
}
