using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class TaskRepeatController : ControllerBase
    {
        //connection  string
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        // GET: api/TaskRepeat
        [HttpGet]
        public List<TaskRepeat> Get()
        {
            var result = new List<TaskRepeat>();

            //SQL command
            string getSql = "SELECT * FROM TaskRepeat";

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
                                int trid = 0;
                                if (!reader.IsDBNull(0))
                                    trid = reader.GetInt32(0);

                                int uid = 0;
                                if (!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                int count = 0;
                                if (!reader.IsDBNull(2))
                                    count = reader.GetInt32(2);

                                string description = "";
                                if (!reader.IsDBNull(3))
                                    description = reader.GetString(3);

                                DateTime createdate = DateTime.MinValue;
                                if (!reader.IsDBNull(4))
                                    createdate = reader.GetDateTime(4);

                                DateTime completedate = DateTime.MinValue;
                                if (!reader.IsDBNull(5))
                                    completedate = reader.GetDateTime(5);

                                bool done = false;
                                if (!reader.IsDBNull(6))
                                    done = reader.GetBoolean(6);

                                var bTasks = new TaskRepeat(trid, uid, count, description, createdate, completedate, done);

                                result.Add(bTasks);
                            }
                        }
                    }
                }
            }
            return result;
        }

        // GET: api/TaskRepeat/id
        [HttpGet("{id}", Name = "GetTaskRepeat")]
        public TaskRepeat GetOneTaskRepeat(int id)
        {
            var result = new List<TaskRepeat>();

            string getSql = "SELECT * FROM TaskRepeat WHERE trid =" + id;

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
                                int trid = 0;
                                if (!reader.IsDBNull(0))
                                    trid = reader.GetInt32(0);

                                int uid = 0;
                                if (!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                int count = 0;
                                if (!reader.IsDBNull(2))
                                    count = reader.GetInt32(2);

                                string description = "";
                                if (!reader.IsDBNull(3))
                                    description = reader.GetString(3);

                                DateTime createdate = DateTime.MinValue;
                                if (!reader.IsDBNull(4))
                                    createdate = reader.GetDateTime(4);

                                DateTime completedate = DateTime.MinValue;
                                if (!reader.IsDBNull(5))
                                    completedate = reader.GetDateTime(5);


                                bool done = false;
                                if (!reader.IsDBNull(6))
                                    done = reader.GetBoolean(6);

                                var bTaskr = new TaskRepeat(trid, uid, count, description, createdate, completedate, done);

                                result.Add(bTaskr);
                            }
                        }
                    }
                }
            }

            return result.Single(x => x.trid.Equals(id));
        }

        // POST: api/TaskRepeat
        [HttpPost]
        public void Post([FromBody] TaskRepeat taskr)
        {
            string insertSql = "INSERT INTO TaskRepeat(uid, count, description, createdate, completedate, done) values (@uid, @count, @description, @createdate, @completedate, 'false')";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@uid", taskr.uid);
                    insertCommand.Parameters.AddWithValue("@count", taskr.count);
                    insertCommand.Parameters.AddWithValue("@description", taskr.description);
                    insertCommand.Parameters.AddWithValue("@createdate", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@completedate", taskr.completedate);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // PUT: api/TaskRepeat/id
        [HttpPut("{id}")]
        public TaskRepeat Put(int id, [FromBody] TaskRepeat taskr)
        {
            TaskRepeat tempTaskRepeat = GetOneTaskRepeat(id);
            string updateSql = "UPDATE TaskRepeat SET count = @count, description = @description, completedate = @completedate, done = @done WHERE trid = @trid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {


                    updateCommand.Parameters.AddWithValue("@trid", id);

                    if(taskr.count != tempTaskRepeat.count)
                    {
                        updateCommand.Parameters.AddWithValue("@count", taskr.count);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@count", tempTaskRepeat.count);
                    }

                    if(tempTaskRepeat.description != taskr.description)
                    {
                        updateCommand.Parameters.AddWithValue("@description", taskr.description);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@description", tempTaskRepeat.description);
                    }

                    if (taskr.completedate != tempTaskRepeat.completedate)
                    {
                        updateCommand.Parameters.AddWithValue("@completedate", taskr.completedate);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@completedate", tempTaskRepeat.completedate);
                    }


                    updateCommand.Parameters.AddWithValue("@done", taskr.done);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetOneTaskRepeat(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM TaskRepeat WHERE trid = @trid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@trid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
