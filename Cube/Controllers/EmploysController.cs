using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cube.Datas;
using Cube.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cube.Controllers
{
    [Authorize]
    public class EmploysController : Controller
    {
        private readonly CubeProjetIndiContext _context;

        public EmploysController(CubeProjetIndiContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Employs
        public async Task<ActionResult> Index()
        {

            var cubeProjetIndiContext = _context.Employs.Include(e => e.Cities).Include(e => e.Service);         
            return View(await cubeProjetIndiContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<ActionResult> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return View("Index");

            var employs = _context.Employs.Include(e => e.Cities).Include(e => e.Service);

            var filteredList = employs.Where(x => x.FirstName.ToUpper().Contains(searchString.ToUpper()) || x.SurName.ToUpper().Contains(searchString.ToUpper()) || x.Cities.Name.ToUpper().Contains(searchString.ToUpper()) || x.Service.Name.ToUpper().Contains(searchString.ToUpper())).ToList();

            return View("Index", filteredList);
        }


        [AllowAnonymous]
        // GET: Employs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employs == null)
            {
                return NotFound();
            }

            var employs = await _context.Employs
                .Include(e => e.Cities)
                .Include(e => e.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employs == null)
            {
                return NotFound();
            }

            return View(employs);
        }

        // GET: Employs/Create
        public IActionResult Create()
        {
            ViewData["CitiesId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: Employs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,SurName,FixPhone,Phone,ServiceId,CitiesId,Admin,PassWord")] Employs employs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitiesId"] = new SelectList(_context.Cities, "Id", "Name", employs.CitiesId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", employs.ServiceId);
            return View(employs);
        }

        // GET: Employs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employs == null)
            {
                return NotFound();
            }

            var employs = await _context.Employs.FindAsync(id);
            if (employs == null)
            {
                return NotFound();
            }
            ViewData["CitiesId"] = new SelectList(_context.Cities, "Id", "Name", employs.CitiesId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", employs.ServiceId);
            return View(employs);
        }

        // POST: Employs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,SurName,FixPhone,Phone,ServiceId,CitiesId,Admin,PassWord")] Employs employs)
        {
            if (id != employs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploysExists(employs.Id))
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
            ViewData["CitiesId"] = new SelectList(_context.Cities, "Id", "Name", employs.CitiesId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", employs.ServiceId);
            return View(employs);
        }

        // GET: Employs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employs == null)
            {
                return NotFound();
            }

            var employs = await _context.Employs
                .Include(e => e.Cities)
                .Include(e => e.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employs == null)
            {
                return NotFound();
            }

            return View(employs);
        }

        // POST: Employs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employs == null)
            {
                return Problem("Entity set 'CubeProjetIndiContext.Employs'  is null.");
            }
            var employs = await _context.Employs.FindAsync(id);
            if (employs != null)
            {
                _context.Employs.Remove(employs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmploysExists(int id)
        {
          return _context.Employs.Any(e => e.Id == id);
        }
    }
}
