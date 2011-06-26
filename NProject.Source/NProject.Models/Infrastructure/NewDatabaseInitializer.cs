using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NProject.Models.Domain;

namespace NProject.Models.Infrastructure
{
    class NewDatabaseInitializer<T> : DropCreateDatabaseIfModelChanges<T> where T : DbContext, IAccessPoint
    {
        protected override void Seed(T context)
        {
            SeedNeededToLaunchData(context);

            context.Users.Add(new User
                                  {
                                      Username = "Director",
                                      Email = "director@nproject.com",
                                      Hash = EncryptMD5("director"),
                                      Role = context.Roles.First(r => r.Name == "Director")
                                  });
            var m =
                new User
                    {
                        Username = "Manager",
                        Email = "manager@nproject.com",
                        Hash = EncryptMD5("manager"),
                        Role = context.Roles.First(r => r.Name == "PM")
                    };
            context.Users.Add(m);
            context.Users.Add(new User
                                  {
                                      Username = "Customer",
                                      Email = "Customer@nproject.com",
                                      Hash = EncryptMD5("customer"),
                                      Role = context.Roles.First(r => r.Name == "Customer")
                                  });
            context.Users.Add(new User
                                  {
                                      Username = "Programmer",
                                      Email = "programmer@nproject.com",
                                      Hash = EncryptMD5("programmer"),
                                      Role = context.Roles.First(r => r.Name == "Programmer")
                                  });


            context.Projects.Add(new Project
                                     {
                                         Name = "Develop a project management system",
                                         Customer = context.Users.First(),
                                         CreationDate = DateTime.Now,
                                         Status = context.ProjectStatuses.First(),
                                         Team = new List<User> {m}
                                     });
            context.SaveChanges();
        }

        private static void SeedNeededToLaunchData(T context)
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
            context.ProjectStatuses.Add(new ProjectStatus { Name = "Finished", Id = 3 }.AsBase());

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
