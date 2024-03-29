﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using MiniArmory.Data.Models;

namespace MiniArmory.Data
{
    public class MiniArmoryDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MiniArmoryDbContext(DbContextOptions<MiniArmoryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Character
            builder.Entity<Character>()
                .HasOne(x => x.User)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasOne(x => x.Race)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.RaceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasOne(x => x.Class)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasOne(x => x.Faction)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasOne(x => x.Realm)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.RealmId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasOne(x => x.Partner)
                .WithOne()
                .HasForeignKey<Character>(x => x.PartnerId)
                .IsRequired(false);

            builder.Entity<Character>()
                .HasIndex(x => x.PartnerId)
                .IsUnique(false);

            builder.Entity<Character>()
                .HasOne(x => x.Image)
                .WithOne(x => x.Character)
                .HasForeignKey<ImageData>(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                .HasIndex(x => x.ImageId)
                .IsUnique(false);

            //Race
            builder.Entity<Race>()
                .HasOne(x => x.Faction)
                .WithMany(x => x.Races)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Race>()
                .HasOne(x => x.RacialSpell)
                .WithOne(x => x.Race)
                .HasForeignKey<Spell>(x => x.RaceId)
                .OnDelete(DeleteBehavior.Restrict);

            //Spell
            builder.Entity<Spell>()
                .HasOne(x => x.Class)
                .WithMany(x => x.Spells)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Spell>()
                .HasIndex(x => x.RaceId)
                .IsUnique(false);

            base.OnModelCreating(builder);
        }

        public virtual DbSet<Achievement> Achievements { get; set; }

        public virtual DbSet<Character> Characters { get; set; }

        public virtual DbSet<Class> Classes { get; set; }

        public virtual DbSet<Faction> Factions { get; set; }

        public virtual DbSet<ImageData> Images { get; set; }

        public virtual DbSet<Mount> Mounts { get; set; }

        public virtual DbSet<Race> Races { get; set; }

        public virtual DbSet<Realm> Realms { get; set; }

        public virtual DbSet<Spell> Spells { get; set; }
    }
}