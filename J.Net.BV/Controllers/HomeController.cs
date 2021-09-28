using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace J.Net.BV.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        IRepository<Documents> db;

        public HomeController()
        {
            //db = new EFDocumentRepository();
            db = new AdoDocumentRepository();
        }

        public ActionResult Index()
        {
            return View(db.Get());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Documents> Get()
        {
            return db.Get();
		}

		[HttpGet("{id}")]
		public Documents Get(int id)
		{
			return db.Get(id);			
		}

		[HttpPost]
        public void Post()
        {
            db.Post();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            db.Delete(id);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    } 
}

