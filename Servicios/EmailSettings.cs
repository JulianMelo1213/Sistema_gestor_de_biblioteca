﻿namespace Sistema_gestor_de_biblioteca.Servicios
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
