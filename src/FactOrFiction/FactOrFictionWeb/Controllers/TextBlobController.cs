using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
    public class TextBlobController : Controller
    {
        private TextBlobContext db = new TextBlobContext();

        // GET: TextBlob
        public async Task<ActionResult> Index()
        {
            return View(await db.TextBlobModels.ToListAsync());
        }

        // GET: TextBlob/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextBlobModel textBlobModel = await db.TextBlobModels.FindAsync(id);
            if (textBlobModel == null)
            {
                return HttpNotFound();
            }

            await db.Entry(textBlobModel).Collection(p => p.Statements).LoadAsync();
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Text")] TextBlobModel textBlobModel)
        {
            if (ModelState.IsValid)
            {
                var finder = FinderFactory.CreateFinder();

                textBlobModel.Id = Guid.NewGuid();

                var statementTasks = ShittyParser.PuctuationParse(textBlobModel.Text).Select(async text => new Statement
                {
                    Id = Guid.NewGuid(),
                    Text = text,
                    Classification = StatementClassification.Other,
                    References = (await finder.FindSuggestions(text)).Select(uri => new Reference
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = "System",
                        Link = uri,
                        Tags = new List<string>()
                    }).ToList()
                });
                var statements = await Task.WhenAll(statementTasks);

                textBlobModel.Statements = statements.ToList();

                // Add TextBlob
                db.TextBlobModels.Add(textBlobModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(textBlobModel);
        }

        // GET: TextBlobModelsController2/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextBlobModel textBlobModel = await db.TextBlobModels.FindAsync(id);
            if (textBlobModel == null)
            {
                return HttpNotFound();
            }
            return View(textBlobModel);
        }

        // POST: TextBlobModelsController2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            TextBlobModel textBlobModel = await db.TextBlobModels.FindAsync(id);
            if (textBlobModel == null)
            {
                return HttpNotFound();
            }

            await db.Entry(textBlobModel).Collection(p => p.Statements).LoadAsync();

            db.TextBlobModels.Remove(textBlobModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
#if (!DEBUG)
            filterContext.ExceptionHandled = true;
#endif

            filterContext.Result = View("Error");
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
