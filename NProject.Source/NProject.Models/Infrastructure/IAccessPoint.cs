using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using NProject.Models.Domain;

namespace NProject.Models.Infrastructure
{
    public interface IAccessPoint : IObjectContextAdapter
    {
        IDbSet<Project> Projects { get; set; }
        IDbSet<Meeting> Meeting { get; set; }
        IDbSet<Task> Tasks { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<ProjectStatus> ProjectStatuses { get; set; }

        int SaveChanges();
    }

    public class DbAccessPoint : DbContext, IAccessPoint
    {
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Meeting> Meeting { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<ProjectStatus> ProjectStatuses { get; set; }

        static DbAccessPoint()
        {
            Database.SetInitializer(new RecreateSchemaIfModelChanges<DbAccessPoint>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().
                HasMany(p => p.Team).
                WithMany(u => u.Projects).Map(
                    t => t.MapLeftKey("ProjectId").
                             MapRightKey("UserId").
                             ToTable("UsersOnProjects"));

            modelBuilder.Entity<Meeting>().
                HasMany(m => m.Members).
                WithMany(u => u.Meetings).Map(
                    t => t.MapLeftKey("MeetingId").
                             MapRightKey("UserId").
                             ToTable("UsersOnMeetings"));
        }
    }
}