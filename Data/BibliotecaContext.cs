using Microsoft.EntityFrameworkCore;
using Sistema_gestor_de_biblioteca.Models;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public DbSet<Libro> Libros { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<Historial> Historiales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Historial>()
            .HasOne(h => h.Usuario)
            .WithMany(u => u.Historiales)
            .HasForeignKey(h => h.UsuarioId);

        modelBuilder.Entity<Historial>()
            .HasOne(h => h.Libro)
            .WithMany(l => l.Historiales)
            .HasForeignKey(h => h.LibroId);

        // Otras configuraciones del modelo...
    }
}
