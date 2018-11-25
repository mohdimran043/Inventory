using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace MOI.Patrol.DataAccessLayer
{
    public class DataAccess
    {
        //public String constr;
        public IConfiguration _connectionstring { get; }
        
        // public DataAccess(IConfiguration configuration)
        //{
        //    _connectionstring = configuration;
        //    constr = _connectionstring["ConnectionStrings:DefaultConnection"];
        //}

        public String constr = "server=localhost;Port=5432;User Id=postgres;password=12345;Database=Patrols";
      

        // function that creates a list of an object from the given qry
        public List<T> PostGre_GetData<T>(string Qry) where T : new()
        {

            DataTable table = new DataTable();

            NpgsqlConnection cont = new NpgsqlConnection();
            cont.ConnectionString = constr;
            cont.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Qry, cont);
            da.Fill(table);
            cont.Close();
            cont.Dispose();


            List<T> list = new List<T>();
            var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
            {
                PropertyInfo = propertyInfo,
                Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
            }).ToList();

            foreach (var row in table.Rows.Cast<DataRow>())
            {
                T obj = new T();
                foreach (var typeProperty in typeProperties)
                {
                    object value = row[typeProperty.PropertyInfo.Name];
                    object safeValue = value == null || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                }
                list.Add(obj);
            }
            return list;
        }

        public string PostGre_ExScalar(string Qry)
        {
            string rcdstr = "";
            NpgsqlConnection cont = new NpgsqlConnection();
            cont.ConnectionString = constr;
            cont.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = cont;
            cmd.CommandText = Qry;
            rcdstr = cmd.ExecuteScalar().ToString();
            return rcdstr;
        }

        public int PostGre_ExNonQry(string Qry)
        {
            int rcdcnt = 0;
            NpgsqlConnection cont = new NpgsqlConnection();
            cont.ConnectionString = constr;
            cont.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = cont;
            cmd.CommandText = Qry;
            rcdcnt = cmd.ExecuteNonQuery();
            cont.Close();
            cont.Dispose();
            return rcdcnt;
        }

        public DataTable PostGre_GetDataTable (string Qry) 
        {

            DataTable table = new DataTable();

            NpgsqlConnection cont = new NpgsqlConnection();
            cont.ConnectionString = constr;
            cont.Open();
            //NpgsqlCommand cmd = new NpgsqlCommand();
            //cmd.CommandText = Qry;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(Qry, cont);

            //for (int i = 0; i<parameters.Count-1; i++)
            //{
            //    NpgsqlParameter paramt = new NpgsqlParameter();
            //    paramt.Direction = ParameterDirection.Input;
            //    paramt.ParameterName = parameters[i][0].Property("name").Value.ToString();
            //    paramt.Value = Convert.ToInt32(parameters[i][1].Property("value").Value.ToString());
            //    paramt.Direction = ParameterDirection.Input;
            //    paramt.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bigint;

            //    da.SelectCommand.Parameters.Add(paramt);
               
                   
            //}

            da.Fill(table);
            cont.Close();
            cont.Dispose();


           
            return table;
        }
    }
}
