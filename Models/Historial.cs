using System;


namespace Sistema_gestor_de_biblioteca.Models
{
    public class Historial
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public string Accion { get; set; }
        public DateTime FechaAccion { get; set; }
    }
}
