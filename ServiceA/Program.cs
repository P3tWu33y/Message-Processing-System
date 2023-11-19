using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        try
        {
            // Generate a random number
            Random random = new Random();
            int randomNumber = random.Next(1, 101);

            // Connection string (replace placeholders with actual values)
            string connectionString = "Server=localhost; Database=main; User Id=sa; Password=220497; Encrypt=False; TrustServerCertificate=True;";


            // SQL command to insert the random number into the database
            string insertQuery = "INSERT INTO [dbo].messages (messages) VALUES (@RandomNumber)";


            // Create a SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Add a parameter for the random number
                    command.Parameters.AddWithValue("@RandomNumber", randomNumber);

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the insertion was successful
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Random number {randomNumber} inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error: No rows affected. The random number may not have been inserted.");
                    }
                }
            }
        }
        catch (Exception generalErr)
        {
            // Handle exceptions
            Console.WriteLine($"An error occurred: {generalErr.Message}");
        }

        Console.ReadLine(); // Keep console window open
    }
}
