
using System.Configuration;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Running.Tracker
{
    internal class RunningController
    {
        string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = "DELETE FROM running WHERE Id = @Id";

                int success = connection.Execute(sql, new { Id = id });

                if (success > 0)
                {
                    Console.WriteLine($"\nSuccessfully deleted record with ID {id}\n");
                }
                else
                {
                    Console.WriteLine($"\nFailed to delete record with ID {id}\n");
                }
            }
        }

        internal List<RunningSession> GetAll()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM running";

                return connection.Query<RunningSession>(sql).ToList();
            }
        }

        internal RunningSession? GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM running WHERE Id = @Id";

                var record = connection.QueryFirstOrDefault<RunningSession>(sql, new { Id = id });

                return record;
            }
        }

        internal void Get()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM running";

                var tableData = connection.Query<RunningSession>(sql).ToList();     // Automatically maps instead of manually doing it like in ADO.NET

                if (!tableData.Any())
                {
                    Console.WriteLine("\nNo rows found!\n");
                }

                Console.WriteLine("\n");
                TableVisualization.ShowTable(tableData);
            }
        }

        internal void Post(RunningSession run)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO running (Date, Duration, Miles) VALUES (@Date, @Duration, @Miles)";

                    var rowsAffected = connection.Execute(sql, new
                    {
                        Date = run.Date,
                        Duration = run.Duration,
                        Miles = run.Miles
                    });
                    
                    if (rowsAffected == 0)
                    {
                        throw new Exception("\nInsert Failed\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal void Update(RunningSession record)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = $"UPDATE running SET Date = @Date, Duration = @Duration, Miles = @Miles WHERE Id = {record.Id}";

                int success = connection.Execute(sql, new { 
                    Date = record.Date, 
                    Duration = record.Duration, 
                    Miles = record.Miles,
                    Id = record.Id
                });

                if (success > 0)
                {
                    Console.WriteLine($"\nSuccessfully updated ID {record.Id}");
                }
                else
                {
                    Console.WriteLine($"\nFailed to update ID {record.Id}");
                }
            }
        }
    }
}