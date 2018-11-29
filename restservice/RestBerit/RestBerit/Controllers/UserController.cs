using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            //SQL command
            string getSql = "SELECT * FROM Users";

            //SQL connection
            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                //SQL command
                dbConnection.Open();

                //SQL command
                using (SqlCommand getSpecificCommand = new SqlCommand(getSql, dbConnection))
                {
                    //SQL data reader
                    using (SqlDataReader reader = getSpecificCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //SQL data reader + reading data
                            while (reader.Read())
                            {

                                int uid = 0;
                                if (!reader.IsDBNull(0))
                                    uid = reader.GetInt32(0);

                                string username = "";
                                if(!reader.IsDBNull(1))
                                    username = reader.GetString(1);

                                string password = ""; 
                                if(!reader.IsDBNull(2))
                                    password = reader.GetString(2);

                                string firstname = "";
                                if (!reader.IsDBNull(3))
                                    firstname = reader.GetString(3);

                                string lastname = "";
                                if (!reader.IsDBNull(4))
                                    firstname = reader.GetString(4);

                                int asid = 0;
                                if (!reader.IsDBNull(5))
                                    asid = reader.GetInt32(5);

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
        [HttpGet("{id}", Name = "GetUser")]
        public Users GetOneUser(int id)
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
                                int uid = 0;
                                if (!reader.IsDBNull(0))
                                    uid = reader.GetInt32(0);

                                string username = "";
                                if (!reader.IsDBNull(1))
                                    username = reader.GetString(1);

                                string password = "";
                                if (!reader.IsDBNull(2))
                                    password = reader.GetString(2);

                                string firstname = "";
                                if (!reader.IsDBNull(3))
                                    firstname = reader.GetString(3);

                                string lastname = "";
                                if (!reader.IsDBNull(4))
                                    firstname = reader.GetString(4);

                                int asid = 0;
                                if (!reader.IsDBNull(5))
                                    asid = reader.GetInt32(5);

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
            string insertSql = "INSERT INTO Users(username, password, firstname, lastname, asid) values (@username, @password, @firstname, @lastname, @asid)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
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
            string updateSql = "UPDATE Users SET username = @username, password = @password, firstname = @firstname, lastname = @lastname, asid = @asid WHERE uid = @uid";
            Users tempUser = GetOneUser(id);

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {             
                    updateCommand.Parameters.AddWithValue("@uid", id);

                    if (user.username != "")
                    {
                        updateCommand.Parameters.AddWithValue("@username", user.username);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@username", tempUser.username);
                    }

                    if (user.password != "")
                    {
                        updateCommand.Parameters.AddWithValue("@password", user.password);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@password", tempUser.password);
                    }

                    if (user.firstname != "")
                    {
                        updateCommand.Parameters.AddWithValue("@firstname", user.firstname);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@firstname", tempUser.firstname);
                    }

                    if (user.lastname != "")
                    {
                        updateCommand.Parameters.AddWithValue("@lastname", user.lastname);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@lastname", tempUser.lastname);
                    }

                    if (user.asid != tempUser.asid)
                    {
                        updateCommand.Parameters.AddWithValue("@asid", user.asid);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@asid", tempUser.asid);
                    }

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetOneUser(id);
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
