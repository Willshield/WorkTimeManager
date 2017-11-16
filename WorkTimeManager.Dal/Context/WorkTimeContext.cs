using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkTimeManager.Dal.Context
{
    public class WorkTimeContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkTime> WorkTimes { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WorkTimeManager.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(project =>
            {
                project
                    .HasMany(p => p.ChildrenProjects)
                    .WithOne(p => p.ParentProject)
                    .HasForeignKey(p => p.ParentProjectID);
            });
        }

    }
}
