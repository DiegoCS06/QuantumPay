using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PromocionComercio : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Descuento { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}