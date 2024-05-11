using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace gamblingsportgame.Models
{
	public class GameDbContext: DbContext
	{
        public GameDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public DbSet<GameModel> Games { get; set; }
    }
}
