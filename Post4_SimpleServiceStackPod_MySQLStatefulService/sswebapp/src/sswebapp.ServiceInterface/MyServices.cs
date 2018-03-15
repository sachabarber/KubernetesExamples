using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ServiceStack;
using sswebapp.ServiceModel;

namespace sswebapp.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }

        public object Post(MySqlRequest request)
        {
            MySqlConnection connection = null;
            try
            {
                var server = request.SqlProps.Host;
                var port = request.SqlProps.Port;
                var uid = "root";
                var password = "password";
                string connectionString = $"server={server};port={port};user={uid};password={password};";

                using (connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"SELECT table_name, table_schema 
                                FROM INFORMATION_SCHEMA.TABLES 
                                WHERE TABLE_TYPE = 'BASE TABLE';";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        //Create a data reader and Execute the command
                        using (MySqlDataReader dataReader = cmd.ExecuteReader())
                        {

                            //Read the data and store them in the list
                            var finalResults = new List<string>();
                            while (dataReader.Read())
                            {
                                finalResults.Add($"Name = '{dataReader["table_name"]}', Schema = '{dataReader["table_schema"]}'");
                            }

                            //close Data Reader
                            dataReader.Close();

                            return new MySqlResponse
                            {
                                Results = finalResults
                            };
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return new MySqlResponse
                {
                    Results = new List<string>() {  ex.Message  + "\r\n" + ex.StackTrace}
                };
            }
            finally
            {
                if(connection != null)
                {
                    if(connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
