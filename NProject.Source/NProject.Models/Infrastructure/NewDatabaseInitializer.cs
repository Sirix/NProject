using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Design;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Devtalk.EF.CodeFirst;
using NProject.Models.Domain;

namespace NProject.Models.Infrastructure
{
    class RecreateSchemaIfModelChanges<T> :IDatabaseInitializer<T> where T : DbContext
    {
    // Fields
    private EdmMetadata _edmMetaData;
    private const string Deletealltablesscript = "declare @cmd varchar(4000)\r\n                declare cmds cursor for \r\n                Select\r\n                    'drop table [' + Table_Name + ']'\r\n                From\r\n                    INFORMATION_SCHEMA.TABLES\r\n\r\n                open cmds\r\n                while 1=1\r\n                begin\r\n                    fetch cmds into @cmd\r\n                    if @@fetch_status != 0 break\r\n                    print @cmd\r\n                    exec(@cmd)\r\n                end\r\n                close cmds\r\n                deallocate cmds";
    private const string Dropallconstraintsscript = "select  \r\n                'ALTER TABLE ' + so.table_name + ' DROP CONSTRAINT ' + so.constraint_name  \r\n                from INFORMATION_SCHEMA.TABLE_CONSTRAINTS so";
    private const string LookupEdmMetaDataTable = "Select COUNT(*) \r\n              FROM INFORMATION_SCHEMA.TABLES T \r\n              Where T.TABLE_NAME = 'EdmMetaData'";

    // Methods
    private bool CompatibleWithModel(string modelHash, DbContext context, ObjectContext objectContext)
    {
        if (objectContext.ExecuteStoreQuery<int>("Select COUNT(*) \r\n              FROM INFORMATION_SCHEMA.TABLES T \r\n              Where T.TABLE_NAME = 'EdmMetaData'", new object[0]).FirstOrDefault<int>() == 1)
        {
            this._edmMetaData = context.Set<EdmMetadata>().FirstOrDefault<EdmMetadata>();
            if (this._edmMetaData != null)
            {
                return (modelHash == this._edmMetaData.ModelHash);
            }
        }
        return false;
    }

    private static string ComputeSha256Hash(string input)
    {
        byte[] buffer = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(input));
        StringBuilder builder = new StringBuilder(buffer.Length * 2);
        foreach (byte num in buffer)
        {
            builder.Append(num.ToString("X2", CultureInfo.InvariantCulture));
        }
        return builder.ToString();
    }

    private void CreateTables(ObjectContext objectContext)
    {
        string commandText = objectContext.CreateDatabaseScript();
        objectContext.ExecuteStoreCommand(commandText, new object[0]);
    }

    private string GetCsdlXmlString(ObjectContext context)
    {
        if (context != null)
        {
            ReadOnlyCollection<EntityContainer> items = context.MetadataWorkspace.GetItems<EntityContainer>(DataSpace.SSpace);
            if (items != null)
            {
                EntityModelSchemaGenerator generator = new EntityModelSchemaGenerator(items.FirstOrDefault<EntityContainer>());
                StringBuilder output = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(output);
                generator.GenerateMetadata();
                generator.WriteModelSchema(writer);
                writer.Flush();
                return output.ToString();
            }
        }
        return string.Empty;
    }

    private string GetModelHash(ObjectContext context)
    {
        return ComputeSha256Hash(this.GetCsdlXmlString(context));
    }

    public void InitializeDatabase(T context)
    {
        ObjectContext objectContext = ((IObjectContextAdapter) context).ObjectContext;
        string modelHash = this.GetModelHash(objectContext);
        if (!this.CompatibleWithModel(modelHash, context, objectContext))
        {
            this.DeleteExistingTables(objectContext);
            this.CreateTables(objectContext);
            this.SaveModelHashToDatabase(context, modelHash, objectContext);
        }
    }

    private void SaveModelHashToDatabase(T context, string modelHash, ObjectContext objectContext)
    {
        if (this._edmMetaData != null)
        {
            objectContext.Detach(this._edmMetaData);
        }
        this._edmMetaData = new EdmMetadata();
        context.Set<EdmMetadata>().Add(this._edmMetaData);
        this._edmMetaData.ModelHash = modelHash;
        context.SaveChanges();
    }

        private void DeleteExistingTables(ObjectContext objectContext)
        {
            var dropConstraintsScript =
                @"declare @cmd varchar(4000)
declare cmds cursor for 
               select  'ALTER TABLE ' + so.TABLE_NAME + ' DROP CONSTRAINT ' + so.constraint_name  from INFORMATION_SCHEMA.TABLE_CONSTRAINTS so order by so.CONSTRAINT_TYPE
open cmds
	while 1=1
       begin
	      fetch cmds into @cmd   
		      if @@fetch_status != 0 break
			                   print @cmd
			                   exec(@cmd)
       end
close cmds
		        deallocate cmds";
            string dropTablesScript =
                @"declare @cmd varchar(4000)
declare cmds cursor for 
               Select 'drop table [' + Table_Name + ']' From INFORMATION_SCHEMA.TABLES
open cmds
	while 1=1
       begin
	      fetch cmds into @cmd   
		      if @@fetch_status != 0 break
			                   print @cmd
			                   exec(@cmd)
       end
close cmds
		        deallocate cmds";
            objectContext.ExecuteStoreCommand(dropConstraintsScript);
            objectContext.ExecuteStoreCommand(dropTablesScript);
        }

    }
    class NewDatabaseInitializer<T> : RecreateSchemaIfModelChanges<T> where T : DbContext, IAccessPoint
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
