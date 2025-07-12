using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class InstitucionBancariaCrudFactory : CrudFactory
    {

        public InstitucionBancariaCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }


        public override void Create(BaseDTO baseDTO)
        {
            var institucionBancaria = baseDTO as InstitucionBancaria;
            var sqlOperation = new SQLOperation() { ProcedureName = "CRE_INSTITUCIONBANCARIA_PR" };

            sqlOperation.ProcedureName = "CRE_INSTITUCIONBANCARIA_PR";

            sqlOperation.AddIntParam("P_CodigoIdentidadBancaria", institucionBancaria.codigoIdentidad);
            sqlOperation.AddIntParam("P_CodigoIBAN", institucionBancaria.codigoIBAN);
            sqlOperation.AddStringParameter("P_CedulaJuridica", institucionBancaria.cedulaJuridica);
            sqlOperation.AddStringParameter("P_Direccion", institucionBancaria.direccionSedePrincipal);
            sqlOperation.AddIntParam("P_Telefono", institucionBancaria.telefono);
            sqlOperation.AddStringParameter("P_CorreoElectronico", institucionBancaria.correoElectronico);

            _sqlDao.ExecuteProcedure(sqlOperation);

        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstInstitucionesBancarias = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_INSTITUCIONBANCARIA_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var institucionBancaria = BuildInstitucionBancaria(row);
                    lstInstitucionesBancarias.Add((T)(object)institucionBancaria);
                }
            }

            return lstInstitucionesBancarias;
        }


        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_ID_PR" };

            sqlOperation.AddIntParam("P_Id", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);
                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByIBAN<T>(int codigoIBAN)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_IBAN_PR" };

            sqlOperation.AddIntParam("P_CodigoIBAN", codigoIBAN);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);

                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByTelefono<T>(int telefono)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_TELEFONO_PR" };

            sqlOperation.AddIntParam("P_Telefono", telefono);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);

                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByEmail<T>(string correoElectronico)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_EMAIL_PR" };

            sqlOperation.AddStringParameter("P_CorreoElectronico", correoElectronico);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);
                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }
            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var institucionBancaria = baseDTO as InstitucionBancaria;
            var sqlOperation = new SQLOperation() { ProcedureName = "UPD_INSTITUCIONBANCARIA_PR" };

            sqlOperation.AddIntParam("Id", institucionBancaria.Id);
            sqlOperation.AddDateTimeParam("Created", institucionBancaria.Created);
            sqlOperation.AddDateTimeParam("Updated", institucionBancaria.Updated);
            sqlOperation.AddIntParam("CodigoIdentidadBancaria", institucionBancaria.codigoIdentidad);
            sqlOperation.AddIntParam("CodigoIBAN", institucionBancaria.codigoIBAN);
            sqlOperation.AddStringParameter("CedulaJuridica", institucionBancaria.cedulaJuridica);
            sqlOperation.AddStringParameter("Direccion", institucionBancaria.direccionSedePrincipal);
            sqlOperation.AddIntParam("Telefono", institucionBancaria.telefono);
            sqlOperation.AddStringParameter("CorreoElectronico", institucionBancaria.correoElectronico);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var institucionBancaria = baseDTO as InstitucionBancaria;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_INSTITUCIONBANCARIA_PR" };
            sqlOperation.AddIntParam("P_Id", institucionBancaria.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private InstitucionBancaria BuildInstitucionBancaria(Dictionary<string, object> row)
        {
            return new InstitucionBancaria()
            {
                Id = (int)row["Id"],
                Created = row["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created"],
                Updated = row["Updated"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Updated"],
                codigoIdentidad = row["CodigoIdentidad"] == DBNull.Value ? 0 : Convert.ToInt32(row["CodigoIdentidad"]),
                codigoIBAN = row["CodigoIBAN"] == DBNull.Value ? 0 : Convert.ToInt32(row["CodigoIBAN"]),
                cedulaJuridica = row["CedulaJuridica"].ToString(),
                direccionSedePrincipal = row["CedulaJuridica"].ToString(),
                telefono = row["Telefono"] == DBNull.Value ? 0 : Convert.ToInt32(row["Telefono"]),
                correoElectronico = row["CorreoElectronico"].ToString()
            };
        }


        public T RetrieveByEmail<T>(T institucionBancaria)
        {
            throw new NotImplementedException();
        }
    }
}
