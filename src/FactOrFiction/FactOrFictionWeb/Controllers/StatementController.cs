using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FactOrFictionWeb.Models;

namespace FactOrFictionWeb.Controllers
{
    [Authorize]
    public class StatementController : Controller
    {
        private TextBlobContext db = new TextBlobContext();

        // GET: Statement/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Statement statementModel = await db.Statements.FindAsync(id);
            if (statementModel == null)
            {
                return HttpNotFound();
            }

            await db.Entry(statementModel).Collection(p => p.References).LoadAsync();
            return View(statementModel);
        }
    }
}