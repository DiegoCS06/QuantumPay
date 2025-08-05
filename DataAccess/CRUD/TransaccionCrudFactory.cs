// 2_DataAccess/DataAccess/CRUD/TransaccionCrudFactory.cs
using System;
using System.Collections.Generic;
using DataAccess.DAOs;
using DTOs;

namespace DataAccess.CRUD
{
    public class TransaccionCrudFactory : CrudFactory
    {
        public TransaccionCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }

        public override void Create(BaseDTO dto)
        {
            var t = (Transaccion)dto;
            var op = new SQLOperation { ProcedureName = "SP_INS_TRANSACCION" };

            op.AddStringParameter("P_IdCuentaBancaria", t.IdCuentaBancaria);
            op.AddStringParameter("P_IBAN", t.IBAN);
            op.AddIntParam("P_IdCuentaComercio", t.IdCuentaComercio);
            op.AddDecimalParam("P_Monto", t.Monto, 18, 2);
            op.AddDecimalParam("P_Comision", t.Comision, 18, 2);
            op.AddDecimalParam("P_DescuentoAplicado", t.DescuentoAplicado, 18, 2);
            op.AddDateTimeParam("P_Fecha", t.Fecha);
            op.AddStringParameter("P_MetodoPago", t.MetodoPago);

            _sqlDao.ExecuteProcedure(op);
        }

        public override void Update(BaseDTO dto)
        {
            var t = (Transaccion)dto;
            var op = new SQLOperation { ProcedureName = "SP_UPD_TRANSACCION" };

            op.AddIntParam("P_Id", t.Id);
            op.AddStringParameter("P_IdCuentaBancaria", t.IdCuentaBancaria);
            op.AddStringParameter("P_IBAN", t.IBAN);
            op.AddIntParam("P_IdCuentaComercio", t.IdCuentaComercio);
            op.AddIntParam("P_IdCuentaCliente", t.IdCuentaCliente);
            op.AddDecimalParam("P_Monto", t.Monto, 18, 2);
            op.AddDecimalParam("P_Comision", t.Comision, 18, 2);
            op.AddDecimalParam("P_DescuentoAplicado", t.DescuentoAplicado, 18, 2);
            op.AddDateTimeParam("P_Fecha", t.Fecha);
            op.AddStringParameter("P_MetodoPago", t.MetodoPago);

            _sqlDao.ExecuteProcedure(op);
        }

        public override void Delete(BaseDTO dto)
        {
            var t = (Transaccion)dto;
            var op = new SQLOperation { ProcedureName = "SP_DEL_TRANSACCION" };
            op.AddIntParam("P_Id", t.Id);
            _sqlDao.ExecuteProcedure(op);
        }

        public override List<T> RetrieveAll<T>()
        {
            var op = new SQLOperation { ProcedureName = "SP_SEL_ALL_TRANSACCIONES" };
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            var list = new List<T>();

            foreach (var r in rows)
            {
                var t = BuildTransaccion(r);
                list.Add((T)(object)t);
            }

            return list;
        }

        public override T RetrieveById<T>(int id)
        {
            var op = new SQLOperation { ProcedureName = "SP_SEL_TRANSACCION_POR_ID" };
            op.AddIntParam("P_Id", id);
            var rows = _sqlDao.ExecuteQueryProcedure(op);

            if (rows.Count == 0)
                return default;

            var t = BuildTransaccion(rows[0]);
            return (T)(object)t;
        }

        public List<Transaccion> RetrieveByBanco(int cuentaId)
        {
            var op = new SQLOperation { ProcedureName = "SP_SEL_TRANSACCIONES_POR_CUENTA" };
            op.AddIntParam("P_IdCuentaBancaria", cuentaId);
            var rows = _sqlDao.ExecuteQueryProcedure(op);

            var list = new List<Transaccion>();
            foreach (var r in rows)
                list.Add(BuildTransaccion(r));

            return list;
        }

        public List<Transaccion> RetrieveByComercio(int comercioId)
        {
            var op = new SQLOperation { ProcedureName = "SP_SEL_TRANSACCIONES_POR_COMERCIO" };
            op.AddIntParam("P_IdCuentaComercio", comercioId);
            var rows = _sqlDao.ExecuteQueryProcedure(op);

            var list = new List<Transaccion>();
            foreach (var r in rows)
                list.Add(BuildTransaccion(r));

            return list;
        }

        public List<Transaccion> RetrieveByCliente(int clienteId)
        {
            var op = new SQLOperation { ProcedureName = "dbo.SP_SEL_TRANSACCIONES_POR_CLIENTE" };
            op.AddIntParam("ClienteId", clienteId);
            var rows = _sqlDao.ExecuteQueryProcedure(op);

            var list = new List<Transaccion>();
            foreach (var r in rows)
                list.Add(BuildTransaccion(r));

            return list;
        }

        private Transaccion BuildTransaccion(Dictionary<string, object> r)
        {
            return new Transaccion
            {
                Id = (int)r["Id"],
                IdCuentaCliente = (int)r["IdCuentaCliente"],
                IdCuentaBancaria = (int)r["IdCuentaBancaria"],
                IBAN = r.ContainsKey("IBAN") ? r["IBAN"].ToString() : string.Empty,
                IdCuentaComercio = (int)r["IdCuentaComercio"],
                Monto = Convert.ToDecimal(r["Monto"]),
                Comision = Convert.ToDecimal(r["Comision"]),
                DescuentoAplicado = Convert.ToDecimal(r["DescuentoAplicado"]),
                Fecha = (DateTime)r["Fecha"],
                MetodoPago = (string)r["MetodoPago"]
            };
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }
    }
}
