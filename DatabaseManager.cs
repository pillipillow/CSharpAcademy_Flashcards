using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Flashcards.Models;

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
                                    FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
                                );
                            END";

                connection.Execute(sql);
            }
        }

        internal void CreateStack(string stackName)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var sql = "INSERT INTO Stacks (Name) VALUES (@Name)";

                connection.Execute(sql, new { Name = stackName });
            }
        }

        internal bool CheckStactExist(string stackname)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                string sql = "SELECT COUNT(1) FROM Stacks WHERE Name = @Name";

                int count = connection.ExecuteScalar<int>(sql, new { Name = stackname });
                return count > 0;
            }
        }

        internal List<Stack> GetStacks()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var sql = "SELECT * FROM Stacks ORDER BY Id ASC";

                return connection.Query<Stack>(sql).ToList();
            }

        }


    }
}
