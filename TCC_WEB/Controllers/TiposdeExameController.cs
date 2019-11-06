using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TCC_WEB.Data;
using TCC_WEB.Models;

namespace TCC_WEB.Controllers
{
    [Authorize]
    [Authorize(Roles = "None")]
    public class TiposdeExameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposdeExameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposdeExame
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipodeExame.ToListAsync());
        }

        // GET: TiposdeExame/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeExame = await _context.TipodeExame
                .FirstOrDefaultAsync(m => m.TipodeExameId == id);
            if (tipodeExame == null)
            {
                return NotFound();
            }

            return View(tipodeExame);
        }

        // GET: TiposdeExame/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposdeExame/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipodeExameId,NomedoExame")] TipodeExame tipodeExame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipodeExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipodeExame);
        }

        // GET: TiposdeExame/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeExame = await _context.TipodeExame.FindAsync(id);
            if (tipodeExame == null)
            {
                return NotFound();
            }
            return View(tipodeExame);
        }

        // POST: TiposdeExame/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipodeExameId,NomedoExame")] TipodeExame tipodeExame)
        {
            if (id != tipodeExame.TipodeExameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipodeExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipodeExameExists(tipodeExame.TipodeExameId))
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
            return View(tipodeExame);
        }

        // GET: TiposdeExame/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeExame = await _context.TipodeExame
                .FirstOrDefaultAsync(m => m.TipodeExameId == id);
            if (tipodeExame == null)
            {
                return NotFound();
            }

            return View(tipodeExame);
        }

        // POST: TiposdeExame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipodeExame = await _context.TipodeExame.FindAsync(id);
            _context.TipodeExame.Remove(tipodeExame);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipodeExameExists(int id)
        {
            return _context.TipodeExame.Any(e => e.TipodeExameId == id);
        }
    }
}
