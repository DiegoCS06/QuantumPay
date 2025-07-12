using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using DataAccess.DAOs;

namespace DataAccess.CRUD
{
    public class ComisionCrudFactory : CrudFactory
    {
        public ComisionCrudFactory() => _sqlDao = SQL_DAO.GetInstance();

        public override void Create(BaseDTO dto)
        {
            var c = (Comision)dto;
            var op = new SQLOperation { ProcedureName = "SP_INS_COMISION" };
            op.AddIntParam("P_IdInstitucionBancaria", c.IdInstitucionBancaria);
            if (c.IdCuentaComercio.HasValue)
                op.AddIntParam("P_IdCuentaComercio", c.IdCuentaComercio.Value);
            op.AddDoubleParam("P_Porcentaje", (double)c.Porcentaje);
            op.AddDoubleParam("P_MontoMaximo", (double)c.MontoMaximo);
            _sqlDao.ExecuteProcedure(op);
        }

        public override void Update(BaseDTO dto)
        {
            var c = (Comision)dto;
            var op = new SQLOperation { ProcedureName = "SP_UPD_COMISION" };
            op.AddIntParam("P_Id", c.Id);
            op.AddIntParam("P_IdInstitucionBancaria", c.IdInstitucionBancaria);
            if (c.IdCuentaComercio.HasValue)
                op.AddIntParam("P_IdCuentaComercio", c.IdCuentaComercio.Value);
            op.AddDoubleParam("P_Porcentaje", (double)c.Porcentaje);
            op.AddDoubleParam("P_MontoMaximo", (double)c.MontoMaximo);
            _sqlDao.ExecuteProcedure(op);
        }

        public override void Delete(BaseDTO dto)
        {
            var c = (Comision)dto;
            var op = new SQLOperation { ProcedureName = "SP_DEL_COMISION" };
            op.AddIntParam("P_Id", c.Id);
            _sqlDao.ExecuteProcedure(op);
        }

        public override T Retrieve<T>() => throw new NotImplementedException();
        public override T RetrieveById<T>(int Id) => throw new NotImplementedException();

        public T RetrieveById<T>(Comision c)
        {
            var op = new SQLOperation { ProcedureName = "SP_RET_COMISION" };
            op.AddIntParam("P_Id", c.Id);
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            if (rows.Count > 0)
            {
                var r = rows[0];
                var x = new Comision
                {
                    Id = (int)r["Id"],
                    IdInstitucionBancaria = (int)r["IdInstitucionBancaria"],
                    IdCuentaComercio = r["IdCuentaComercio"] as int?,
                    Porcentaje = Convert.ToDecimal(r["Porcentaje"]),
                    MontoMaximo = Convert.ToDecimal(r["MontoMaximo"])
                };
                return (T)Convert.ChangeType(x, typeof(T));
            }
            return default;
        }

        public override List<T> RetrieveAll<T>()
        {
            var op = new SQLOperation { ProcedureName = "SP_RET_ALL_COMISION" };
            var rows = _sqlDao.ExecuteQueryProcedure(op);
            var list = new List<T>();
            foreach (var r in rows)
            {
                var x = new Comision
                {
                    Id = (int)r["Id"],
                    IdInstitucionBancaria = (int)r["IdInstitucionBancaria"],
                    IdCuentaComercio = r["IdCuentaComercio"] as int?,
                    Porcentaje = Convert.ToDecimal(r["Porcentaje"]),
                    MontoMaximo = Convert.ToDecimal(r["MontoMaximo"])
                };
                list.Add((T)Convert.ChangeType(x, typeof(T)));
            }
            return list;
        }
    }
}