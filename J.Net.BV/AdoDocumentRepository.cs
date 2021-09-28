using J.Net.BV.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace J.Net.BV
{
	public class AdoDocumentRepository : IRepository<Documents>
    {
        private localContext db;
                
        readonly string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=local;Integrated Security=SSPI;";

        public AdoDocumentRepository()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<localContext>();

            var options = optionsBuilder
                    .UseSqlServer(connectionString)
                    .Options;

            this.db = new localContext(options);
        }

        public IEnumerable<Documents> Get()
        {
            List<Documents> result = new List<Documents>();
            string queryStrings = 
                @"SELECT d.[id]
                      ,[Amount]
                      ,[Description]
                  FROM [local].[dbo].[Documents] d
                  left join dbo.DocumentsStatuses ds with(nolock) on ds.DocumentId =  d.id
                  where ds.StatusId = 1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(queryStrings, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Documents.fromDataReader(reader));
                        }
                    }
                }
            }
            return result;
        }

        public Documents Get(int id)
        {
            string queryStrings =
                   @"SELECT d.[id]
                      ,[Amount]
                      ,[Description]
                  FROM [local].[dbo].[Documents] d
                  left join dbo.DocumentsStatuses ds with(nolock) on ds.DocumentId =  d.id
                  where ds.StatusId = 1 and d.[id] = {0};";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format(queryStrings, id), connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Documents.fromDataReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public void Post()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                string.Format(@"INSERT INTO [dbo].[Documents]
                                       ([Amount]
                                       ,[Description])
                                VALUES (@Amount, @Description);
                                INSERT INTO[dbo].[DocumentsStatuses]
                                            ([DocumentId]
                                        ,[StatusId])
                                    VALUES(SCOPE_IDENTITY(), 1);"), connection))
                {
                    command.Parameters.Add("@Amount", SqlDbType.Float).Value = new Random().Next(0, 9999999);
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = new Random().Next(0, 9999999).ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                string.Format(@"DELETE FROM [dbo].[Documents] WHERE id = @id;DELETE FROM [dbo].[DocumentsStatuses] WHERE [DocumentId] = @id;"), connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{                    
					db.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

	}
}
