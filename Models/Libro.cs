using System.Collections.Generic;

namespace Sistema_gestor_de_biblioteca.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Genero { get; set; }
        public bool EstaReservado { get; set; }

        // Propiedad de navegación para los préstamos
        public ICollection<Prestamo> Prestamos { get; set; }

        // Propiedad de navegación para los historiales
        public ICollection<Historial> Historiales { get; set; }
    }
}
