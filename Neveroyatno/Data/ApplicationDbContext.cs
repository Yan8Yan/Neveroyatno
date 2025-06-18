using Microsoft.EntityFrameworkCore;
using Neveroyatno.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Neveroyatno.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }  
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TestResult> TestResults { get; set; }


        //здесь описаны связи таблиц (т.е один-к-одному и так далее)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.Test)
                .WithOne(t => t.Lecture)
                .HasForeignKey<Test>(t => t.LectureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.Tasks)
                .WithOne(tsk => tsk.Test)
                .HasForeignKey(tsk => tsk.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.Author)
                .WithMany()
                .HasForeignKey(l => l.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}







