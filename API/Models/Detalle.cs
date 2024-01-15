using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Detalle
    {
        public int Id { get; set; }
        public int? IdFactura { get; set; }
        public int? CodArticulo { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Total { get; set; }

        public virtual ArtVenta? CodArticuloNavigation { get; set; }
        public virtual Encabezado? IdFacturaNavigation { get; set; }
    }
}
