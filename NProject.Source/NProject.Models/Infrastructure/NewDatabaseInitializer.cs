using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Devtalk.EF.CodeFirst;
using NProject.Models.Domain;

namespace NProject.Models.Infrastructure
{
    class NewDatabaseInitializer<T> : DontDropDbJustCreateTablesIfModelChanged<T> where T : DbContext, IAccessPoint
    {
        public new void InitializeDatabase(T context)
        {
            base.InitializeDatabase(context);
            this.Seed(context);
        }

        internal void Seed(T context)
        {
            SeedNeededToLaunchData(context);
            SeedFakeData(context);
        }
        private void SeedFakeData(T context)
        {
            context.Users.Add(new User
                                  {
                                      Username = "Director",
                                      Email = "director@nproject.com",
                                      Hash = EncryptMD5("director"),
                                      Role = context.Roles.First(r => r.Name == "Director")
                                  });
            var m1 = new User
                         {
                             Username = "Manager",
                             Email = "manager@nproject.com",
                             Hash = EncryptMD5("manager"),
                             Role = context.Roles.First(r => r.Name == "PM")
                         };
            var m2 = new User
                         {
                             Username = "Stiv",
                             Email = "veryCoolPM@Stiv.com",
                             Hash = EncryptMD5("Stiv"),
                             Role = context.Roles.First(r => r.Name == "PM")
                         };
            var p1 = new User
                         {
                             Username = "Mark",
                             Email = "coolProgrammer@nproject.com",
                             Hash = EncryptMD5("Mark"),
                             Role = context.Roles.First(r => r.Name == "Programmer")
                         };
            var customer = new User
                               {
                                   Username = "Customer",
                                   Email = "Customer@nproject.com",
                                   Hash = EncryptMD5("customer"),
                                   Role = context.Roles.First(r => r.Name == "Customer")
                               };

            context.Users.Add(new User
                                  {
                                      Username = "Programmer",
                                      Email = "programmer@nproject.com",
                                      Hash = EncryptMD5("programmer"),
                                      Role = context.Roles.First(r => r.Name == "Programmer")
                                  });

            context.Users.Add(new User
                                  {
                                      Username = "Programmer2",
                                      Email = "programmer2@nproject.com",
                                      Hash = EncryptMD5("programmer2"),
                                      Role = context.Roles.First(r => r.Name == "Programmer")
                                  });

            context.Users.Add(customer);
            context.Users.Add(p1);
            context.Users.Add(m1);
            context.Users.Add(m2);
            // context.SaveChanges();

            var project1 = new Project
                               {
                                   Name = "Develop a project management system",
                                   Customer = customer,
                                   CreationDate = DateTime.Now,
                                   StartDate = DateTime.Now,
                                   DeliveryDate = DateTime.Now.AddDays(1),
                                   Status = context.ProjectStatuses.First(),
                                   Team = new List<User> {m2}
                               };


            context.Projects.Add(project1);
            var task1 = new Task
                            {
                                Title = "Converter",
                                Description = "Create texture convertor",
                                Responsible = p1,
                                CreationDate = DateTime.Now,
                                BeginDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(1),
                                Status = context.ProjectStatuses.First()
                            };
            context.Tasks.Add(task1);
            context.Projects.Add(new Project
                                     {
                                         Name = "Create a 3D game",
                                         Customer = customer,
                                         CreationDate = DateTime.Now,
                                         StartDate = DateTime.Now,
                                         DeliveryDate = DateTime.Now.AddDays(1),
                                         Status = context.ProjectStatuses.First(),
                                         Team = new List<User> {m1, p1},
                                         Tasks =
                                             new List<Task> {task1}
                                     });

            context.SaveChanges();
        }

        internal void SeedNeededToLaunchData(T context)
        {
            var roleAdmin =
                new Role {Name = "Admin", Description = "get fully privilegies on user controlling", BaseLocation="account/list"}.AsBase();

            context.Roles.Add(roleAdmin);
            context.Roles.Add(new Role { Name = "PM", Description = "Project manager" }.AsBase());
            context.Roles.Add(new Role { Name = "Director", Description = "Offce director" }.AsBase());
            context.Roles.Add(new Role { Name = "Customer", Description = "Customer who wants to create an app" }.AsBase());
            context.Roles.Add(new Role { Name = "Programmer", Description = "Works in the office" }.AsBase());

            context.ProjectStatuses.Add(new ProjectStatus {Name = "Just Created", Id = 1}.AsBase());
            context.ProjectStatuses.Add(new ProjectStatus { Name = "Developing", Id = 2 }.AsBase());
            context.ProjectStatuses.Add(new ProjectStatus { Name = "Freezed", Id = 3 }.AsBase());
            context.ProjectStatuses.Add(new ProjectStatus { Name = "Finished", Id = 4 }.AsBase());

            context.Users.Add(new User
                                  {
                                      Username = "admin",
                                      Email = "admin@nproject.com",
                                      Hash = EncryptMD5("admin"),
                                      Role = roleAdmin
                                  });
            
            context.SaveChanges();
        }

        private static string EncryptMD5(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var valueArray = Encoding.UTF8.GetBytes(value);
            valueArray = md5.ComputeHash(valueArray);
            var encrypted = "";
            for (var i = 0; i < valueArray.Length; i++)
                encrypted += valueArray[i].ToString("x2").ToLower();
            return encrypted;
        }
    }
}
