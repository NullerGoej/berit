using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestBerit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        //connection  string
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        // GET: api/Task
        [HttpGet]
        public List<Tasks> Get()
        {
            var result = new List<Tasks>();

            //SQL command
            string getSql = "SELECT * FROM Tasks";

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
                                int tid = 0;
                                if(!reader.IsDBNull(0))
                                    tid = reader.GetInt32(0);

                                int uid = 0;
                                if(!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                DateTime timestamp = DateTime.MinValue;
                                if(!reader.IsDBNull(2))
                                    timestamp = reader.GetDateTime(2);

                                DateTime endstamp = DateTime.MinValue;
                                if (!reader.IsDBNull(3))
                                    endstamp = reader.GetDateTime(3);

                                string description = "";
                                if(!reader.IsDBNull(4))
                                    description = reader.GetString(4);

                                bool done = false; 
                                if(!reader.IsDBNull(5))
                                    done = reader.GetBoolean(5);

                                var bTasks = new Tasks(tid, uid, timestamp, endstamp, description, done);

                                result.Add(bTasks);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // GET: api/Task/id
        [HttpGet("{id}", Name = "GetTask")]
        public Tasks GetOneTask(int id)
        {
            var result = new List<Tasks>();

            string getSql = "SELECT * FROM Tasks WHERE tid =" + id;

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
                                int tid = 0;
                                if (!reader.IsDBNull(0))
                                    tid = reader.GetInt32(0);

                                int uid = 0;
                                if (!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                DateTime timestamp = DateTime.MinValue;
                                if (!reader.IsDBNull(2))
                                    timestamp = reader.GetDateTime(2);

                                DateTime endstamp = DateTime.MinValue;
                                if (!reader.IsDBNull(3))
                                    endstamp = reader.GetDateTime(3);

                                string description = "";
                                if (!reader.IsDBNull(4))
                                    description = reader.GetString(4);

                                bool done = false;
                                if (!reader.IsDBNull(5))
                                    done = reader.GetBoolean(5);

                                var bTasks = new Tasks(tid, uid, timestamp, endstamp, description, done);

                                result.Add(bTasks);
                            }
                        }
                    }
                }
            }

            return result.Single(x => x.tid.Equals(id));
        }

        // POST: api/Task
        [HttpPost]
        public void Post([FromBody] Tasks task)
        {
            string insertSql = "INSERT INTO Tasks(uid, timestamp, description, done) values (@uid, @timestamp, @description, 'false')";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@uid", task.uid);
                    insertCommand.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@description", task.description);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // PUT: api/Task/id
        [HttpPut("{id}")]
        public Tasks Put(int id, [FromBody] Tasks task)
        {
            Tasks tempTask = GetOneTask(id);
            string updateSql = "UPDATE Tasks SET endstamp = @endstamp, description = @description, done = @done WHERE tid = @tid";
            //if (tempTask.endstamp == DateTime.MinValue && task.done)
            //{
            //    updateSql = "UPDATE Tasks SET endstamp = @endstamp, description = @description, done = @done WHERE tid = @tid";
            //}
            //else
            //{
            //    updateSql = "UPDATE Tasks SET description = @description, done = @done WHERE tid = @tid";
            //}

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    

                    updateCommand.Parameters.AddWithValue("@tid", id);

                    if (tempTask.endstamp != task.endstamp)
                    {
                        updateCommand.Parameters.AddWithValue("@endstamp", task.endstamp);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@endstamp", tempTask.endstamp);
                    }

                    if (tempTask.description != task.description)
                    {
                        updateCommand.Parameters.AddWithValue("@description", task.description);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@description", tempTask.description);
                    }

                    updateCommand.Parameters.AddWithValue("@done", task.done);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetOneTask(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM Tasks WHERE tid = @tid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@tid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
