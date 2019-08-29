using Alpha.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Data
{
    public class AlphaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public AlphaDbContext(DbContextOptions<AlphaDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Reservation>(entity =>
                {
                    entity.HasOne(e => e.User)
                    .WithMany(e => e.Reservations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(e => e.Room)
                    .WithMany(e => e.Reservations)
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                }
             );
            base.OnModelCreating(modelBuilder);
        }

    }
}
