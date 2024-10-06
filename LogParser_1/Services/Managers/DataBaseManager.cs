using LogParser_1.OPTIONS;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LogParser_1.Services.Managers
{
    internal class DataBaseManager
    {
        public static void CreateAndInsertLogsIntoDatabase(List<Dictionary<string, object>> logs, string dataBaseName = "log.sqlite")//Maybe leave one db file and change just table
        {
            InitializeDatabase(dataBaseName);
            try
            {
                using (var connection = new SqliteConnection(Options.connectionStringDefaultPath + dataBaseName))//@ for sql injection protection
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())////for performance upload in baches
                    {
                        foreach (var log in logs)
                        {
                            foreach (var key in log.Keys)
                            {
                                EnsureColumnExists(connection, key);
                            }

                            var keyCollumn = string.Join(", ", log.Keys);
                            var valueReferences = string.Join(", ", log.Keys.Select(l => $"@{l}"));

                            var command = connection.CreateCommand();
                            command.CommandText = $@"INSERT INTO LogEntries ({keyCollumn})
                                                        VALUES ({valueReferences})";//variables

                            foreach (var kvp in log)//parameters to variables
                            {
                                command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
                            }
                            command.ExecuteNonQuery();

                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Exeptions.ExeptionsHandler(ex);
            }
        }

        private static void InitializeDatabase(string dataBaseName)
        {
            try
            {
                using (var connection = new SqliteConnection(Options.connectionStringDefaultPath + dataBaseName))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS LogEntries (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT
                    )";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exeptions.ExeptionsHandler(ex);
            }
        }
        private static void EnsureColumnExists(SqliteConnection connection, string columnName)
        {
            var command = connection.CreateCommand();
            command.CommandText = $@"PRAGMA table_info(LogEntries);";//to get what collumns exist

            var reader = command.ExecuteReader();
            bool columnExists = false;

            while (reader.Read())
            {
                if (reader.GetString(1).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    columnExists = true;
                    break;
                }
            }

            if (!columnExists)
            {
                var alterCommand = connection.CreateCommand();
                alterCommand.CommandText = $@"
                ALTER TABLE LogEntries ADD COLUMN {columnName} TEXT;";
                alterCommand.ExecuteNonQuery();
            }
        }
    }
}
