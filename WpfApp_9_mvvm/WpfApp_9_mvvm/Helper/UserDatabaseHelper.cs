using Microsoft.Data.Sqlite;
using System.IO;

public static class UserDatabaseHelper
{
    private static string _databasePath = "UserDatabase.db";
    private static string _connectionString = $"Data Source={_databasePath}";

    public static void InitializeDatabase()
    {
        if (!File.Exists(_databasePath))
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS User (
                        Name TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Age INTEGER,
                        Address TEXT Not Null
                    )";

                command.ExecuteNonQuery();
            }
        }
    }

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}