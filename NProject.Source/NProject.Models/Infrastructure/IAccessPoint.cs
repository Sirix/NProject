using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using NProject.Models.Domain;

namespace NProject.Models.Infrastructure
{
    public interface IAccessPoint : IObjectContextAdapter
    {
        IDbSet<Project> Projects { get; set; }
        IDbSet<Task> Tasks { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<ProjectStatus> ProjectStatuses { get; set; }
        IDbSet<Role> Roles { get; set; }

        int SaveChanges();
    }

    public class DbAccessPoint : DbContext, IAccessPoint
    {
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<ProjectStatus> ProjectStatuses { get; set; }
        public IDbSet<Role> Roles { get; set; }

        static DbAccessPoint()
        {
            Database.SetInitializer(new NewDatabaseInitializer<DbAccessPoint>());
        }
        public DbAccessPoint()
        {
            Projects = Set<Project>();
            Tasks = Set<Task>();
            Users = Set<User>();
            ProjectStatuses = Set<ProjectStatus>();
            Roles = Set<Role>();
        }
    }
/*
    [Obsolete("Using Moq<IAccessPoint> instead.")]
    public class FakeAccessPoint : IAccessPoint
    {
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Status> Statuses { get; set; }

        public FakeAccessPoint()
        {
            Projects = new InMemoryDbSet<Project>();
            Tasks = new InMemoryDbSet<Task>();
            Users = new InMemoryDbSet<User>();
        }

        public int SaveChanges()
        {
            return 1;
        }

        public System.Data.Objects.ObjectContext ObjectContext
        {
            get { throw new NotSupportedException(); }
        }
    }
 * */
}