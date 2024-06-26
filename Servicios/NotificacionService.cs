// Services/NotificacionService.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sistema_gestor_de_biblioteca.Models;
using Sistema_gestor_de_biblioteca.Servicios;


public class NotificacionService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificacionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                var prestamos = await context.Prestamos
                    .Where(p => p.FechaDevolucion <= DateTime.Now.AddDays(3) && !p.Renovado)
                    .Include(p => p.Usuario)
                    .Include(p => p.Libro) // Asegúrate de incluir el libro también
                    .ToListAsync();

                foreach (var prestamo in prestamos)
                {
                    var usuario = prestamo.Usuario;
                    var libro = prestamo.Libro;
                    var subject = "Recordatorio de Devolución de Libro";
                    var message = $"Estimado {usuario.Nombre},\n\nEl libro '{libro.Titulo}' que has prestado está próximo a vencer. Por favor, devuelve el libro antes de la fecha de devolución: {prestamo.FechaDevolucion:dd/MM/yyyy}.\n\nGracias.";

                    await emailService.SendEmailAsync(usuario.Email, subject, message);
                }
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
