using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactOrFictionWeb.Models;

namespace FactOrFictionWeb.Controllers
{
    public class TextBlobController : Controller
    {
        private TextBlobContext db = new TextBlobContext();

        // GET: TextBlob
        public ActionResult Index()
        {
            return View(db.TextBlobModels.ToList());
        }

        // GET: TextBlob/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextBlobModel textBlobModel = db.TextBlobModels.Find(id);
            if (textBlobModel == null)
            {
                return HttpNotFound();
            }
            return View(textBlobModel);
        }

        // GET: TextBlob/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextBlob/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] TextBlobModel textBlobModel)
        {
            if (ModelState.IsValid)
            {
                textBlobModel.Id = Guid.NewGuid();
                db.TextBlobModels.Add(textBlobModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(textBlobModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
