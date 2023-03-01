using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppNt.Models;
using RankingEquipos.Context;
using Microsoft.AspNetCore.Authorization;

namespace AppNt.Controllers
{
    public class LeagueController : Controller
    {
        private readonly RankingDataBaseContext _context;

        public LeagueController(RankingDataBaseContext context)
        {
            _context = context;
        }

        // GET: League



        [Authorize(Roles = "VOTANTE")]
        public IActionResult IndexUnaConfederacion(int id)
        {
            TempData["indexLeague"] = id;
            var leagues = _context.Leagues.Where(a => a.Confederation.Id == id).ToList();
            return View(leagues);
        }

        // GET: League
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Index()
        {
            var rankingDataBaseContext = _context.Leagues.Include(a => a.Confederation);
            return View(await rankingDataBaseContext.ToListAsync());
        }

        // GET: League/Details/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues
                .Include(a => a.Confederation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (league == null)
            {
                return NotFound();
            }

            return View(league);
        }

        // GET: League/Create
        [Authorize(Roles = "ADMINISTRADOR")]
        public IActionResult Create()
        {
            ViewData["ConfederationId"] = new SelectList(_context.Confederations, "Id", "Name");
            return View();
        }

        // POST: League/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ConfederationId, Confederation")] League league)
        {
            if (ModelState.IsValid)
            {
                _context.Add(league);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConfederationId"] = new SelectList(_context.Confederations, "Id", "Name", league.ConfederationId);
            return View(league);
        }

        // GET: League/Edit/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }
            ViewData["ConfederationId"] = new SelectList(_context.Confederations, "Id", "Name", league.ConfederationId);
            return View(league);
        }

        // POST: League/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ConfederationId")] League league)
        {
            if (id != league.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(league);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeagueExists(league.Id))
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
            ViewData["ConfederationId"] = new SelectList(_context.Confederations, "Id", "Name", league.ConfederationId);
            return View(league);
        }

        // GET: League/Delete/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues
                .Include(a => a.Confederation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (league == null)
            {
                return NotFound();
            }

            return View(league);
        }

        // POST: League/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var league = await _context.Leagues.FindAsync(id);
            _context.Leagues.Remove(league);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeagueExists(int id)
        {
            return _context.Leagues.Any(e => e.Id == id);
        }
    }
}
