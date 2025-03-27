using Microsoft.Data.Sqlite;

namespace Running.Tracker
{
    internal class DatabaseManager
    {
        internal void CreateTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS running (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Duration TEXT,
                            Miles REAL
                        )";

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}