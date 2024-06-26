using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_gestor_de_biblioteca.Models;

namespace Sistema_gestor_de_biblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        //GET: Prestamos
        public async Task<IActionResult> Index()
        {
            var bibliotecaContext = _context.Prestamos.Include(p => p.Libro).Include(p => p.Usuario);
            return View(await bibliotecaContext.ToListAsync());
        }

        //// GET: Prestamos/Historial
        //public async Task<IActionResult> Historial()
        //{
        //    var historiales = await _context.Historiales
        //        .Include(h => h.Libro)
        //        .Include(h => h.Usuario)
        //        .ToListAsync();

        //    return View(historiales);
        //}
        //// GET: Prestamos/HistorialPorUsuario/5
        //public async Task<IActionResult> HistorialPorUsuario(int usuarioId)
        //{
        //    var historiales = _context.Historiales
        //        .Include(h => h.Libro)
        //        .Include(h => h.Usuario)
        //        .Where(h => h.UsuarioId == usuarioId);

        //    return View(await historiales.ToListAsync());
        //}

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,LibroId,FechaPrestamo,FechaDevolucion")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", prestamo.UsuarioId);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,LibroId,FechaPrestamo,FechaDevolucion")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.LibroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", prestamo.UsuarioId);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El préstamo ha sido eliminado con éxito.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var libro = await _context.Libros.FindAsync(prestamo.LibroId);
            if (libro != null)
            {
                libro.EstaReservado = false;
                _context.Update(libro);
            }

            _context.Prestamos.Remove(prestamo);

            // Registrar la acción en el historial
            var historial = new Historial
            {
                UsuarioId = prestamo.UsuarioId,
                LibroId = prestamo.LibroId,
                Accion = "Devolver",
                FechaAccion = DateTime.Now
            };
            _context.Historiales.Add(historial);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "El libro ha sido devuelto con éxito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renovar(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null || prestamo.Renovado)
            {
                TempData["ErrorMessage"] = "Préstamo no encontrado o ya renovado.";
                return RedirectToAction(nameof(Index));
            }

            prestamo.FechaDevolucion = prestamo.FechaDevolucion.AddDays(7); // Añadir 7 días a la fecha de devolución
            prestamo.Renovado = true;
            _context.Update(prestamo);

            // Registrar la acción en el historial
            var historial = new Historial
            {
                UsuarioId = prestamo.UsuarioId,
                LibroId = prestamo.LibroId,
                Accion = "Renovar",
                FechaAccion = DateTime.Now
            };
            _context.Historiales.Add(historial);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "El préstamo ha sido renovado con éxito.";
            return RedirectToAction(nameof(Index));
        }

        //Historial de Préstamos
        public async Task<IActionResult> Historial(int usuarioId)
        {
            var historiales = _context.Historiales
                .Include(h => h.Libro)
                .Include(h => h.Usuario)
                .Where(h => h.UsuarioId == usuarioId);

            return View(await historiales.ToListAsync());
        }
    }
}
