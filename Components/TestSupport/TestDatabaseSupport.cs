using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace TestSupport
{
    public class TestDatabaseSupport
    {
        public static string RegistrationConnectionString => ConnectionString("tracker_registration_dotnet_test");
        public static string BacklogConnectionString => ConnectionString("tracker_backlog_dotnet_test");
        public static string AllocationsConnectionString => ConnectionString("tracker_allocations_dotnet_test");
        public static string TimesheetsConnectionString => ConnectionString("tracker_timesheets_dotnet_test");

        private const string DbUser = "tracker_dotnet";
        private const string DbPassword = "password";

        public static string ConnectionString(string database) =>
            $"server=127.0.0.1;uid={DbUser};pwd={DbPassword};database={database}";

        private readonly string _connectionString;

        public TestDatabaseSupport(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void ExecSql(string sql)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public IList<IDictionary<string, object>> QuerySql(string sql)
        {
            var result = new List<IDictionary<string, object>>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var rowData = Enumerable.Range(0, reader.FieldCount)
                                    .ToDictionary(reader.GetName, reader.GetValue);

                                result.Add(rowData);
                            }

                            reader.NextResult();
                        }
                    }
                }
            }

            return result;
        }

        public void TruncateAllTables()
        {
            var dbName = new MySqlConnectionStringBuilder(_connectionString).Database;

            var tableNameSql = $@"set foreign_key_checks = 0;
                select table_name FROM information_schema.tables
                where table_schema='{dbName}' and table_name != 'schema_version';";

            var truncateSql = "";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = tableNameSql;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var table = reader.GetString(0);
                                truncateSql += $"truncate {table};";
                            }

                            reader.NextResult();
                        }
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = truncateSql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}