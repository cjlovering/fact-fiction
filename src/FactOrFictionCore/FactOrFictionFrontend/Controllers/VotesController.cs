using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactOrFictionCommon.Models.RelationshipModels;
using FactOrFictionFrontend.Data;
using Microsoft.AspNetCore.Identity;
using FactOrFictionCommon.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using FactOrFictionFrontend.Controllers.Utils;

namespace FactOrFictionFrontend.Controllers
{
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public VotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<VotesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // POST: Votes/Cast
        [HttpPost]
        public async Task<IActionResult> Cast([Bind("Type, SentenceId")] Vote vote)
        {
            _logger.LogInformation(String.Format("The timestamp is {0}", vote.Timestamp));
            if (ModelState.IsValid)
            {
                vote.Timestamp = DateTime.Now;
                vote.UserId = _userManager.GetUserId(User);
                var oldVote = await _context.Votes
                    .Include(v => v.Sentence)
                    .SingleOrDefaultAsync(v => v.UserId == vote.UserId && v.SentenceId == vote.SentenceId);

                // when the vote already exists in the database and if the type is the same, remove from the database
                if (oldVote != null && oldVote.Type == vote.Type)
                { 
                    if (vote.Type == VoteType.TRUE)
                    {
                        oldVote.Sentence.VoteTrue--;
                    }
                    else
                    {
                        oldVote.Sentence.VoteFalse--;
                    }

                    _context.Votes.Remove(oldVote);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation(String.Format("delele vote {0}", oldVote.ToString()));

                    var VotesDict = new Dictionary<Guid, string>();
                    VotesDict.Add(oldVote.SentenceId, VoteType.UNVOTED.ToString());
  
                    return Json(new
                    {
                        Sentences = new SentenceViewModel[] { new SentenceViewModel(oldVote.Sentence) },
                        Votes = VotesDict, 
                    });
                }
                // when the user wants to switch vote from TRUE to FALSE or vice versa: delete the previous vote, add new vote
                else if (oldVote != null && oldVote.Type != vote.Type)
                {

                    var VotesDict = new Dictionary<Guid, string>();

                    if (vote.Type == VoteType.TRUE)
                    {
                        oldVote.Sentence.VoteTrue++;
                        oldVote.Sentence.VoteFalse--;
                        VotesDict.Add(oldVote.SentenceId, VoteType.TRUE.ToString());
                    }
                    else
                    {
                        oldVote.Sentence.VoteTrue--;
                        oldVote.Sentence.VoteFalse++;
                        VotesDict.Add(oldVote.SentenceId, VoteType.FALSE.ToString());
                    }
                    oldVote.Type = vote.Type;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation(String.Format("delele vote {0} and added new vote {1}", oldVote.ToString(), vote.ToString()));


                    return Json(new
                    {
                        Sentences = new SentenceViewModel[] { new SentenceViewModel(oldVote.Sentence) },
                        Votes = VotesDict,
                    });
                }
                else
                {
                    var matchingSentence = await _context.Sentences.SingleOrDefaultAsync(s => s.Id == vote.SentenceId);

                    var VotesDict = new Dictionary<Guid, string>();
                    vote.Sentence = matchingSentence;
                    // vote in a new sentence
                    if (vote.Type == VoteType.TRUE)
                    {
                        matchingSentence.VoteTrue++;
                        VotesDict.Add(vote.SentenceId, VoteType.TRUE.ToString());
                    }
                    else
                    {
                        matchingSentence.VoteFalse++;
                        VotesDict.Add(vote.SentenceId, VoteType.FALSE.ToString());
                    }

                    _context.Votes.Add(vote);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation(String.Format("added vote {0}", vote.ToString()));
                    return Json(new
                    {
                        Sentences = new SentenceViewModel[] { new SentenceViewModel(matchingSentence) },
                        Votes = VotesDict,
                    });
                }
            }
            return NotFound("Model State not valid");

        }

    }
}
