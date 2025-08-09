using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class ClienteCuentaCrudFactory : CrudFactory
    {
        public ClienteCuentaCrudFactory() => _sqlDao = SQL_DAO.GetInstance();

        public override void Create(BaseDTO dto)
        {
            var c = (ClienteCuenta)dto;
            var op = new SQLOperation { ProcedureName = "SP_INS_CLIENTE_CUENTA" };
            op.AddIntParam("ClienteId", c.ClienteId);
            op.AddVarcharParam("NumeroCuenta", c.NumeroCuenta, 50);
            op.AddVarcharParam("Banco", c.Banco, 100);
            op.AddVarcharParam("TipoCuenta", c.TipoCuenta, 50);
            op.AddDecimalParam("Saldo", c.Saldo, 18, 2);
            _sqlDao.ExecuteProcedure(op);
        }

        public override void Update(BaseDTO dto)
        {
            var c = (ClienteCuenta)dto;
            var op = new SQLOperation { ProcedureName = "SP_UPD_CLIENTE_CUENTA" };
            op.AddIntParam("Id", c.Id);
            op.AddVarcharParam("NumeroCuenta", c.NumeroCuenta, 50);
            op.AddVarcharParam("Banco", c.Banco, 100);
            op.AddVarcharParam("TipoCuenta", c.TipoCuenta, 50);
            op.AddDecimalParam("Saldo", c.Saldo, 18, 2);
            _sqlDao.ExecuteProcedure(op);
        }

        public override void Delete(BaseDTO dto)
        {
            var c = (ClienteCuenta)dto;
            var op = new SQLOperation { ProcedureName = "SP_DEL_CLIENTE_CUENTA" };
            op.AddIntParam("Id", c.Id);
            _sqlDao.ExecuteProcedure(op);
        }

        public override T Retrieve<T>() => throw new NotImplementedException();
        public override T RetrieveById<T>(int Id) => throw new NotImplementedException();
        public override List<T> RetrieveAll<T>() => throw new NotImplementedException();

        public List<ClienteCuenta> RetrieveByCliente(int clienteId)
        {
            var op = new SQLOperation { ProcedureName = "SP_SEL_CLIENTE_CUENTAS" };
            op.AddIntParam("ClienteId", clienteId);
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            var list = new List<ClienteCuenta>();
            foreach (var r in rows)
            {
                var cc = new ClienteCuenta
                {
                    Id = (int)r["Id"],
                    ClienteId = (int)r["ClienteId"],
                    NumeroCuenta = r["NumeroCuenta"].ToString(),
                    Banco = r["Banco"].ToString(),
                    TipoCuenta = r["TipoCuenta"].ToString(),
                    Saldo = Convert.ToDecimal(r["Saldo"]),
                    FechaCreacion = (DateTime)r["FechaCreacion"]
                };
                list.Add(cc);
            }
            return list;
        }
    }
}
