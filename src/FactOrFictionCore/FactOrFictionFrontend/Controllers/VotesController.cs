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

namespace FactOrFictionFrontend.Controllers
{
    public class VotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Type,SentenceId")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                vote.Timestamp = DateTime.Now;
                vote.UserId = _userManager.GetUserId(User);
                var sentence = await _context.Sentences.SingleOrDefaultAsync(s => s.Id == vote.SentenceId);

                if (sentence != null)
                {
                    _context.Add(vote);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        type = vote.Type,
                        sentenceId = vote.SentenceId,
                        timestamp = vote.Timestamp
                    });
                }
            }
            return NotFound("Sentence not found.");
        }
    }
}
