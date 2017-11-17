using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactOrFictionCommon.Models;
using FactOrFictionFrontend.Data;
using Microsoft.AspNetCore.Identity;
using FactOrFictionFrontend.Controllers.Utils;
using FactOrFictionUrlSuggestions;
using Microsoft.Extensions.Logging;

namespace FactOrFictionFrontend.Controllers
{
    public class SentencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        private const int PAGE_SIZE = 5;

        public SentencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sentences/Feed
        public async Task<IActionResult> Feed(Guid? id, int page)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Cannot find user with ID '{_userManager.GetUserId(User)}'.");
            }
            DateTime timestamp = new DateTime();
            if (!id.HasValue || id.Value == Guid.Empty || id.Value.Equals(""))
            {
                timestamp = DateTime.Now;
            }
            else
            {
                var originalSentence = await _context.Sentences
                    .Include(s => s.OriginalTextEntry)
                    .SingleOrDefaultAsync(s => s.Id == id.Value);

                if (originalSentence == null)
                {
                    return NotFound();
                }
                timestamp = originalSentence.OriginalTextEntry.CreatedAt;
            }

            // select the next 10 sentences that were submitted before this timestamp
            var sentenceQuery = (
                from entry in _context.Sentences
                orderby entry.OriginalTextEntry.CreatedAt descending
                where entry.Type == SentenceType.OBJECTIVE
                where entry.OriginalTextEntry.CreatedAt.CompareTo(timestamp) < 0
                select entry
            );

            var sentenceViewModelQuery = sentenceQuery
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Select(sent => new SentenceViewModel(sent));

            return Json(new
            {
                Sentences = await sentenceViewModelQuery.ToListAsync()
            });   
        }

        // Get: Sentences/Details
        public async Task<IActionResult> Details(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var _sent = _context.Sentences.SingleOrDefault(s => s.Id == Id);
            if (_sent == null)
            {
                return NotFound();
            }
            if (_sent.Type != SentenceType.OBJECTIVE)
            {
                return NotFound();
            }
            var finder = FinderFactory.CreateFinder();
            var urlClassifier = new URLClassification();
            var referenceTasks = (await finder.FindSuggestions(_sent.Content))
                .Select(async uri =>
                 {
                     var bias = await urlClassifier.ClassifyOutletDescription(uri.Host);
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
            var entityFinder = new EntityFinder();
            var entityList = (await entityFinder.GetEntities(_sent.Content))
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
                }).ToList();
            var entityTasks = entityList
                .Select(async e =>
                {
                    e.Persona = PersonasDBLookups.ByName[e.Name].FirstOrDefault();
                    if (e.Persona != null)
                    {
                        await e.Persona.FetchRecentStatements();
                    }
                    return e;
                });
            var entities = await Task.WhenAll(entityTasks);

            return Json(new
            {
                Refererences = references,
                Entites = entities
            });
        }
    }
}
