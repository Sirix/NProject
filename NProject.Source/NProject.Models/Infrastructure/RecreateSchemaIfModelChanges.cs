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

namespace NProject.Models.Infrastructure
{
    public partial class RecreateSchemaIfModelChanges<T> : IDatabaseInitializer<T> where T : DbContext, IAccessPoint
    {
        private EdmMetadata _edmMetaData;

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
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            string modelHash = this.GetModelHash(objectContext);
            if (!this.CompatibleWithModel(modelHash, context, objectContext))
            {
                this.DeleteExistingTables(objectContext);
                this.CreateTables(objectContext);
                this.SaveModelHashToDatabase(context, modelHash, objectContext);
                this.Seed(context);
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
}
