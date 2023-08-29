using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Apartaments.Infrastructure
{
    public static class SqlLiteManager
    {
        private enum TableExists : Int64
        {
            None,
            Yes
        }

        private const string pathToDb = "~/../Apartaments.db";
        private const string connectionString = $"DataSource={pathToDb};Mode=ReadWrite;";

        private static bool initialized;

        private static SqliteConnection connection;

        public static void Init() 
        {
            initialized = false;

            using (var connection = new SqliteConnection($"DataSource={pathToDb};Mode=ReadWriteCreate;"))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = File.ReadAllText(@"./Infrastructure/SqlQuerys/ExistsTableQuery.sql");
                    cmd.Parameters.AddWithValue("$name", "Apartaments");

                    switch ((Int64)cmd.ExecuteScalar())
                    {
                        case (Int64)TableExists.None:
                            using (var cmd_tb = connection.CreateCommand())
                            {
                                cmd_tb.CommandText = File.ReadAllText(@"./Infrastructure/SqlQuerys/CreateTable_Apartaments.sql");
                                cmd_tb.ExecuteNonQuery();
                            }
                            break;
                    }

                    cmd.CommandText = File.ReadAllText(@"./Infrastructure/SqlQuerys/ExistsTableQuery.sql");
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("$name", "history_apartment_prices");

                    switch ((Int64)cmd.ExecuteScalar())
                    {
                        case (Int64)TableExists.None:
                            using (var cmd_tb = connection.CreateCommand())
                            {
                                cmd_tb.CommandText = File.ReadAllText(@"./Infrastructure/SqlQuerys/CreateTable_HistoryApartmentPrices.sql");
                                cmd_tb.ExecuteNonQuery();
                            }
                            break;
                    }
                }
            }

            connection = new SqliteConnection(connectionString);
            connection.Open();

            initialized = true;
        }

        public static void Close()
        {
            if (connection is SqliteConnection c) c.Close();
        }

        public static void SeedData()
        {
            if (!initialized) return;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(_id) FROM Apartaments";

                    if ((Int64)cmd.ExecuteScalar() == 0)
                    {
                        cmd.CommandText = File.ReadAllText(@"./Infrastructure/SqlQuerys/SeedDataQuery.sql");
                        cmd.ExecuteNonQuery();
                    }
                }

                var start_date = new DateTime(2020, 1, 1);
                int start_price = 1000000;
                int end_price = 6000000;

                var rnd_date = new Random();
                var rnd_price = new Random();

                int range_date = (DateTime.Today - start_date).Days;

                using (var cmd = connection.CreateCommand())
                {  
                    cmd.CommandText = "DELETE FROM history_apartment_prices";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT _id FROM Apartaments";

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            using (var cmd_insert = connection.CreateCommand())
                            {
                                while (dr.Read())
                                {
                                    for (int i = 0; i < 12; i++)
                                    {
                                        cmd_insert.CommandText = $@"
                                        INSERT INTO history_apartment_prices 
                                        (
                                            id_apartment
                                            ,date_update_price
                                            ,price
                                        )
                                        VALUES
                                        (
                                            {dr["_id"]}
                                            ,'{start_date.AddDays(rnd_date.Next(range_date)): yyyy-MM-dd HH:mm:ss}'
                                            ,{rnd_price.Next(start_price, end_price)}
                                        )";

                                        cmd_insert.ExecuteNonQuery();
                                    }

                                }
                            }

                        }
                    }
                }
            }
            
        }

        public static SqliteDataReader GetReader(string query, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(query) ||
                string.IsNullOrWhiteSpace(query))
                throw new Exception("param 'query' is empty. ");

            var cmd = connection.CreateCommand();
            cmd.CommandText = query;

            if (parameters is Dictionary<string, object> args)
                foreach (var key in args.Keys)
                    cmd.Parameters.AddWithValue(key, args[key]);

            var reader = cmd.ExecuteReader();

            return reader;
        }
    }
}
