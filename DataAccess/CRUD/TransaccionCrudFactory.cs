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
            var op = new SQLOperation { ProcedureName = "CRE_TRANSACCION_PR" };
            op.AddIntParam("P_IdCuentaBancaria", t.IdCuentaBancaria);
            op.AddIntParam("P_IdCuentaComercio", t.IdCuentaComercio);
            op.AddIntParam("P_IdCuentaCliente", t.IdCuentaCliente);
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
            var op = new SQLOperation { ProcedureName = "UPD_TRANSACCION_PR" };
            op.AddIntParam("P_Id", t.Id);
            op.AddIntParam("P_IdCuentaBancaria", t.IdCuentaBancaria);
            op.AddIntParam("P_IdCuentaComercio", t.IdCuentaComercio);
            op.AddIntParam("P_IdCuentaCliente", t.IdCuentaCliente);
            op.AddDecimalParam("P_Monto", t.Monto, 18, 2);
            op.AddDecimalParam("P_Comision", t.Comision, 18, 2);
            op.AddDecimalParam("P_DescuentoAplicado", t.DescuentoAplicado, 18, 2);
            op.AddDateTimeParam("P_Fecha", t.Fecha);
            op.AddStringParameter("P_MetodoPago", t.MetodoPago);
            _sqlDao.ExecuteProcedure(op);
        }
        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }


        public override List<T> RetrieveAll<T>()
        {
            var lstTransacciones = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_TRANSACCION_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var cliente = BuildTransaccion(row);
                    lstTransacciones.Add((T)(object)cliente);
                }
            }

            return lstTransacciones;
        }

        public override T RetrieveById<T>(int id)
        {
            var op = new SQLOperation { ProcedureName = "RET_TRANSACCION_BY_ID_PR" };
            op.AddIntParam("P_Id", id);
            var lstResult = _sqlDao.ExecuteQueryProcedure(op);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildTransaccion(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByBanco<T>(int idCuentaBancaria)
        {
            var op = new SQLOperation { ProcedureName = "RET_TRANSACCION_BY_BANCO_PR" };
            op.AddIntParam("P_IdCuentaBancaria", idCuentaBancaria);
            var lstResult = _sqlDao.ExecuteQueryProcedure(op);

            if (typeof(T) == typeof(List<Transaccion>))
            {
                var transacciones = new List<Transaccion>();
                foreach (var row in lstResult)
                {
                    transacciones.Add(BuildTransaccion(row));
                }
                return (T)(object)transacciones;
            }
            else if (lstResult.Count > 0)
            {
                var cliente = BuildTransaccion(lstResult[0]);
                return (T)(object)cliente;
            }
            return default(T);
        }

        public T RetrieveByComercio<T>(int idComercio)
        {

            var op = new SQLOperation { ProcedureName = "RET_TRANSACCION_BY_COMERCIO_PR" };
            op.AddIntParam("P_IdCuentaComercio", idComercio);
            var lstResult = _sqlDao.ExecuteQueryProcedure(op);
            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildTransaccion(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByCliente<T>(int idCliente)
        {

            var op = new SQLOperation { ProcedureName = "RET_TRANSACCION_BY_CLIENTE_PR" };
            op.AddIntParam("P_IdCuentaCliente", idCliente);
            var lstResult = _sqlDao.ExecuteQueryProcedure(op);
            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildTransaccion(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public override void Delete(BaseDTO baseDTO)
        {
            var transaccion = baseDTO as Transaccion;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_TRANSACCION_PR" };
            sqlOperation.AddIntParam("P_Id", transaccion.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public  List<Transaccion> RetrieveAllById(int clienteId)
        {
            var lstTransacciones = new List<Transaccion>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_TRANSACCION_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var cliente = BuildTransaccion(row);
                    lstTransacciones.Add((Transaccion)(object)cliente);
                }
            }

            return lstTransacciones.Where(t => t.IdCuentaCliente == clienteId).ToList();
        }

        private Transaccion BuildTransaccion(Dictionary<string, object> r)
        {
            return new Transaccion()
            {
                Id = (int)r["Id"],
                IdCuentaBancaria = (int)r["IdCuentaBancaria"],
                IdCuentaComercio = (int)r["IdCuentaComercio"],
                IdCuentaCliente = (int)r["IdCuentaCliente"],
                Monto = Convert.ToDecimal(r["Monto"]),
                Comision = Convert.ToDecimal(r["Comision"]),
                DescuentoAplicado = Convert.ToDecimal(r["DescuentoAplicado"]),
                Fecha = (DateTime)r["Fecha"],
                MetodoPago = (string)r["MetodoPago"]
            };
        }


    }
}