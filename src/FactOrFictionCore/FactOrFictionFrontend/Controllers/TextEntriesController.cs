using FactOrFictionCommon.Models;
using FactOrFictionCommon.Models.RelationshipModels;
using FactOrFictionFrontend.Controllers.Utils;
using FactOrFictionFrontend.Data;
using FactOrFictionTextHandling.InferSentClient;
using FactOrFictionTextHandling.MLClient;
using FactOrFictionTextHandling.Parser;
using FactOrFictionTextHandling.SentenceProducer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionFrontend.Controllers
{
    public class TextEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TextEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: TextEntries
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Cannot find user with ID '{_userManager.GetUserId(User)}'.");
            }

            bool isAdmin = await _userManager.IsInRoleAsync(user, ApplicationRole.ADMINISTRATOR);
            var textEntries = (
                from entry in _context.TextEntries.Include(t => t.CreatedByUser)
                where isAdmin || entry.UserId == user.Id
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
                ISentenceProducer sentenceProducer;
                if (_configuration["MLService:Type"] == "HACC")
                {
                    string HACC_URL = _configuration["MLService:Url"];
                    string HACC_KEY = _configuration["MLService:HACC:Key"];
                    HaccClient client = new HaccClient(HACC_URL, HACC_KEY);
                    sentenceProducer = new SentenceProducer<HaccResult>(client, client);
                }
                else
                {
                    string LUIS_KEY = _configuration["MLService:LUIS:Key"];
                    string LUIS_URL = _configuration["MLService:Url"].Replace("<KEY>", LUIS_KEY);
                    sentenceProducer = new SentenceProducer<LuisResult>(new LuisClient(LUIS_URL), new WorkingParser());
                }

                textEntry.Id = Guid.NewGuid();
                textEntry.UserId = _userManager.GetUserId(User);
                textEntry.CreatedAt = DateTime.Now;

                var parsingTask = await sentenceProducer.GetStatements(textEntry);

                var sentenceTasks = Task.WhenAll(parsingTask);
                var sentences = await sentenceTasks;

                string INFERSENT_URL = ""; // change these
                string INFERSENT_KEY = "";
                InferSentClient inferSentClient = new InferSentClient(INFERSENT_URL, INFERSENT_KEY);
                var sentencesWithRelated = await inferSentClient.ConnectInferSent(sentences);
                
                Array.Sort(sentencesWithRelated);

                _context.Add(textEntry);
                _context.AddRange(sentencesWithRelated);
                await _context.SaveChangesAsync();

                
                var VotesDict = sentencesWithRelated.ToDictionary(sent => sent.Id, sent => VoteType.UNVOTED.ToString());

                return Json(new
                {
                    Sentences = sentencesWithRelated.Select(
                        sent => new SentenceViewModel(sent)),
                    // Return Votes here
                    Votes = VotesDict
                });
            }
            return View(textEntry);
        }

        // GET: TextEntries1/Delete/5
        [Authorize(Roles = ApplicationRole.ADMINISTRATOR)]
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
        [Authorize(Roles = ApplicationRole.ADMINISTRATOR)]
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
