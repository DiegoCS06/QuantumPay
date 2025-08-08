using System;
namespace DTOs
{
    public class ClienteCuenta : BaseDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string Banco { get; set; }
        public string TipoCuenta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
