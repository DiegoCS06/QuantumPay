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

            sqlOperation.AddStringParameter("P_cedula", cliente.cedula);
            sqlOperation.AddStringParameter("P_nombre", cliente.nombre);
            sqlOperation.AddStringParameter("P_apellidos", cliente.apellido);
            sqlOperation.AddStringParameter("P_telefono", cliente.telefono);
            sqlOperation.AddStringParameter("P_correoElectronico", cliente.correo);
            sqlOperation.AddStringParameter("P_direccion", cliente.direccion);
            sqlOperation.AddStringParameter("P_fotoCedula", cliente.fotoCedula);
            sqlOperation.AddDateTimeParam("P_fechaNacimiento", cliente.fechaNacimiento.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddStringParameter("P_fotoPerfil", cliente.fotoPerfil);
            sqlOperation.AddStringParameter("P_contrasena", cliente.contrasena);
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

            sqlOperation.AddStringParameter("P_cedula", cedula);

            var lstResult = _sqlDao.ExecuteQueryProcedure(sqlOperation);

            if (lstResult.Count > 0)
            {
                var row = lstResult[0];
                var cliente = BuildCliente(row);

                return (T)Convert.ChangeType(cliente, typeof(T));
            }

            return default(T);
        }

        public T RetrieveByTelefono<T>(string telefono)
        {
            var sqlOperation = new SQLOperation() { ProcedureName = "RET_CLIENTE_BY_TELEFONO_PR" };

            sqlOperation.AddStringParameter("P_telefono", telefono);

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

            sqlOperation.AddIntParam("P_idCliente", Id);

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

            sqlOperation.AddStringParameter("P_correoElectronico", correo);

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

            sqlOperation.AddIntParam("P_idCliente", cliente.Id);
            sqlOperation.AddStringParameter("P_cedula", cliente.cedula);
            sqlOperation.AddStringParameter("P_nombre", cliente.nombre);
            sqlOperation.AddStringParameter("P_apellidos", cliente.apellido);
            sqlOperation.AddStringParameter("P_telefono", cliente.telefono);
            sqlOperation.AddStringParameter("P_correoElectronico", cliente.correo);
            sqlOperation.AddStringParameter("P_direccion", cliente.direccion);
            sqlOperation.AddStringParameter("P_fotoCedula", cliente.fotoCedula);
            sqlOperation.AddDateTimeParam("P_fechaNacimiento", cliente.fechaNacimiento.ToDateTime(TimeOnly.MinValue));
            sqlOperation.AddStringParameter("P_fotoPerfil", cliente.fotoPerfil);
            sqlOperation.AddStringParameter("P_contrasena", cliente.contrasena);
            sqlOperation.AddStringParameter("P_IBAN", cliente.IBAN);

            _sqlDao.ExecuteProcedure(sqlOperation);
        }


        public override void Delete(BaseDTO baseDTO)
        {
            var cliente = baseDTO as Cliente;
            var sqlOperation = new SQLOperation() { ProcedureName = "DEL_CLIENTE_PR" };
            sqlOperation.AddIntParam("P_idCliente", cliente.Id);
            _sqlDao.ExecuteProcedure(sqlOperation);
        }

        //Metodo que convierte diccionario en un usuario
        private Cliente BuildCliente(Dictionary<string, object> row)
        {
            return new Cliente()
            {
                Id = (int)row["idCliente"],
                cedula = row["cedula"].ToString(),
                nombre = row["nombre"].ToString(),
                apellido = row["apellidos"].ToString(),
                telefono = row["telefono"].ToString(),
                correo = row["correoElectronico"].ToString(),
                direccion = row["direccion"].ToString(),
                fotoCedula = row["fotoCedula"].ToString(),
                fechaNacimiento = row["fechaNacimiento"] == DBNull.Value? DateOnly.MinValue: DateOnly.FromDateTime((DateTime)row["fechaNacimiento"]),
                fotoPerfil = row["fotoPerfil"].ToString(),
                contrasena = row["contrasena"].ToString(),
                IBAN = row["IBAN"].ToString()
            };
        }
    }
}
