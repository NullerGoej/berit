using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestBerit.Model;

namespace RestBerit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        // GET: api/User
        [HttpGet]
        public List<Users> Get()
        {
            var result = new List<Users>();

            string getSql = "SELECT * FROM Users";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand getSpecificCommand = new SqlCommand(getSql, dbConnection))
                {
                    using (SqlDataReader reader = getSpecificCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int uid = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                string password = reader.GetString(2);
                                string firstname = reader.GetString(3);
                                string lastname = reader.GetString(4);
                                int asid = reader.GetInt32(5);

                                var bUsers = new Users(uid, username, password, firstname, lastname, asid);

                                result.Add(bUsers);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // GET: api/User/id
        [HttpGet("{id}", Name = "Get")]
        public Users GetSpecific(int id)
        {
            var result = new List<Users>();

            string getSql = "SELECT * FROM Users WHERE uid =" + id;

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand getSpecificCommand = new SqlCommand(getSql, dbConnection))
                {
                    using (SqlDataReader reader = getSpecificCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int uid = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                string password = reader.GetString(2);
                                string firstname = reader.GetString(3);
                                string lastname = reader.GetString(4);
                                int asid = reader.GetInt32(5);

                                var bUsers = new Users(uid, username, password, firstname, lastname, asid);

                                result.Add(bUsers);
                            }
                        }
                    }
                }
            }

            return result.Single(x => x.uid.Equals(id));
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] Users user)
        {
            string insertSql = "INSERT INTO Users(uid, username, password, firstname, lastname, asid) values (@uid, @username, @password, @firstname, @lastname, @asid)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@uid", user.uid);
                    insertCommand.Parameters.AddWithValue("@username", user.username);
                    insertCommand.Parameters.AddWithValue("@password", user.password);
                    insertCommand.Parameters.AddWithValue("@firstname", user.firstname);
                    insertCommand.Parameters.AddWithValue("@lastname", user.lastname);
                    insertCommand.Parameters.AddWithValue("@asid", user.asid);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // PUT: api/User/id
        [HttpPut("{id}")]
        public Users Put(int id, [FromBody] Users user)
        {
            string updateSql = "UPDATE Users SET (username, password, firstname, lastname, asid) values (@username, @password, @firstname, @lastname, @asid) Where uid = @uid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    updateCommand.Parameters.AddWithValue("@uid", id);

                    if (user.username != null)
                    { updateCommand.Parameters.AddWithValue("@username", user.username); }

                    if (user.password != null)
                    { updateCommand.Parameters.AddWithValue("@password", user.password); }

                    if (user.firstname != null)
                    { updateCommand.Parameters.AddWithValue("@firstname", user.firstname); }

                    if (user.lastname != null)
                    { updateCommand.Parameters.AddWithValue("@lastname", user.lastname); }

                    if (user.asid != null)
                    { updateCommand.Parameters.AddWithValue("@asid", user.asid); }

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetSpecific(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM Users WHERE uid = @uid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@uid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

    }
}
