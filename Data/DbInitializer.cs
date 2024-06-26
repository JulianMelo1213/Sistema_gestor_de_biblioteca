using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sistema_gestor_de_biblioteca.Models;
using System;
using System.Linq;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new BibliotecaContext(
            serviceProvider.GetRequiredService<DbContextOptions<BibliotecaContext>>()))
        {
            // Verificar si hay libros existentes.
            if (!context.Libros.Any())
            {
                // Si no hay libros, agregar los iniciales.
                var libros = new Libro[]
                {
                    new Libro{Titulo="El Quijote",Autor="Miguel de Cervantes",ISBN="1234567890",FechaPublicacion=DateTime.Parse("1605-01-16"),Genero="Novela"},
                    new Libro{Titulo="Cien Años de Soledad",Autor="Gabriel García Márquez",ISBN="2345678901",FechaPublicacion=DateTime.Parse("1967-05-30"),Genero="Realismo Mágico"},
                    new Libro{Titulo="Don Juan Tenorio",Autor="José Zorrilla",ISBN="3456789012",FechaPublicacion=DateTime.Parse("1844-03-28"),Genero="Drama"}
                    // Agrega más libros si es necesario
                };

                foreach (Libro l in libros)
                {
                    context.Libros.Add(l);
                }
                context.SaveChanges();
            }

            // Aquí puedes agregar lógica para verificar si necesitas agregar más libros en el futuro.
            // Por ejemplo, podrías buscar un libro específico y si no existe, añadirlo.

            if (!context.Libros.Any(l => l.ISBN == "123132136"))
            {
                var libroNuevo = new Libro { Titulo = "Prueba22", Autor = "Prueba22", ISBN = "123132136", FechaPublicacion = DateTime.Parse("1668-01-01"), Genero = "prueba" };
                context.Libros.Add(libroNuevo);
                context.SaveChanges();
            }

            // Verificar si hay usuarios existentes.
            if (!context.Usuarios.Any())
            {
                // Si no hay usuarios, agregar los iniciales.
                var usuarios = new Usuario[]
                {
                    new Usuario{Nombre="Juan",Apellido="Pérez",Email="juan.perez@example.com",FechaRegistro=DateTime.Parse("2023-01-01")},
                    new Usuario{Nombre="María",Apellido="López",Email="maria.lopez@example.com",FechaRegistro=DateTime.Parse("2023-02-01")}
                    // Agrega más usuarios si es necesario
                };

                foreach (Usuario u in usuarios)
                {
                    context.Usuarios.Add(u);
                }
                context.SaveChanges();
            }

            // Verificar si hay préstamos existentes.
            //if (!context.Prestamos.Any())
            //{
            //    // Si no hay préstamos, agregar los iniciales.
            //    var prestamos = new Prestamo[]
            //    {
            //        new Prestamo{UsuarioId=1,LibroId=1,FechaPrestamo=DateTime.Now,FechaDevolucion=DateTime.Now.AddDays(14)},
            //        new Prestamo{UsuarioId=2,LibroId=2,FechaPrestamo=DateTime.Now,FechaDevolucion=DateTime.Now.AddDays(14)},

            //        // Agrega más préstamos si es necesario
            //    };

            //    foreach (Prestamo p in prestamos)
            //    {
            //        context.Prestamos.Add(p);
            //    }
            //    context.SaveChanges();
            //}
        }
    }
}
