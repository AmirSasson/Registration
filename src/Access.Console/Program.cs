using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreHelloWorld.Models;
using Dapper;

namespace CoreHelloWorld
{
    /// <summary>
    /// The program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"hello from {Environment.MachineName}!");

            // Dapper.
            DapperCall();

            AdoCall();
            //// reader.

            CallService();
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        private static void CallService()
        {
        }



        /// <summary>
        /// Dappers the call.
        /// </summary>
        private static void DapperCall()
        {
            Console.WriteLine($"{nameof(DapperCall)} calling to get brands!");
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=REAL-APR-DEV;Initial Catalog=_dicts;Persist Security Info=True;User ID=rldev;Password=developme"))
            {
                con.Open();
                var brands = con.Query<Brand>(
                    new CommandDefinition(
                        commandText: "USP_GetBrands",
                        commandType: System.Data.CommandType.StoredProcedure));

                Console.WriteLine($"Got {brands.Count()} Brands");
            }
        }

        /// <summary>
        /// ADOs the call.
        /// </summary>
        private static void AdoCall()
        {
            Console.WriteLine($"{nameof(AdoCall)} calling to get brands!");
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=REAL-APR-DEV;Initial Catalog=_dicts;Persist Security Info=True;User ID=rldev;Password=developme"))
            {
                con.Open();
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("USP_GetBrands", con))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var brandId = reader.GetInt32(0);
                        Console.WriteLine($"Got {brandId} Brand!");
                    }
                }
            }
        }
    }
}
