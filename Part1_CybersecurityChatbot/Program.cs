using System;
using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Entry point for the Cybersecurity Awareness Chatbot console application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Application entry point — creates and starts the ChatBot.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        static void Main(string[] args)
        {
            Console.Title = "CyberBot — Cybersecurity Awareness Chatbot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Test MySQL connection before starting chatbot
            if (TestDatabaseConnection())
            {
                Console.WriteLine("✓ Database connected successfully!\n");
            }
            else
            {
                Console.WriteLine("⚠ Warning: Database connection failed. Continuing without database...\n");
            }

            var bot = new ChatBot();
            bot.Start();

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey();
        }

        /// <summary>
        /// Tests the MySQL database connection.
        /// </summary>
        /// <returns>True if connection successful, false otherwise.</returns>
        static bool TestDatabaseConnection()
        {
            // TODO: Replace with your actual MySQL credentials
           string connectionString = "Server=localhost;Database=chatbot_db;User=chatbot_user;Password=ChatBot123!;Port=3306;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connecting to MySQL database...");
                    
                    // Optional: Get MySQL version
                    string query = "SELECT VERSION()";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    string version = cmd.ExecuteScalar().ToString();
                    Console.WriteLine($"MySQL Version: {version}");
                    
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}