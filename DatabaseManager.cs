using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Flashcards
{
    internal class DatabaseManager
    {
        //Appsetting.json config connection
        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            return connectionString;

        }

        public bool TestConnection()
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
    }
}
