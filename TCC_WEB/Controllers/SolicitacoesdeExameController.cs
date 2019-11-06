using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TCC_WEB.Data;
using TCC_WEB.Models;

namespace TCC_WEB.Controllers
{
    public class SolicitacoesdeExameController : Controller
    {
        private readonly UserManager<Usuario> _gerenciadorUsuarios;
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly RoleManager<NivelAcesso> _roleManager;
        private readonly ApplicationDbContext _context;

        public SolicitacoesdeExameController(UserManager<Usuario> gerenciadorUsuarios, SignInManager<Usuario> gerenciadorLogin, RoleManager<NivelAcesso> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _gerenciadorUsuarios = gerenciadorUsuarios;
            _gerenciadorLogin = gerenciadorLogin;
            _context = context;
        }


        // GET: SolicitacoesdeExame
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SolicitacoesdeExame.Include(s => s.Paciente).Include(s => s.TipodeExame);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Index(string txtProcurar)
        {
            if (!String.IsNullOrEmpty(txtProcurar))
                return View(await _context.SolicitacoesdeExame.Where(td => (td.Paciente.Nome.ToUpper() + " " + td.Médico.ToUpper() + " " + td.TipodeExame.NomedoExame.ToUpper()).Contains(txtProcurar.ToUpper())).
                    Include(r => r.TipodeExame).Include(r => r.Paciente).ToListAsync());

            return View(await _context.SolicitacoesdeExame.Include(r => r.TipodeExame).Include(r => r.Paciente).ToListAsync());
        }

        // GET: SolicitacoesdeExame/Details/5
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaodeExame = await _context.SolicitacoesdeExame
                .Include(s => s.Paciente)
                .Include(s => s.TipodeExame)
                .FirstOrDefaultAsync(m => m.SolicitacaodeExameId == id);
            if (solicitacaodeExame == null)
            {
                return NotFound();
            }

            return View(solicitacaodeExame);
        }

        // GET: SolicitacoesdeExame/Create
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome");
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame");
            var userid = _gerenciadorUsuarios.GetUserId(HttpContext.User);
            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Usuario user = _gerenciadorUsuarios.FindByIdAsync(userid).Result;
            ViewData["UsuarioId"] = user.Nome.ToString() + " " + user.SobreNome.ToString();
            return View();
        }

        // POST: SolicitacoesdeExame/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Create([Bind("SolicitacaodeExameId,Data,TipodeExameId,Médico,PacienteId")] SolicitacaodeExame solicitacaodeExame)
        {
            var userid = _gerenciadorUsuarios.GetUserId(HttpContext.User);
            Usuario user = _gerenciadorUsuarios.FindByIdAsync(userid).Result;
            string nome = user.Nome.ToString() + " " + user.SobreNome.ToString();

            if (ModelState.IsValid && solicitacaodeExame.Médico == nome)
            {
                _context.Add(solicitacaodeExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", solicitacaodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", solicitacaodeExame.TipodeExameId);
           
            return View(solicitacaodeExame);
        }

        // GET: SolicitacoesdeExame/Edit/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaodeExame = await _context.SolicitacoesdeExame.FindAsync(id);
            if (solicitacaodeExame == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", solicitacaodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", solicitacaodeExame.TipodeExameId);
            return View(solicitacaodeExame);
        }

        // POST: SolicitacoesdeExame/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int id, [Bind("SolicitacaodeExameId,Data,TipodeExameId,Médico,PacienteId")] SolicitacaodeExame solicitacaodeExame)
        {
            if (id != solicitacaodeExame.SolicitacaodeExameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitacaodeExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitacaodeExameExists(solicitacaodeExame.SolicitacaodeExameId))
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", solicitacaodeExame.PacienteId);
            ViewData["TipodeExameId"] = new SelectList(_context.TiposdeExame, "TipodeExameId", "NomedoExame", solicitacaodeExame.TipodeExameId);
            return View(solicitacaodeExame);
        }

        // GET: SolicitacoesdeExame/Delete/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaodeExame = await _context.SolicitacoesdeExame
                .Include(s => s.Paciente)
                .Include(s => s.TipodeExame)
                .FirstOrDefaultAsync(m => m.SolicitacaodeExameId == id);
            if (solicitacaodeExame == null)
            {
                return NotFound();
            }

            return View(solicitacaodeExame);
        }

        // POST: SolicitacoesdeExame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitacaodeExame = await _context.SolicitacoesdeExame.FindAsync(id);
            _context.SolicitacoesdeExame.Remove(solicitacaodeExame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitacaodeExameExists(int id)
        {
            return _context.SolicitacoesdeExame.Any(e => e.SolicitacaodeExameId == id);
        }
    }
}
