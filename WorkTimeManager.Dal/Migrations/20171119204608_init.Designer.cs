using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WorkTimeManager.LocalDB.Context;

namespace WorkTimeManager.LocalDB.Migrations
{
    [DbContext(typeof(WorkTimeContext))]
    [Migration("20171119204608_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3");

            modelBuilder.Entity("WorkTimeManager.Model.Models.Issue", b =>
                {
                    b.Property<int>("IssueID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsFavourite");

                    b.Property<string>("Priority");

                    b.Property<int>("ProjectID");

                    b.Property<string>("Subject");

                    b.Property<string>("Tracker");

                    b.Property<DateTime>("Updated");

                    b.HasKey("IssueID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("WorkTimeManager.Model.Models.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("ParentProjectID");

                    b.HasKey("ProjectID");

                    b.HasIndex("ParentProjectID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("WorkTimeManager.Model.Models.WorkTime", b =>
                {
                    b.Property<int>("WorkTimeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<bool>("Dirty");

                    b.Property<double>("Hours");

                    b.Property<int>("IssueID");

                    b.Property<DateTime?>("StartTime");

                    b.HasKey("WorkTimeID");

                    b.HasIndex("IssueID");

                    b.ToTable("WorkTimes");
                });

            modelBuilder.Entity("WorkTimeManager.Model.Models.Issue", b =>
                {
                    b.HasOne("WorkTimeManager.Model.Models.Project", "Project")
                        .WithMany("Issues")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkTimeManager.Model.Models.Project", b =>
                {
                    b.HasOne("WorkTimeManager.Model.Models.Project", "ParentProject")
                        .WithMany("ChildrenProjects")
                        .HasForeignKey("ParentProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkTimeManager.Model.Models.WorkTime", b =>
                {
                    b.HasOne("WorkTimeManager.Model.Models.Issue", "Issue")
                        .WithMany("WorkTimes")
                        .HasForeignKey("IssueID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
