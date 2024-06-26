using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_gestor_de_biblioteca.Models;
using Sistema_gestor_de_biblioteca.Servicios;

namespace Sistema_gestor_de_biblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;


        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index(string filtro)
        {
            var libros = from l in _context.Libros
                         select l;

            if (!string.IsNullOrEmpty(filtro))
            {
                if (filtro == "disponibles")
                {
                    libros = libros.Where(l => !l.EstaReservado);
                }
                else if (filtro == "no-disponibles")
                {
                    libros = libros.Where(l => l.EstaReservado);
                }
            }

            var usuarios = await _context.Usuarios.ToListAsync();
            ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nombre");
            ViewBag.Filtro = filtro;
            return View(await libros.ToListAsync());
        }
        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Autor,ISBN,FechaPublicacion,Genero,CantidadDisponible")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El libro ha sido agregado con éxito.";
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Autor,ISBN,FechaPublicacion,Genero")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
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
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "El libro ha sido eliminado con éxito.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }

        public IActionResult Test()
        {
            return Content("Test method is working");
        }


        // Método para la búsqueda
        public async Task<IActionResult> Search(string searchString)
        {
            var libros = from l in _context.Libros
                         select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(s => s.Titulo.Contains(searchString) || s.Autor.Contains(searchString) || s.Genero.Contains(searchString));
            }

            return View("Index", await libros.ToListAsync()); // Reutiliza la vista Index
        }

        // Método para la reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(int id, int? usuarioId)
        {
            if (usuarioId == null)
            {
                TempData["ErrorMessage"] = "Por favor, seleccione un usuario para reservar.";
                return RedirectToAction(nameof(Index));
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                TempData["ErrorMessage"] = "El libro no existe.";
                return RedirectToAction(nameof(Index));
            }

            if (libro.EstaReservado)
            {
                TempData["ErrorMessage"] = "El libro ya está reservado.";
                return RedirectToAction(nameof(Index));
            }

            libro.EstaReservado = true;
            _context.Update(libro);

            var prestamo = new Prestamo
            {
                UsuarioId = usuarioId.Value,
                LibroId = id,
                FechaPrestamo = DateTime.Now,
                FechaDevolucion = DateTime.Now.AddDays(14)
            };
            _context.Prestamos.Add(prestamo);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "El libro ha sido reservado con éxito.";
            return RedirectToAction(nameof(Index));
        }

    }
}
