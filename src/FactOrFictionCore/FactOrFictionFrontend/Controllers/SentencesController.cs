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

namespace FactOrFictionFrontend.Controllers
{
    public class SentencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SentencesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sentences/Feed
        public async Task<IActionResult> Feed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException(
                    $"Cannot find user with ID '{_userManager.GetUserId(User)}'.");
            }

            var textEntries = (
                from entry in _context.Sentences
                orderby entry.OriginalTextEntry.CreatedAt descending
                where entry.Type == SentenceType.OBJECTIVE
                select entry
            )
            .Take(10)
            .Select(sent => new SentenceViewModel(sent));

            return Json(new
            {
                Sentences = await textEntries.ToListAsync()
            });
        }

        // GET: Sentences
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sentences.Include(s => s.OriginalTextEntry);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sentences/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentence = await _context.Sentences
                .Include(s => s.OriginalTextEntry)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sentence == null)
            {
                return NotFound();
            }

            return View(sentence);
        }

        // GET: Sentences/Create
        public IActionResult Create()
        {
            ViewData["TextEntryId"] = new SelectList(_context.TextEntries, "Id", "Id");
            return View();
        }

        // POST: Sentences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,VoteTrue,VoteFalse,Position,Confidence,Type,TextEntryId")] Sentence sentence)
        {
            if (ModelState.IsValid)
            {
                sentence.Id = Guid.NewGuid();
                _context.Add(sentence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TextEntryId"] = new SelectList(_context.TextEntries, "Id", "Id", sentence.TextEntryId);
            return View(sentence);
        }

        // GET: Sentences/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentence = await _context.Sentences.SingleOrDefaultAsync(m => m.Id == id);
            if (sentence == null)
            {
                return NotFound();
            }
            ViewData["TextEntryId"] = new SelectList(_context.TextEntries, "Id", "Id", sentence.TextEntryId);
            return View(sentence);
        }

        // POST: Sentences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Content,VoteTrue,VoteFalse,Position,Confidence,Type,TextEntryId")] Sentence sentence)
        {
            if (id != sentence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sentence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SentenceExists(sentence.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TextEntryId"] = new SelectList(_context.TextEntries, "Id", "Id", sentence.TextEntryId);
            return View(sentence);
        }

        // GET: Sentences/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentence = await _context.Sentences
                .Include(s => s.OriginalTextEntry)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sentence == null)
            {
                return NotFound();
            }

            return View(sentence);
        }

        // POST: Sentences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sentence = await _context.Sentences.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sentences.Remove(sentence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SentenceExists(Guid id)
        {
            return _context.Sentences.Any(e => e.Id == id);
        }
    }
}
