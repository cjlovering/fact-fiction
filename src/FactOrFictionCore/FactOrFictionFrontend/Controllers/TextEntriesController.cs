using FactOrFictionCommon.Models;
using FactOrFictionCommon.Models.RelationshipModels;
using FactOrFictionFrontend.Controllers.Utils;
using FactOrFictionFrontend.Data;
using FactOrFictionTextHandling.MLClient;
using FactOrFictionTextHandling.SentenceProducer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionFrontend.Controllers
{
    public class TextEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string LUIS_URL = "https://eastus2.api.cognitive.microsoft.com/luis/v2.0/apps/79af6370-41bd-4d03-9c7c-5f234eb6049c?subscription-key=784cc32302a84581ab894febc8775393&timezoneOffset=0&verbose=true&q=";
        private const string HACC_URL = "http://127.0.0.1:32788/score";

        public TextEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TextEntries
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Cannot find user with ID '{_userManager.GetUserId(User)}'.");
            }

            var textEntries = (
                from entry in _context.TextEntries
                orderby entry.CreatedAt descending
                select entry
            );

           return View(await textEntries.ToListAsync());
        }

        // GET: TextEntries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textEntry = await _context.TextEntries
                .Include(t => t.CreatedByUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (textEntry == null)
            {
                return NotFound();
            }

            return View(textEntry);
        }

        // GET: TextEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TextEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Content")] TextEntry textEntry)
        {
            if (ModelState.IsValid)
            {
                //var sentenceProducer = new SentenceProducer<LuisResult>(new LuisClient(LUIS_URL), new WorkingParser());
                HaccClient client = new HaccClient(HACC_URL);
                var sentenceProducer = new SentenceProducer<HaccResult>(client, client);
                textEntry.Id = Guid.NewGuid();
                textEntry.UserId = _userManager.GetUserId(User);
                textEntry.CreatedAt = DateTime.Now;

                var parsingTask = await sentenceProducer.GetStatements(textEntry);

                var sentenceTasks = Task.WhenAll(parsingTask);
                var sentences = await sentenceTasks;

                Array.Sort(sentences);

                _context.Add(textEntry);
                _context.AddRange(sentences);
                await _context.SaveChangesAsync();

                var VotesDict = sentences.ToDictionary(sent => sent.Id, sent => VoteType.UNVOTED.ToString());

                return Json(new
                {
                    Sentences = sentences.Select(
                        sent => new SentenceViewModel(sent)),
                    // Return Votes here
                    Votes = VotesDict
                });
            }
            return View(textEntry);
        }

        // GET: TextEntries1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textEntry = await _context.TextEntries.SingleOrDefaultAsync(m => m.Id == id);
            if (textEntry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", textEntry.UserId);
            return View(textEntry);
        }

        // POST: TextEntries1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTextEntry(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textEntryToUpdate = await _context.TextEntries.SingleOrDefaultAsync(e => e.Id == id);
            if (await TryUpdateModelAsync<TextEntry>(
                textEntryToUpdate,
                "",
                e => e.Content))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(textEntryToUpdate);
        }

        // GET: TextEntries1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var textEntry = await _context.TextEntries
                .Include(t => t.CreatedByUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (textEntry == null)
            {
                return NotFound();
            }

            return View(textEntry);
        }

        // POST: TextEntries1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var textEntry = await _context.TextEntries.SingleOrDefaultAsync(m => m.Id == id);
            _context.TextEntries.Remove(textEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextEntryExists(Guid id)
        {
            return _context.TextEntries.Any(e => e.Id == id);
        }
    }
}
