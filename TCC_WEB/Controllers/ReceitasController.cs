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
    [Authorize]
    public class ReceitasController : Controller
    {
        private readonly UserManager<Usuario> _gerenciadorUsuarios;
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly RoleManager<NivelAcesso> _roleManager;
        private readonly ApplicationDbContext _context;

        public ReceitasController(UserManager<Usuario> gerenciadorUsuarios, SignInManager<Usuario> gerenciadorLogin, RoleManager<NivelAcesso> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _gerenciadorUsuarios = gerenciadorUsuarios;
            _gerenciadorLogin = gerenciadorLogin;
            _context = context;
        }

        // GET: Receitas
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Receitas.Include(r => r.Medicamento).Include(r => r.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        [HttpPost]
        public async Task<IActionResult> Index(string txtProcurar)
        {
            if (!String.IsNullOrEmpty(txtProcurar))
                return View(await _context.Receitas.Where(td => (td.Paciente.Nome.ToUpper() + " " + td.Médico.ToUpper()+" "+td.Medicamento.NomeGenerico).Contains(txtProcurar.ToUpper())).
                    Include(r => r.Medicamento).Include(r => r.Paciente).ToListAsync());

            var applicationDbContext = _context.Receitas.Include(r => r.Medicamento).Include(r => r.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Receitas/Details/5
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Medicamento)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.ReceitaId == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // GET: Receitas/Create
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public IActionResult Create()
        {
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "NomeGenerico");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome");
            var userid = _gerenciadorUsuarios.GetUserId(HttpContext.User);
            if (userid == null)
            {
                return RedirectToAction("Login","Account");
            }
            Usuario user = _gerenciadorUsuarios.FindByIdAsync(userid).Result;
            ViewData["UsuarioId"] = user.Nome.ToString()+" "+user.SobreNome.ToString() ;
            return View();
        }

        // POST: Receitas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Create([Bind("ReceitaId,Data,MedicamentoId,Dose,PacienteId,Médico")] Receita receita)
        {
            var userid = _gerenciadorUsuarios.GetUserId(HttpContext.User);
            Usuario user = _gerenciadorUsuarios.FindByIdAsync(userid).Result;
            string nome= user.Nome.ToString() + " " + user.SobreNome.ToString();
            
            if (ModelState.IsValid&&receita.Médico==nome)
            {
                _context.Add(receita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = user.Nome.ToString() + " " + user.SobreNome.ToString();
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "NomeGenerico", receita.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", receita.PacienteId);
            return View(receita);
        }

        // GET: Receitas/Edit/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "NomeGenerico", receita.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", receita.PacienteId);
            return View(receita);
        }

        // POST: Receitas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Edit(int id, [Bind("ReceitaId,Data,MedicamentoId,Dose,PacienteId,Médico")] Receita receita)
        {
            if (id != receita.ReceitaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaExists(receita.ReceitaId))
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
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "NomeGenerico", receita.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", receita.PacienteId);
            return View(receita);
        }

        // GET: Receitas/Delete/5
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Medicamento)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.ReceitaId == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // POST: Receitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "Administrador,Médico")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            _context.Receitas.Remove(receita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceitaExists(int id)
        {
            return _context.Receitas.Any(e => e.ReceitaId == id);
        }
    }
}
