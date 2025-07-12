using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class CuentaComercioCrudFactory : CrudFactory
    {

        public CuentaComercioCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }


        public override void Create(BaseDTO baseDTO)
        {
            var cuentaComercio = baseDTO as CuentaComercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "CRE_CUENTACOMERCIO_PR" };

            sqlOperation.ProcedureName = "CRE_CUENTACOMERCIO_PR";

            sqlOperation.AddStringParameter("P_NombreUsuario", cuentaComercio.NombreUsuario);
            sqlOperation.AddStringParameter("P_Contrasena", cuentaComercio.Contrasena);
            sqlOperation.AddStringParameter("P_CedulaJuridica", cuentaComercio.CedulaJuridica);
            sqlOperation.AddIntParam("P_Telefono", cuentaComercio.Telefono);
            sqlOperation.AddStringParameter("P_CorreoElectronico", cuentaComercio.CorreoElectronico);
            sqlOperation.AddStringParameter("P_Direccion", cuentaComercio.Direccion);

            _sqlDao.ExecuteProcedure(sqlOperation);

        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstCuentasComercio = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_CUENTACOMERCIO_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var cuentaComercio = BuildCuentasComercio(row);
                    lstCuentasComercio.Add((T)(object)cuentaComercio);
                }
            }

            return lstCuentasComercio;
        }


        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CUENTACOMERCIO_BY_ID_PR" };

            sqlOperation.AddIntParam("P_Id", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cuentaComercio = BuildCuentasComercio(row);
                return (T)Convert.ChangeType(cuentaComercio, typeof(T));
            }

            return default(T);
        }


        public T RetrieveByUserName<T>(string NombreUsuario)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CUENTACOMERCIO_BY_USERNAME_PR" };

            sqlOperation.AddStringParameter("P_NombreUsuario", NombreUsuario);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cuentaComercio = BuildCuentasComercio(row);

                return (T)Convert.ChangeType(cuentaComercio, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByTelefono<T>(int Telefono)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CUENTACOMERCIO_BY_TELEFONO_PR" };

            sqlOperation.AddIntParam("P_Telefono", Telefono);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cuentaComercio = BuildCuentasComercio(row);

                return (T)Convert.ChangeType(cuentaComercio, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByEmail<T>(string CorreoElectronico)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CUENTACOMERCIO_BY_EMAIL_PR" };

            sqlOperation.AddStringParameter("P_CorreoElectronico", CorreoElectronico);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cuentaComercio = BuildCuentasComercio(row);
                return (T)Convert.ChangeType(cuentaComercio, typeof(T));
            }
            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var cuentaComercio = baseDTO as CuentaComercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "UPD_CUENTACOMERCIO_PR" };

            sqlOperation.AddIntParam("P_Id", cuentaComercio.Id);
            sqlOperation.AddDateTimeParam("P_Created", cuentaComercio.Created);
            sqlOperation.AddDateTimeParam("P_Updated", cuentaComercio.Updated);
            sqlOperation.AddStringParameter("P_NombreUsuario", cuentaComercio.NombreUsuario);
            sqlOperation.AddStringParameter("P_Contrasena", cuentaComercio.Contrasena);
            sqlOperation.AddStringParameter("P_CedulaJuridica", cuentaComercio.CedulaJuridica);
            sqlOperation.AddIntParam("P_Telefono", cuentaComercio.Telefono);
            sqlOperation.AddStringParameter("P_CorreoElectronico", cuentaComercio.CorreoElectronico);
            sqlOperation.AddStringParameter("P_Direccion", cuentaComercio.Direccion);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var cuentaComercio = baseDTO as CuentaComercio;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_CUENTACOMERCIO_PR" };
            sqlOperation.AddIntParam("P_Id", cuentaComercio.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private CuentaComercio BuildCuentasComercio(Dictionary<string, object> row)
        {
            return new CuentaComercio()
            {
                Id = (int)row["Id"],
                Created = row["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created"],
                Updated = row["Updated"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Updated"],
                NombreUsuario = row["NombreUsuario"].ToString(),
                Contrasena = row["Contrasena"].ToString(),
                CedulaJuridica = row["CedulaJuridica"].ToString(),
                Telefono = row["Telefono"] == DBNull.Value ? 0 : Convert.ToInt32(row["Telefono"]),
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                Direccion = row["Direccion"].ToString()
            };
        }

        public T RetrieveByEmail<T>(T cuentaComercio)
        {
            throw new NotImplementedException();
        }
    }
}
