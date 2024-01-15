using WebApp.Models;
using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class ArtVenta
    {
        public ArtVenta()
        {
            Detalles = new HashSet<Detalle>();
        }

        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
        public bool? Iva { get; set; }
        public decimal? Total { get; set; }

        public virtual ICollection<Detalle> Detalles { get; set; }
    }
}
