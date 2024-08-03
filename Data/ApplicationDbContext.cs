using Microsoft.EntityFrameworkCore;
using System.Linq;
public class ApplicationDbContext : DbContext    
    {
        public DbSet<Assess> Assesses {get; set;}
        public DbSet<Learn> Learns {get; set;}
        public DbSet<Live> Lives {get; set;}

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        
    }