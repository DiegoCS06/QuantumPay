using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess.DAOs
{

    /*
     * clase con instrucciones de lo que tiene que hacer el sql dao
     */

    public class SQLOperation
    {

        public string ProcedureName { get; set; }
        public List<SqlParameter> Parameters { get; set; }
        public SQLOperation()
        {
            Parameters = new List<SqlParameter>();
        }

        public void AddStringParameter(string ParamName, string ParamValue)
        {
            Parameters.Add(new SqlParameter(ParamName, ParamValue));
        }

        public void AddIntParam(string paramName, int paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDoubleParam(string paramName, double paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDateTimeParam(string paramName, DateTime paramValue)
        {
            Parameters.Add(new SqlParameter(paramName, paramValue));
        }

        public void AddDecimalParam(string paramName, decimal value, byte precision, byte scale)
        {
            var param = new SqlParameter(paramName, System.Data.SqlDbType.Decimal)
            {
                Precision = precision,
                Scale = scale,
                Value = value
            };
            Parameters.Add(param);
        }
        internal void AddVarcharParam(string paramName, string value, int size)
        {
            var param = new SqlParameter(paramName, System.Data.SqlDbType.NVarChar, size)
            {
                Value = value ?? (object)DBNull.Value
            };
            Parameters.Add(param); // Usa tu lista interna de parámetros, igual que con otros métodos
        }

        internal void AddStringParameter(string v, int idCuentaBancaria)
        {
            throw new NotImplementedException();
        }
          
        public void AddParam(string paramName, object value)
        {
            Parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
        }
    }
}