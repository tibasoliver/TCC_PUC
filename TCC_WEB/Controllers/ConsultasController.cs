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
    public class ConsultasController : Controller
    {
        private readonly UserManager<Usuario> _gerenciadorUsuarios;
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly RoleManager<NivelAcesso> _roleManager;
        private readonly ApplicationDbContext _context;

        public ConsultasController(UserManager<Usuario> gerenciadorUsuarios, SignInManager<Usuario> gerenciadorLogin, RoleManager<NivelAcesso> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _gerenciadorUsuarios = gerenciadorUsuarios;
            _gerenciadorLogin = gerenciadorLogin;
            _context = context;
        }

        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Consultas.Include(c => c.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        [HttpPost]
        public async Task<IActionResult> Index(string txtProcurar)
        {
            
            if (!String.IsNullOrEmpty(txtProcurar))
                return View(await _context.Consultas.Where(td => (td.Data.ToString("dd/MM/yyyy") + " " + td.Médico.ToUpper() + " " + td.Paciente.Nome.ToUpper()).Contains(txtProcurar.ToUpper())).
                    Include(r => r.Paciente).ToListAsync());

            var applicationDbContext = _context.Consultas.Include(c => c.Paciente);
            return View(await applicationDbContext.ToListAsync());
         
        }

        [Authorize]
        [Authorize(Roles = "Administrador,Atendente,Médico")]
        // GET: Consultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ConsultaId == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // GET: Consultas/Create
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome");
            ViewData["NomeMedico"] = new SelectList(await _gerenciadorUsuarios.GetUsersInRoleAsync("Médico"), "Nome", "Nome");
            return View();
        }

        // POST: Consultas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultaId,Data,PacienteId,Médico,horario")] Consulta consulta)
        {
            var igual = _context.Consultas.FirstOrDefault(p => (p.Data == consulta.Data)&&(p.Médico==consulta.Médico)&&(p.horario==consulta.horario));
            var igual1 = _context.Consultas.FirstOrDefault(p => (p.Data == consulta.Data) && (p.PacienteId == consulta.PacienteId) && (p.horario == consulta.horario));

            if (ModelState.IsValid&&igual==null&&igual1==null)
            {
                var datatxt = consulta.Data.ToString();
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            ViewData["NomeMedico"] = new SelectList(await _gerenciadorUsuarios.GetUsersInRoleAsync("Médico"), "Nome", "Nome");

            if(igual != null)
            ViewData["Message1"] = "Este médico já tem uma consulta marcada nesta data e horário";
            if (igual1 != null)
            ViewData["Message2"] = "Este paciente já tem uma consulta marcada nesta data e horário";
            return View(consulta);
        }

        // GET: Consultas/Edit/5
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            ViewData["NomeMedico"] = new SelectList(await _gerenciadorUsuarios.GetUsersInRoleAsync("Médico"), "Nome", "Nome");
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsultaId,Data,PacienteId,Médico,horario")] Consulta consulta)
        {
            if (id != consulta.ConsultaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.ConsultaId))
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "Nome", consulta.PacienteId);
            return View(consulta);
        }

        // GET: Consultas/Delete/5
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(m => m.ConsultaId == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // POST: Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [Authorize(Roles = "Administrador,Atendente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.ConsultaId == id);
        }
    }
}
