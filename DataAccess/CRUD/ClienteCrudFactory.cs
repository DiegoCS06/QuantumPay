using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class ClienteCrudFactory
        : CrudFactory
    {

        public ClienteCrudFactory()
        {
            _sqlDao = SQL_DAO.GetInstance();
        }


        public override void Create(BaseDTO baseDTO)
        {
            var cliente = baseDTO as Cliente;
            var sqlOperation = new SQLOperation() { ProcedureName = "CRE_CLIENTE_PR" };

            sqlOperation.ProcedureName = "CRE_CLIENTE_PR";

            sqlOperation.AddStringParameter("P_Cedula", cliente.cedula);
            sqlOperation.AddStringParameter("P_Nombre", cliente.nombre);
            sqlOperation.AddStringParameter("P_Apellidos", cliente.apellido);
            sqlOperation.AddIntParam("P_Telefono", cliente.telefono);
            sqlOperation.AddStringParameter("P_Correo", cliente.correo);
            sqlOperation.AddStringParameter("P_Direccion", cliente.direccion);
            sqlOperation.AddStringParameter("P_FotoCedula", cliente.fotoCedula);
            sqlOperation.AddDateTimeParam("P_FechaNacimiento", cliente.fechaNacimiento.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddStringParameter("P_FotoPerfil", cliente.fotoPerfil);
            sqlOperation.AddStringParameter("P_Contrasena", cliente.contrasena);
            sqlOperation.AddStringParameter("P_IBAN", cliente.IBAN);

            _sqlDao.ExecuteProcedure(sqlOperation);

        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstClientes = new List<T>();

            var sqlOperation = new SQLOperation() { ProcedureName = "RET_ALL_CLIENTE_PR" };

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                foreach (var row in lstResult)
                {
                    var cliente = BuildCliente(row);
                    lstClientes.Add((T)(object)cliente);
                }
            }

            return lstClientes;
        }

        public T RetrieveByCedula<T>(string cedula)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CLIENTE_BY_CEDULA_PR" };

            sqlOperation.AddStringParameter("P_Cedula", cedula);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildCliente(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByTelefono<T>(int telefono)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CLIENTE_BY_TELEFONO_PR" };

            sqlOperation.AddIntParam("P_Telefono", telefono);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildCliente(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public override T RetrieveById<T>(int Id)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CLIENTE_BY_ID_PR" };

            sqlOperation.AddIntParam("P_Id", Id);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildCliente(row);
                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByEmail<T>(string correo)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CLIENTE_BY_EMAIL_PR" };

            sqlOperation.AddStringParameter("P_Correo", correo);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildCliente(row);
                return (T)Convert.ChangeType(cliente, typeof(T));
            }
            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            var cliente = baseDTO as Cliente;
            var sqlOperation = new SQLOperation() { ProcedureName = "UPD_CLIENTE_PR" };

            sqlOperation.AddIntParam("P_Id", cliente.Id);
            sqlOperation.AddStringParameter("P_Cedula", cliente.cedula);
            sqlOperation.AddStringParameter("P_Nombre", cliente.nombre);
            sqlOperation.AddStringParameter("P_Apellidos", cliente.apellido);
            sqlOperation.AddIntParam("P_Telefono", cliente.telefono);
            sqlOperation.AddStringParameter("P_Correo", cliente.correo);
            sqlOperation.AddStringParameter("P_Direccion", cliente.direccion);
            sqlOperation.AddStringParameter("P_FotoCedula", cliente.fotoCedula);
            sqlOperation.AddDateTimeParam("P_FechaNacimiento", cliente.fechaNacimiento.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddStringParameter("P_FotoPerfil", cliente.fotoPerfil);
            sqlOperation.AddStringParameter("P_Contrasena", cliente.contrasena);
            sqlOperation.AddStringParameter("P_IBAN", cliente.IBAN);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var cliente = baseDTO as Cliente;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_CLIENTE_PR" };
            sqlOperation.AddIntParam("P_Id", cliente.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private Cliente BuildCliente(Dictionary<string, object> row)
        {
            return new Cliente()
            {
                Id = (int)row["Id"],
                Created = row["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created"],
                Updated = row["Updated"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Updated"],
                cedula = row["Cedula"].ToString(),
                nombre = row["Nombre"].ToString(),
                apellido = row["Apellidos"].ToString(),
                telefono = row["Telefono"] == DBNull.Value ? 0 : Convert.ToInt32(row["Telefono"]),
                correo = row["CorreoElectronico"].ToString(),
                direccion = row["Direccion"].ToString(),
                fotoCedula = row["FotoCedula"].ToString(),
                fechaNacimiento = row["FechaNacimiento"] == DBNull.Value ? DateOnly.MinValue : (DateOnly)row["FechaNacimiento"],
                fotoPerfil = row["FotoPerfil"].ToString(),
                contrasena = row["Contrasena"].ToString(),
                IBAN = row["IBAN"].ToString()
            };
        }


        public T RetrieveByEmail<T>(T cliente)
        {
            throw new NotImplementedException();
        }
    }
}
