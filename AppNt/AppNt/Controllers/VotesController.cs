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
using System.Security.Claims;

namespace AppNt.Controllers
{
    public class VotesController : Controller
    {
        private readonly RankingDataBaseContext _context;

        public VotesController(RankingDataBaseContext context)
        {
            _context = context;
        }



        public IActionResult mostrarRanking()
        {
            int id = (int)TempData["indexLeague"];

            var votos = _context.Votes.Include(x => x.Team).Where(x => x.valueVote == true).Where(x => x.Team.LeagueId == (int)TempData["indexLeague"]).ToList();
            var agruparPorTeam = votos.GroupBy(x => x.Team).ToList();
            var mayores = agruparPorTeam.OrderByDescending(x => x.Key.Vote.Count);
            int pos = 1;
            var listaVotosTeam = new List<TeamVote>();
            foreach (var item in mayores)
            {
                listaVotosTeam.Add(new TeamVote
                {
                    Id = pos,
                    Name = item.Key.Name,
                    QtyVotes = item.Key.Vote.Count()
                });

                pos++;
            }
            /*
             
            var votosProfesor = mayores.Select(x => new VotoProfesor {
                    Name = x.Key.Name,
                    QtyVotes = x.Key.Vote.Count(),

                })
                .ToList();
            */
            TempData["indexLeague"] = id;

            return View(listaVotosTeam);
        }

        // GET: Votes
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Index()
        {
            var rankingDataBaseContext = _context.Votes.Include(v => v.Team).Include(v => v.User);
            return View(await rankingDataBaseContext.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Team)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        [Authorize(Roles = "VOTANTE")]
        // GET: Votes/Create
        public IActionResult Create()
        {
            int id = (int)TempData["indexLeague"];

            var claimsUser = User.Claims;
            var usernameClaim = claimsUser.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value;

            ViewData["TeamId"] = new SelectList(_context.Teams.Where(x => x.LeagueId== id), "Id", "Name");
            // ViewData["ProfesorId"] = new SelectList(_context.Profesors.Where(x=> x.AsignatureId == i), "Id", "Lastname");
            // ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.Email == usernameClaim), "Id", "Email").FirstOrDefault();
            //  ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            TempData["indexLeague"] = id;
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [Authorize(Roles = "USUARIO")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TeamId,valueVote")] Vote vote)
        {

            var claimsUser = User.Claims;
            var usernameClaim = claimsUser.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value;

            User user = _context.Users.Where(x => x.Email == usernameClaim).FirstOrDefault();
            int userId = user.Id;
            vote.User = user;
            vote.UserId = userId;
            vote.valueVote = true;
            if (ModelState.IsValid)
            {
                _context.Add(vote);
                await _context.SaveChangesAsync();

                return RedirectToAction("IndexForUsers", "Confederation");
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "name", vote.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", vote.UserId);
            return Redirect("/Confederation/IndexForUsers");
        }

        // GET: Votes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", vote.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", vote.UserId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TeamId,valueVote")] Vote vote)
        {
            if (id != vote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", vote.TeamId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", vote.UserId);
            return View(vote);
        }

        [Authorize(Roles = "USUARIO")]
        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Team)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.Id == id);
        }
    }
}
