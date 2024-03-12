using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace SimpleWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly string connString = "Host=myServer;Username=myUser;Password=myPassword;Database=myDatabase";

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            bool isUp = CheckDatabaseStatus();
            return Ok(new { message = "database-status", value = isUp });
        }

        private bool CheckDatabaseStatus()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT 1", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null && result.ToString() == "1";
                    }
                    //conn.Close();
                }
            }
            catch
            {
                return false; // Database is down or inaccessible
            }
        }





        [HttpGet("persons")]
        public IActionResult GetPersons()
        {
            string query = $"SELECT name, age FROM person";
            var result = ExecuteQuery(query);

            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }



        [HttpGet("{name}")]
        public IActionResult GetPersonByName(string name)
        {
            string query = $"SELECT name, age FROM person WHERE name = '{name}'";
            var result = ExecuteQuery(query);

            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        private List<Person> ExecuteQuery(string query)
        {
            List<Person> result = new List<Person>();

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Person
                            {
                                Name = reader.GetString(0),
                                Age = reader.GetInt32(1)
                            });
                        }
                    }
                }
                conn.Close();
            }

            return result;
        }





        [HttpPost("{name}/{age}")]
        public IActionResult AddPerson(string name, int age)
        {
            string query = $"INSERT INTO person (name, age) VALUES ('{name}', {age})";
            ExecuteNonQuery(query);
            return Ok("Success!");
        }

        private void ExecuteNonQuery(string query)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }

    public class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }



}

