using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAOs;
using DTOs;

namespace DataAccess.CRUD
{
    public class TransaccionCrudFactory : CrudFactory
    {
        public TransaccionCrudFactory() => _sqlDao = SQL_DAO.GetInstance();

        public override void Create(BaseDTO dto)
        {
            var t = (Transaccion)dto;
            var op = new SQLOperation { ProcedureName = "SP_INS_TRANSACCION" };
            op.AddIntParam("P_IdCuentaBancaria", t.IdCuentaBancaria);
            op.AddIntParam("P_IdCuentaComercio", t.IdCuentaComercio);
            op.AddDoubleParam("P_Monto", (double)t.Monto);
            op.AddDoubleParam("P_Comision", (double)t.Comision);
            op.AddDoubleParam("P_DescuentoAplicado", (double)t.DescuentoAplicado);
            op.AddDateTimeParam("P_Fecha", t.Fecha);
            op.AddStringParameter("P_MetodoPago", t.MetodoPago);
            _sqlDao.ExecuteProcedure(op);
        }

        public override void Update(BaseDTO dto) => throw new NotImplementedException();
        public override void Delete(BaseDTO dto) => throw new NotImplementedException();
        public override T Retrieve<T>() => throw new NotImplementedException();
        public override T RetrieveById<T>(int Id) => throw new NotImplementedException();
        public override List<T> RetrieveAll<T>() => throw new NotImplementedException();

        public List<Transaccion> RetrieveByBanco(int idBanco)
        {
            var op = new SQLOperation { ProcedureName = "SP_RET_TRANS_POR_CUENTA" };
            op.AddIntParam("P_IdCuentaBancaria", idBanco);
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            var lst = new List<Transaccion>();
            foreach (var r in rows)
                lst.Add(new Transaccion
                {
                    Id = (int)r["Id"],
                    IdCuentaBancaria = (int)r["IdCuentaBancaria"],
                    IdCuentaComercio = (int)r["IdCuentaComercio"],
                    Monto = Convert.ToDecimal(r["Monto"]),
                    Comision = Convert.ToDecimal(r["Comision"]),
                    DescuentoAplicado = Convert.ToDecimal(r["DescuentoAplicado"]),
                    Fecha = (DateTime)r["Fecha"],
                    MetodoPago = (string)r["MetodoPago"]
                });
            return lst;
        }

        public List<Transaccion> RetrieveByComercio(int idComercio)
        {
            var op = new SQLOperation { ProcedureName = "SP_RET_TRANS_POR_CUENTA" };
            op.AddIntParam("P_IdCuentaComercio", idComercio);
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            var lst = new List<Transaccion>();
            foreach (var r in rows)
                lst.Add(new Transaccion
                {
                    Id = (int)r["Id"],
                    IdCuentaBancaria = (int)r["IdCuentaBancaria"],
                    IdCuentaComercio = (int)r["IdCuentaComercio"],
                    Monto = Convert.ToDecimal(r["Monto"]),
                    Comision = Convert.ToDecimal(r["Comision"]),
                    DescuentoAplicado = Convert.ToDecimal(r["DescuentoAplicado"]),
                    Fecha = (DateTime)r["Fecha"],
                    MetodoPago = (string)r["MetodoPago"]
                });
            return lst;
        }
    }
}