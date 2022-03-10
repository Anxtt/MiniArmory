using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniArmory.Data.Data.Models;

namespace MiniArmory.Data.Data
{
    public class MiniArmoryDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MiniArmoryDbContext(DbContextOptions<MiniArmoryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Faction> Factions { get; set; }

        public DbSet<Mount> Mounts { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Realm> Realms { get; set; }

        public DbSet<Spell> Spells { get; set; }
    }
}