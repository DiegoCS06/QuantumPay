using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Comision : BaseDTO
    {
        public int IdInstitucionBancaria { get; set; }
        public int? IdCuentaComercio { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal MontoMaximo { get; set; }
    }
}