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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public AlphaDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(entity =>
                {
                    entity.HasOne(e => e.User).WithMany(e => e.Reservations).HasForeignKey(e => e.UserId);
                    entity.HasOne(e => e.Room).WithMany(e => e.Reservations).HasForeignKey(e => e.RoomId);
                }
             );
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AlphaDb;Trusted_Connection=True;");
        }
    }
}
