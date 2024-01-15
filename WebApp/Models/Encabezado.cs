using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Encabezado
    {
        public Encabezado()
        {
            Detalles = new HashSet<Detalle>();
        }

        public int FacturaId { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Cliente { get; set; }

        public virtual Usuario? ClienteNavigation { get; set; }
        public virtual ICollection<Detalle> Detalles { get; set; }
    }
}
