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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AppNt.Controllers
{
    public class UserController : Controller
    {
        private readonly RankingDataBaseContext _context;

        public UserController(RankingDataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpGet]
        public IActionResult IniciarSesion(string returnUrl)
        {
            TempData["UrlIngreso"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(User usuario)
        {
            var user = _context.Users.Where(m => m.Email == usuario.Email && m.Password == usuario.Password).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorEnLogin = "Verifique el usuario y contraseña";
                return View();
            }

            ClaimsIdentity identidad = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // Agregamos a la credencial el nombre de usuario
            identidad.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            // Agregamos a la credencial el nombre del estudiante/administrador
            identidad.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            // Agregamos a la credencial el Rol
            identidad.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

            ClaimsPrincipal principal = new ClaimsPrincipal(identidad);

            // Ejecutamos el Login
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            

            if (user.Role == Role.ADMINISTRADOR) {
                return Redirect("/Confederation/Index");
            }
            

            return Redirect("/Confederation/IndexForUsers");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Salir()
        {
          
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IdentificationNumber,Password,Lastname,Email,Age")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                ClaimsIdentity identidad = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                // Agregamos a la credencial el nombre de usuario
                identidad.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                // Agregamos a la credencial el nombre del estudiante/administrador
                identidad.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
                // Agregamos a la credencial el Rol
                identidad.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

                ClaimsPrincipal principal = new ClaimsPrincipal(identidad);

                // Ejecutamos el Login
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                if (user.Role == Role.ADMINISTRADOR)
                {
                    return Redirect("/Confederation/Index");
                }


                return Redirect("/Confederation/IndexForUsers"); ;
            }
            return View(user);
        }

        [Authorize]
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IdentificationNumber,Password,Lastname,Email,Age,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
