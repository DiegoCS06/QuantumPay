using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class PromocionComercioCrudFactory : CrudFactory
    {
        public PromocionComercioCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }

        public override void Create(BaseDTO baseDTO)
        {
            var promocion = baseDTO as PromocionComercio;
            var sqlOperation = new SQLOperation { ProcedureName = "CRE_PROMOCIONCOMERCIO_PR" };
            sqlOperation.AddStringParameter("P_Nombre", promocion.Nombre);
            sqlOperation.AddStringParameter("P_Descripcion", promocion.Descripcion);
            sqlOperation.AddDecimalParam("P_Descuento", promocion.Descuento, 5, 4);
            sqlOperation.AddDateTimeParam("P_FechaInicio", promocion.FechaInicio.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddDateTimeParam("P_FechaFin", promocion.FechaFin.ToDateTime(TimeOnly.MinValue));
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lst = new List<T>();
            var sqlOperation = new SQLOperation { ProcedureName = "RET_ALL_PROMOCIONCOMERCIO_PR" };
            var results = _sqlDao.ExecuteQueryProcedure(sqlOperation);
            foreach (var row in results)
            {
                var promocion = BuildPromocionComercio(row);
                lst.Add((T)(object)promocion);
            }
            return lst;
        }

        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation { ProcedureName = "RET_PROMOCIONCOMERCIO_BY_ID_PR" };
            sqlOperation.AddIntParam("P_Id", Id);
            var results = _sqlDao.ExecuteQueryProcedure(sqlOperation);
            if (results.Count > 0)
            {
                var promocion = BuildPromocionComercio(results[0]);
                return (T)(object)promocion;
            }
            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var promocion = baseDTO as PromocionComercio;
            var sqlOperation = new SQLOperation { ProcedureName = "UPD_PROMOCIONCOMERCIO_PR" };
            sqlOperation.AddIntParam("P_Id", promocion.Id);
            sqlOperation.AddStringParameter("P_Nombre", promocion.Nombre);
            sqlOperation.AddStringParameter("P_Descripcion", promocion.Descripcion);
            sqlOperation.AddDecimalParam("P_Descuento", promocion.Descuento, 5, 4);
            sqlOperation.AddDateTimeParam("P_FechaInicio", promocion.FechaInicio.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddDateTimeParam("P_FechaFin", promocion.FechaFin.ToDateTime(TimeOnly.MinValue));
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO baseDTO)
        {
            var promocion = baseDTO as PromocionComercio;
            var sqlOperation = new SQLOperation { ProcedureName = "DEL_PROMOCIONCOMERCIO_PR" };
            sqlOperation.AddIntParam("P_Id", promocion.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        public override T Retrieve<T>() => throw new NotImplementedException();

        private PromocionComercio BuildPromocionComercio(Dictionary<string, object> row)
        {
            return new PromocionComercio
            {
                Id = (int)row["id"],
                Nombre = row["nombre"].ToString(),
                Descripcion = row["descripcion"].ToString(),
                Descuento = Convert.ToDecimal(row["descuento"]),
                FechaInicio = DateOnly.FromDateTime(Convert.ToDateTime(row["fechaInicio"])),
                FechaFin = DateOnly.FromDateTime(Convert.ToDateTime(row["fechaFin"]))
            };
        }
    }
}