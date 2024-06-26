using System.Collections.Generic;

namespace Sistema_gestor_de_biblioteca.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Propiedad de navegación para los préstamos
        public ICollection<Prestamo> Prestamos { get; set; }

        // Propiedad de navegación para los historiales
        public ICollection<Historial> Historiales { get; set; }
    }
}
