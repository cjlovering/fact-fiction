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
using FactOrFictionTextHandling.Luis;
using FactOrFictionTextHandling.Parser;
using FactOrFictionTextHandling.StatementProducer;
using FactOrFictionUrlSuggestions;
using FactOrFictionCommon.Models;

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
            await db.Entry(textBlobModel).Collection(p => p.Entities).LoadAsync();
            await FetchPersonas(textBlobModel);
            return View(new TextBlobModel(textBlobModel));
        }

        private async Task FetchPersonas(TextBlobModel textBlobModel)
        {
            foreach (var e in textBlobModel.Entities)
            {
                e.Persona = PersonasDBLookups.ByName[e.Name].FirstOrDefault();
                if (e.Persona != null)
                {
                    await e.Persona.FetchRecentStatements();
                }
            }
        }

        // GET: TextBlob/Create
        public ActionResult Create()
        {
            var textBlobModel = new TextBlobModel
            {
                Statements = new List<Statement>()
            };
            return View(textBlobModel);
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
                var statementProducer = new StatementProducer(new LuisClientFactory("https://eastus2.api.cognitive.microsoft.com/luis/v2.0/apps/79af6370-41bd-4d03-9c7c-5f234eb6049c?subscription-key=784cc32302a84581ab894febc8775393&timezoneOffset=0&verbose=true&q=").Create());
                var finder = FinderFactory.CreateFinder();
                var urlClassifier = new URLClassification();

                textBlobModel.Id = Guid.NewGuid();
                textBlobModel.CreatedBy = User.Identity.Name;

                var statementTasks = Task.WhenAll(statementProducer.GetStatements(textBlobModel));
                var statements = await statementTasks;

                // Generate references for each statement
                var statementTasks2 = statements
                    .Select(async statement =>
                    {
                        if (statement.Classification == StatementClassification.Other) return statement;

                        var referenceTasks = (await finder.FindSuggestions(statement.Text)).Select(async uri =>
                        {
                            var bias = (await urlClassifier.ClassifyOutletDescription(uri.Host));

                            return new Reference
                            {
                                Id = Guid.NewGuid(),
                                CreatedBy = "System",
                                Link = uri,
                                Tags = new List<string>(),
                                Bias = bias == null ? null : new Bias(bias, Guid.NewGuid())
                            };
                        });

                        var references = await Task.WhenAll(referenceTasks);
                        return new Statement(statement, references.ToList());
                    });

                var statementsWithReferences = await Task.WhenAll(statementTasks2);
                textBlobModel.Statements = statementsWithReferences.ToList();

                var entityFinder = new EntityFinder();
                var entities = await entityFinder.GetEntities(textBlobModel.Text);
                textBlobModel.Entities = entities
                    .Select(e => new Entity
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = "Microsoft Entity Linking",
                        Name = entityFinder.ExtractEntityName(e),
                        WikiUrl = entityFinder.ExtractEntityWikiUrlString(e),
                        Matches = entityFinder.ExtractMatches(e)
                            .Select(tuple => new Match
                            {
                                Id = Guid.NewGuid(),
                                Text = tuple.Item1,
                                Offset = tuple.Item2
                            })
                            .ToList()
                    })
                    .ToList();

                // Save TextBlob
                db.TextBlobModels.Add(textBlobModel);
                await db.SaveChangesAsync();
                await FetchPersonas(textBlobModel);
                return View(textBlobModel);
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
            await db.Entry(textBlobModel).Collection(p => p.Entities).LoadAsync();
            db.TextBlobModels.Remove(textBlobModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

//        protected override void OnException(ExceptionContext filterContext)
//        {
////#if (!DEBUG)
////            filterContext.ExceptionHandled = true;
////#endif

//            filterContext.Result = View("Error");
//        }

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
