using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdoProperties.Repository
{
    public class AdoDatabase : IAdoDatabase
    {
        public void SqlCommand(string query)
        {
            SqlConnection sc = new SqlConnection();
            SqlCommand com = new SqlCommand();
            sc.ConnectionString = DatabaseConfiguration.ConnectionString;
            sc.Open();
            com.Connection = sc;
            com.CommandText = (query);
            com.ExecuteNonQuery();
            sc.Close();
        }
        public List<T> SqlRead<T>(string query)
        {
            SqlConnection connection = new SqlConnection(DatabaseConfiguration.ConnectionString);
            connection.Open();
            SqlCommand comand = new SqlCommand(
            query, connection);
            var reading = comand.ExecuteReader();
            var list = DataReaderMapToList<T>(reading);
            connection.Close();
            return list;
        }
        public T SqlReadScaler<T>(string query)
        {
            SqlConnection connection = new SqlConnection(DatabaseConfiguration.ConnectionString);
            connection.Open();
            SqlCommand comand = new SqlCommand(
            query, connection);
            var reading = comand.ExecuteReader();
            var list = DataReaderMapToList<T>(reading)[0];
            connection.Close();
            return list;
        }
        private List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
