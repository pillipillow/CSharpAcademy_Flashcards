using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Flashcards
{
    internal class DatabaseManager
    {
        //Appsetting.json config connection
        internal string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            return connectionString;

        }

        internal bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    return true; // Connection successful
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false; // Connection failed
            }
        }

        internal void CreateTable()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var sql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
                            BEGIN
                                CREATE TABLE Stacks (
                                    Id INT PRIMARY KEY IDENTITY(1,1),
                                    Name NVARCHAR(255) UNIQUE NOT NULL
                                );
                            END
                            
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards')
                            BEGIN
                                CREATE TABLE Flashcards (
                                    Id INT PRIMARY KEY IDENTITY(1,1),
                                    StackId INT NOT NULL,
                                    Question NVARCHAR(MAX) NOT NULL,
                                    Answer NVARCHAR(MAX) NOT NULL,
                                    CONSTRAINT FK_Flashcards_Stacks FOREIGN KEY (StackId) 
                                        REFERENCES Stacks(Id) ON DELETE CASCADE
                                );
                            END";

                connection.Execute(sql);
            }
        }

        internal bool CreateStack(string stackName)
        {
            try
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    var sql = "INSERT INTO Stacks (Name) VALUES (@Name)";
                    connection.Execute(sql, new { Name = stackName });
                    return true;
                }
            }
            catch (SqlException ex) 
            {
                return false;
            }
        }

    }
}
