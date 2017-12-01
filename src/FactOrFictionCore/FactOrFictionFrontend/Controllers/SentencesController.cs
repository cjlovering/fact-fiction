using FactOrFictionCommon.Models;
using FactOrFictionCommon.Models.RelationshipModels;
using FactOrFictionCommon.Models.SentenceViewModels;
using FactOrFictionFrontend.Data;
using FactOrFictionTextHandling.InferSentClient;
using FactOrFictionUrlSuggestions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FactOrFictionFrontend.Controllers
{
    public class SentencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        private const int PAGE_SIZE = 5;

        public SentencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
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

            var userId = _userManager.GetUserId(User);

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

            var sentences = await sentenceViewModelQuery.ToListAsync();

            var previousVotes = await _context.Votes.Where(v => v.UserId == userId).ToDictionaryAsync(v => v.SentenceId, v => v.Type);

            var VotesDict = sentences.ToDictionary(
                sent => sent.Id,
                sent =>
                {
                    if (previousVotes.ContainsKey(sent.Id))
                    {
                        return previousVotes[sent.Id].ToString();
                    }
                    else
                    {
                        return VoteType.UNVOTED.ToString();
                    }
                }
                );

            return Json(new
            {
                Sentences = sentences,
                Votes = VotesDict
            });
        }

        // GET: Sentences/Details
        public async Task<IActionResult> Details(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var _sent = _context.Sentences
                                .SingleOrDefault(s => (s.Id == Id && s.Type == SentenceType.OBJECTIVE));
            if (_sent == null)
            {
                return NotFound();
            }
            if (_sent.Type != SentenceType.OBJECTIVE)
            {
                return NotFound();
            }
            var factory = new FinderFactory(_configuration["Authentication:Finder:Key"]);
            var finder = factory.CreateFinder();

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

            var entityFinder = new EntityFinder(_configuration["Authentication:Entity:Key"]);

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
                References = references,
                Entities = entities
            });
        }

        // GET: Sentences/Related/Id
        public async Task<IActionResult> Related(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var _sent = await _context.Sentences
                                      .SingleOrDefaultAsync(s => s.Id == Id 
                                                              && s.Type == SentenceType.OBJECTIVE
                                                              && s.InferSentVectorsString != null);
            if (_sent == null)
            {
                return NotFound();
            }

            var inferSentVector = _sent.InferSentVectorsDouble;

            var relatedSentencesQuery = _context.Sentences
                .Where(candidate => candidate.InferSentVectorsString != null && candidate.Id != Id && candidate.TextEntryId != _sent.TextEntryId)
                .Select(candidate => new
                {
                    sentence = candidate,
                    distance = DistanceCalculator.CalculateCosineSimilarity(
                        _sent.InferSentVectorsDouble,
                        candidate.InferSentVectorsDouble) // distance = 1 if the two sentences are the same
                })
                .Where(_sentence => _sentence.distance < 1.0 && _sentence.distance > 0.7)
                .OrderByDescending(candidate => candidate.distance)
                .Take(5)
                .Select(_sentence => _sentence.sentence);

            List<Sentence> relatedSentences = await relatedSentencesQuery.ToListAsync();

            return Json(new
            {
                Sentences = relatedSentences.Select(
                        sent => new SentenceViewModel(sent)),
                SentenceId = Id
            });
        }
    }
}
