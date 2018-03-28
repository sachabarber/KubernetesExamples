using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using ServiceStack;
using sswebapp.ServiceModel;

namespace sswebapp.ServiceInterface
{
    public class MyServices : Service
    {
        /// <summary>
        /// Set in <c>sswebapp.Startup.cs</c>. This is just for demo purposes only
        /// this is not a great design, but for this quick and dirty demo it does the job
        /// </summary>
        public static IEnumerable<KeyValuePair<string,string>> AllVariablesFromStartup { get; set; }


        public object Get(Hello request)
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

        public object Get(ShowSettingRequest request)
        {
            try
            {
                var allVars = new List<string>();
                foreach (var kvp in AllVariablesFromStartup)
                {
                    allVars.Add($"Key: {kvp.Key}, Value: {kvp.Value}");
                }

                return new ShowSettingResponse { Results = allVars };
            }
            catch(Exception ex)
            {
                return new ShowSettingResponse { Results = new List<string>() {ex.Message } };
            }
        }
    }
}
