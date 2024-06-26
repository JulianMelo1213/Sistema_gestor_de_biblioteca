# **Sistema de Gestión de Biblioteca en .NET Core MVC**

## Descripción del Proyecto:

El objetivo de este proyecto es desarrollar un sistema de gestión de biblioteca utilizando .NET Core MVC. El sistema permitirá a los usuarios buscar libros, realizar reservas, revisar su historial de préstamos, renovar los préstamos existentes.

## Especificaciones Técnicas:

### Tecnología:

- Lenguaje de Programación: C# con .NET Core MVC
- Base de datos: SQL Server

### Arquitectura de la Aplicación:

La aplicación se basará en el patrón de diseño MVC (Modelo-Vista-Controlador).

- **Modelo**: Representará los objetos de dominio (como Libro, Usuario, Préstamo), y los servicios que gestionan estas entidades y sus interacciones.
- **Vista**: Será responsable de la presentación y la interacción con el usuario. Incluirá las páginas web para buscar libros, realizar y gestionar reservas, y revisar el historial de préstamos.
- **Controlador**: Manejará las solicitudes de los usuarios, coordinará los modelos y las vistas, y controlará el flujo de la aplicación.

### Funcionalidades del Sistema:

- **Búsqueda de libros**: Los usuarios deben poder buscar libros por título, autor o género.
- **Reserva de libros**: Los usuarios deben poder reservar libros disponibles en la biblioteca.
- **Renovación de préstamos**: Los usuarios deben poder renovar sus préstamos si el libro no ha sido reservado por otro usuario.
- **Historial de préstamos**: Los usuarios deben poder revisar su historial de préstamos.
- **Notificaciones**: Los usuarios deben recibir notificaciones sobre la fecha de devolución de los libros prestados.
