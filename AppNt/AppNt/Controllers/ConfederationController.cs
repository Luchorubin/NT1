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
    public class ConfederationController : Controller
    {
        private readonly RankingDataBaseContext _context;

        public ConfederationController(RankingDataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "VOTANTE")]
        public async Task<IActionResult> IndexForUsers()
        {
            return View(await _context.Confederations.ToListAsync());
        }

        // GET: Confederation
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Confederations.ToListAsync());
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        // GET: Confederation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Confederations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        // GET: Confederation/Create
        [Authorize(Roles = "ADMINISTRADOR")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Confederation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Confederation Confederation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Confederation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Confederation);
        }

        // GET: Confederation/Edit/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var confederation = await _context.Confederations.FindAsync(id);
            if (confederation == null)
            {
                return NotFound();
            }
            return View(confederation);
        }

        // POST: Confederation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Confederation confederation)
        {
            if (id != confederation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(confederation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfederationExists(confederation.Id))
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
            return View(confederation);
        }

        // GET: Confederation/Delete/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var confederation = await _context.Confederations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (confederation == null)
            {
                return NotFound();
            }

            return View(confederation);
        }

        // POST: Confederation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var confederation = await _context.Confederations.FindAsync(id);
            _context.Confederations.Remove(confederation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConfederationExists(int id)
        {
            return _context.Confederations.Any(e => e.Id == id);
        }
    }
}
