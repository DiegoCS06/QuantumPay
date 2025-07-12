using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class ComercioCrudFactory : CrudFactory
    {

        public ComercioCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }


        public override void Create(BaseDTO baseDTO)
        {
            var comercio = baseDTO as Comercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "CRE_COMERCIO_PR" };

            sqlOperation.ProcedureName = "CRE_COMERCIO_PR";

            sqlOperation.AddStringParameter("P_Nombre", comercio.Nombre);

            _sqlDao.ExecuteProcedure(sqlOperation);

        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstComercios = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_COMCERCIO_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var comercio = BuildComercio(row);
                    lstComercios.Add((T)(object)comercio);
                }
            }

            return lstComercios;
        }

        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_COMERCIO_BY_ID_PR" };

            sqlOperation.AddIntParam("P_Id", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var comercio = BuildComercio(row);
                return (T)Convert.ChangeType(comercio, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByComercioName<T>(string nombre)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_COMERCIO_BY_NAME_PR" };

            sqlOperation.AddStringParameter("P_Nombre", nombre);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var comercio = BuildComercio(row);

                return (T)Convert.ChangeType(comercio, typeof(T));
            }

            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var comercio = baseDTO as Comercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "UPD_COMERCIO_PR" };

            sqlOperation.AddIntParam("P_Id", comercio.Id);
            sqlOperation.AddIntParam("P_IdCuenta", comercio.IdCuenta);
            sqlOperation.AddDateTimeParam("P_Created", comercio.Created);
            sqlOperation.AddDateTimeParam("P_Updated", comercio.Updated);
            sqlOperation.AddStringParameter("P_Nombre", comercio.Nombre);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var comercio = baseDTO as Comercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_COMERCIO_PR" };
            sqlOperation.AddIntParam("P_Id", comercio.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private Comercio BuildComercio(Dictionary<string, object> row)
        {
            return new Comercio()
            {
                Id = (int)row["Id"],
                IdCuenta = (int)row["IdCuenta"],
                Created = row["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created"],
                Updated = row["Updated"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Updated"],
                Nombre = row["Nombre"].ToString()
            };
        }


        public T RetrieveByEmail<T>(T comercio)
        {
            throw new NotImplementedException();
        }
    }
}
