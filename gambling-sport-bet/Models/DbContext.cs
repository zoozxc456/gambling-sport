using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace gamblingsportbet.Models
{
    public class BetDbContext : DbContext
    {
        public BetDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BetRecordModel>(entity =>
            {
                entity.ToTable("bet_records")
                .HasKey(x => new { x.MemberId, x.GameId });
            });
        }
        public DbSet<BetRecordModel> BetRecords { get; set; }
    }
}
