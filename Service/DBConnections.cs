using Microsoft.Data.Sqlite;
using System;
using System.Data.SQLite;
using System.IO;

namespace DesktopApplication.Service
{
    public class DBConnections
    {
        public static void CreateDatabase()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ElectronPoC.sqlite");

            bool newDatabase = !File.Exists(path); 

            if (newDatabase)
            {
                SQLiteConnection.CreateFile(path);
            }

            string connectionString = $"Data Source={path};Version=3;";
            using (var m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();

                using (var command = new SQLiteCommand(m_dbConnection))
                {
                    // Corporation Table
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Corporation (
                            CorporationId INTEGER PRIMARY KEY AUTOINCREMENT,
                            CorporationName TEXT NOT NULL,
                            IsSync INTEGER NOT NULL DEFAULT 0
                        );";
                    command.ExecuteNonQuery();

                    // Branch Table
                    command.CommandText = @"
                       CREATE TABLE IF NOT EXISTS Branch (
                           BranchId INTEGER PRIMARY KEY AUTOINCREMENT,
                           CorporationId INTEGER NOT NULL,
                           BranchName TEXT NOT NULL,
                           BranchCreatedDate TEXT NOT NULL,
                           IsSync INTEGER NOT NULL DEFAULT 0,
                           FOREIGN KEY (CorporationId) REFERENCES Corporation(CorporationId)
                       );";
                    command.ExecuteNonQuery();


                    // User Table
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS User (
                            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                            CorporationId INTEGER NOT NULL,
                            BranchId INTEGER NOT NULL,
                            Username TEXT NOT NULL,
                            Role TEXT NOT NULL,
                            CreatedDate TEXT NOT NULL,
                            IsSync INTEGER NOT NULL DEFAULT 0,
                            FOREIGN KEY (CorporationId) REFERENCES Corporation(CorporationId),
                            FOREIGN KEY (BranchId) REFERENCES Branch(BranchId)
                        );";
                    command.ExecuteNonQuery();

                    // Category Table
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Category (
                            CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
                            BranchId INTEGER NOT NULL,
                            CorporationId INTEGER NOT NULL,
                            CreatedByUserId INTEGER NOT NULL,
                            CategoryName TEXT NOT NULL,
                            CreatedDate TEXT NOT NULL,
                            IsSync INTEGER NOT NULL DEFAULT 0,
                            FOREIGN KEY (CorporationId) REFERENCES Corporation(CorporationId),
                            FOREIGN KEY (BranchId) REFERENCES Branch(BranchId),
                            FOREIGN KEY (CreatedByUserId) REFERENCES User(UserId)
                        );";
                    command.ExecuteNonQuery();

                    // Optional: Insert some sample data if the database is newly created
                    if (newDatabase)
                    {
                        command.CommandText = "INSERT INTO Corporation (CorporationName) VALUES ('Default Corp')";
                        command.ExecuteNonQuery();
                    }
                }

                m_dbConnection.Close();
            }
        }
    }
}


