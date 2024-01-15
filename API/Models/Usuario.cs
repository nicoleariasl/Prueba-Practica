using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Encabezados = new HashSet<Encabezado>();
        }

        public int Id { get; set; }
        public string? Usuario1 { get; set; }
        public string? Clave { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Encabezado> Encabezados { get; set; }
    }
}
