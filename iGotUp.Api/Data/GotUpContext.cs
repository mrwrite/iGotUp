using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iGotUp.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iGotUp.Api.Data
{
    public class GotUpContext : IdentityDbContext<GotUpUser, IdentityRole<int>, int>
    {
        public GotUpContext(DbContextOptions<GotUpContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Relational().TableName.ToTrimPrefix();
            }
        }

        public DbSet<Run> Runs { get; set; }
        public DbSet<RunPlayers> RunPlayers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
    }

    public static class StringExtensions
    {
        public static string ToTrimPrefix(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Regex.Replace(input, @"^AspNet", "");
        }
    }
}