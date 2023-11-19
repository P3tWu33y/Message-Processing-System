using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using StackExchange.Redis;
using System.Threading;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        try
        {
            // Connection strings (replace with your actual connection strings)
            string mssqlConnectionString = "Server=localhost; Database=main; User Id=sa; Password=220497; Encrypt=False; TrustServerCertificate=True;";
            string redisConnectionString = "localhost:6379,abortConnect=false";

            // SQL query to retrieve new messages
            string selectQuery = "SELECT [id], (messages) FROM [main].[dbo].[messages] WHERE isProcessed = 0"; // Assuming there's a flag (Processed) indicating whether the message is processed

            // Interval between each check (milliseconds)
            int checkInterval = 5000; // 5 seconds, adjust as needed

            while (true)
            {
                try
                {
                    Console.WriteLine("Checking for new messages...");

                    // List to store processed messages
                    List<long> processedMessageIds = new List<long>();

                    // Retrieve new messages from MSSQL
                    using (SqlConnection connection = new SqlConnection(mssqlConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        // Process the data (double each value)
                                        int originalValue = Convert.ToInt32(reader["messages"]);
                                        int processedValue = originalValue * 2;

                                        Console.WriteLine($"Processing message with ID {reader["id"]} - Original Value: {originalValue}, Processed Value: {processedValue}");

                                        // Convert the processed data to JSON
                                        string jsonData = JsonConvert.SerializeObject(new { OriginalValue = originalValue, ProcessedValue = processedValue });

                                        // Upload the processed data to Redis
                                        UploadToRedis(redisConnectionString, reader["id"].ToString(), jsonData);

                                        // Store the processed message ID
                                        processedMessageIds.Add((long)reader["id"]);
                                    }
                                    catch (Exception processException)
                                    {
                                        Console.WriteLine($"Error processing data: {processException.Message}");
                                    }
                                }
                            }
                        }
                    }

                    // Close the SqlDataReader before processing and updating
                    Console.WriteLine("Closing SqlDataReader...");
                    Console.WriteLine("Updating the database...");

                    using (SqlConnection connection = new SqlConnection(mssqlConnectionString))
                    {
                        connection.Open();

                        // Update the flag in MSSQL to mark the messages as processed
                        foreach (long messageId in processedMessageIds)
                        {
                            MarkMessageAsProcessed(connection, messageId);
                        }
                    }

                    Console.WriteLine("Finished processing messages. Waiting for the next check...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                // Wait for the specified interval before the next check
                Thread.Sleep(checkInterval);
            }
        }
        catch (Exception mainException)
        {
            // Handle exceptions in the main loop
            Console.WriteLine($"Main loop error: {mainException.Message}");
        }
    }

    static void UploadToRedis(string redisConnectionString, string key, string jsonData)
    {
        try
        {
            // ConnectionMultiplexer implements the low-level redis operations
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString))
            {
                // Database 0 is the default, change the number if you are using a different database
                IDatabase db = redis.GetDatabase();

                // Add the processed value as JSON to Redis
                db.StringSet(key, jsonData);

                Console.WriteLine($"Processed data for ID {key} uploaded to Redis: {jsonData}");
            }
        }
        catch (Exception redisException)
        {
            // Handle Redis connection exception
            Console.WriteLine($"Redis Connection Error: {redisException.Message}");
        }
    }

    static void MarkMessageAsProcessed(SqlConnection connection, long messageId)
    {
        try
        {
            // Implement this method to update the flag (isProcessed) in MSSQL to mark the message as processed
            // For example: 
            using (SqlCommand updateCommand = new SqlCommand("UPDATE [main].[dbo].[messages] SET isProcessed = 1 WHERE id = @ID", connection))
            {
                updateCommand.Parameters.AddWithValue("@ID", messageId);
                updateCommand.ExecuteNonQuery();
            }

            Console.WriteLine($"Message with ID {messageId} marked as completed.");
        }
        catch (Exception markProcessedException)
        {
            // Handle exception when marking message as processed
            Console.WriteLine($"Error marking message as processed: {markProcessedException.Message}");
        }
    }
}
