using J.Net.BV.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace J.Net.BV
{
	public class EFDocumentRepository : IRepository<Documents>
    {
        private localContext db;

        public EFDocumentRepository()
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
            var activeList = db.DocumentsStatuses.Where(x => x.StatusId == (int)DocumentsStatuses.Status.CREATED).Select(x => x.DocumentId).ToList();
            return db.Documents.Where(x => activeList.Contains(x.Id)).ToList();
        }

        public Documents Get(int id)
        {
            var doc = db.Documents.Where(x => x.Id == id).FirstOrDefault();
            if (db.DocumentsStatuses.Where(x => x.DocumentId == doc.Id).First().StatusId == (int)DocumentsStatuses.Status.CREATED)
            {
                return db.Documents.Where(x => x.Id == id).FirstOrDefault();
            }
            return null;
        }

        public void Post()
        {
            var d = db.Documents.Add(
                    new Documents()
                    {
                        Amount = new Random().Next(0, 9999999),
                        Description = new Random().Next(0, 9999999).ToString()
                    });
            db.SaveChanges();
            db.DocumentsStatuses.Add(
                 new DocumentsStatuses
                 {
                     DocumentId = d.Entity.Id,
                     StatusId = (int)DocumentsStatuses.Status.CREATED,
                 }
                );
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.DocumentsStatuses.Where(x => x.DocumentId == id).First().StatusId = (int)DocumentsStatuses.Status.DELETED;
            db.SaveChanges();
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
