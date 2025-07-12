using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class AdminCrudFactory : CrudFactory
    {

        public AdminCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }


        public override void Create(BaseDTO baseDTO)
        {
            var admin = baseDTO as Admin;
            var sqlOperation = new SQLOperation() { ProcedureName = "CRE_ADMIN_PR" };

            sqlOperation.ProcedureName = "CRE_ADMIN_PR";

            sqlOperation.AddStringParameter("P_NombreUsuario", admin.nombreUsuario);
            sqlOperation.AddStringParameter("P_Contrasena", admin.contrasena);

            _sqlDao.ExecuteProcedure(sqlOperation);

        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstAdmins = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_ADMINS_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var admin = BuildAdmin(row);
                    lstAdmins.Add((T)(object)admin);
                }
            }

            return lstAdmins;
        }

        public T RetrieveByUserName<T>(Admin admin)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ADMIN_BY_USERNAME_PR" };

            sqlOperation.AddStringParameter("P_NombreUsuario", admin.nombreUsuario);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                admin = BuildAdmin(row);

                return (T)Convert.ChangeType(admin, typeof(T));
            }

            return default(T);
        }

        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ADMIN_BY_ID_PR" };

            sqlOperation.AddIntParam("@P_idAdmin", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var admin = BuildAdmin(row);
                return (T)Convert.ChangeType(admin, typeof(T));
            }

            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var admin = baseDTO as Admin;
            var sqlOperation = new SQLOperation() { ProcedureName = "UPD_ADMIN_PR" };

            sqlOperation.AddIntParam("P_Id", admin.Id);
            sqlOperation.AddDateTimeParam("P_Created", admin.Created);
            sqlOperation.AddDateTimeParam("P_Updated", admin.Updated);
            sqlOperation.AddStringParameter("P_NombreUsuario", admin.nombreUsuario);
            sqlOperation.AddStringParameter("P_Contrasena", admin.contrasena);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var admin = baseDTO as Admin;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_ADMIN_PR" };
            sqlOperation.AddIntParam("P_Id", admin.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private Admin BuildAdmin(Dictionary<string, object> row)
        {
            return new Admin()
            {
                Id = (int)row["Id"],
                Created = row["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created"],
                Updated = row["Updated"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Updated"],
                nombreUsuario = row["NombreUsuario"].ToString(),
                contrasena = row["Contrasena"].ToString()
            };
        }

    }
}
