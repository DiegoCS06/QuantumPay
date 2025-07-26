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

            sqlOperation.AddStringParameter("P_codigoIdentidad", institucionBancaria.codigoIdentidad);
            sqlOperation.AddStringParameter("P_codigoIBAN", institucionBancaria.codigoIBAN);
            sqlOperation.AddStringParameter("P_cedulaJuridica", institucionBancaria.cedulaJuridica);
            sqlOperation.AddStringParameter("P_direccionSedePrincipal", institucionBancaria.direccionSedePrincipal);
            sqlOperation.AddIntParam("@P_telefono", institucionBancaria.telefono);
            sqlOperation.AddStringParameter("P_estadoSolicitud", institucionBancaria.estadoSolicitud);
            sqlOperation.AddStringParameter("P_correoElectronico", institucionBancaria.correoElectronico);
            sqlOperation.AddStringParameter("P_contrasena", institucionBancaria.contrasena);

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

            sqlOperation.AddIntParam("P_idInstBancaria", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);
                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByCodigoIdentidad<T>(string codigoIdentidad)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_CODIGOIDENTIDAD_PR" };

            sqlOperation.AddStringParameter("P_codigoIdentidad", codigoIdentidad);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var institucionBancaria = BuildInstitucionBancaria(row);

                return (T)Convert.ChangeType(institucionBancaria, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByIBAN<T>(string codigoIBAN)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_INSTITUCIONBANCARIA_BY_IBAN_PR" };

            sqlOperation.AddStringParameter("P_codigoIBAN", codigoIBAN);

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

            sqlOperation.AddIntParam("P_telefono", telefono);

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

            sqlOperation.AddStringParameter("P_correoElectronico", correoElectronico);

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

            sqlOperation.AddIntParam("P_idInstBancaria", institucionBancaria.Id);
            sqlOperation.AddStringParameter("P_codigoIdentidad", institucionBancaria.codigoIdentidad);
            sqlOperation.AddStringParameter("P_codigoIBAN", institucionBancaria.codigoIBAN);
            sqlOperation.AddStringParameter("P_cedulaJuridica", institucionBancaria.cedulaJuridica);
            sqlOperation.AddStringParameter("P_direccionSedePrincipal", institucionBancaria.direccionSedePrincipal);
            sqlOperation.AddIntParam("P_telefono", institucionBancaria.telefono);
            sqlOperation.AddStringParameter("P_estadoSolicitud", institucionBancaria.estadoSolicitud);
            sqlOperation.AddStringParameter("P_correoElectronico", institucionBancaria.correoElectronico);
            sqlOperation.AddStringParameter("P_contrasena", institucionBancaria.contrasena);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var institucionBancaria = baseDTO as InstitucionBancaria;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_INSTITUCIONBANCARIA_PR" };
            sqlOperation.AddIntParam("P_idInstBancaria", institucionBancaria.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        private InstitucionBancaria BuildInstitucionBancaria(Dictionary<string, object> row)
        {
            return new InstitucionBancaria()
            {
                Id = (int)row["idInstBancaria"],
                codigoIdentidad = row["codigoIdentidad"].ToString(),
                codigoIBAN = row["codigoIBAN"].ToString(),
                cedulaJuridica = row["cedulaJuridica"].ToString(),
                direccionSedePrincipal = row["direccionSedePrincipal"].ToString(),
                telefono = row["telefono"] == DBNull.Value ? 0 : Convert.ToInt32(row["telefono"]),
                estadoSolicitud = row["estadoSolicitud"].ToString(),
                correoElectronico = row["correoElectronico"].ToString(),
                contrasena = row["contrasena"].ToString()
            };
        }

        public T RetrieveByEmail<T>(T institucionBancaria)
        {
            throw new NotImplementedException();
        }
    }
}
