using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FactOrFictionTextHandling.Parser;
using FactOrFictionUrlSuggestions;
using FactOrFictionWeb.Models;

namespace FactOrFictionWeb.Controllers
{
    [Authorize]
    public class ReferenceController : Controller
    {
        private TextBlobContext db = new TextBlobContext();

        // GET: Reference/Create/parentId
        public ActionResult Create(Guid parentId)
        {
            var referenceModel = new Reference
            {
                StatementId = parentId
            };
            return View(referenceModel);
        }

        // POST: Reference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,LinkString,StatementId")] Reference referenceModel)
        {
            if (ModelState.IsValid)
            {
                var urlClassifier = new URLClassification();

                referenceModel.Id = Guid.NewGuid();
                referenceModel.CreatedBy = User.Identity.Name;
                referenceModel.Tags = new List<string>
                {
                    await urlClassifier.ClassifyOutletDescription(referenceModel.Link.Host)
                };

                // Add Reference
                db.References.Add(referenceModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Statement", new { id = referenceModel.StatementId });
            }

            return View(referenceModel);
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