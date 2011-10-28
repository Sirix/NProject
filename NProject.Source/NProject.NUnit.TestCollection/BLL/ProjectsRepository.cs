using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NProject.BLL;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;
using NUnit.Framework;

namespace NProject.NUnit.TestCollection.BLL
{
    class ProjectsRepository_Tests
    {
        [Test]
        public void Get_Project_List_Depends_On_His_Role()
        {


            //var user = new User {Username = "Manager", Id = 1, Role = db.Roles.First(i => i.Id == 1)};

            //db.SetupGet(x => x.Users).Returns(new InMemoryDbSet<User>(user));
            //db.SetupGet(x => x.Projects).Returns(
            //    new InMemoryDbSet<Project>(new Project { Id = 1 }));
            //taskController.AccessPoint = db.Object;
        }
    }
}
