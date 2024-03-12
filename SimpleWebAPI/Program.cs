using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace SimpleWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitDb();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void InitDb()
        {
            // Connection string for PostgreSQL database
            string connString = "Host=localhost;Username=myUser;Password=myPassword;Database=myDatabase";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Execute database initialization queries here
                using (NpgsqlCommand cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS person (name VARCHAR(255), age INT)", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

    }
}