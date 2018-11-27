using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestBerit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeritTaskController : ControllerBase
    {
        private const string connection = "Server=tcp:berit.database.windows.net,1433;Initial Catalog=BeritDB;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // GET: api/Berit
        [HttpGet]
        public List<BeritTasks> Get()
        {
            var result = new List<BeritTasks>();

            string getSql = "SELECT id, task FROM BeritTasks";

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
                                int idr = reader.GetInt32(0);
                                string taskr = reader.GetString(1);

                                var bTasks = new BeritTasks(idr, taskr);

                                result.Add(bTasks);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // GET: api/Berit/5
        [HttpGet("{id}", Name = "Get")]
        public BeritTasks GetSpecific(int id)
        {
            var result = new List<BeritTasks>();

            string getSql = "SELECT id, task FROM BeritTasks Where BeritTasks.id =" + id;

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand getSpecificCommand = new SqlCommand(getSql, dbConnection))
                {
                    using (SqlDataReader reader = getSpecificCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                int idr = reader.GetInt32(0);
                                string taskr = reader.GetString(1);

                                var bTasks = new BeritTasks(idr, taskr);

                                result.Add(bTasks);
                            }
                        }
                    }
                }
            }

            return result.Single(task => task.Id.Equals(id));
        }

        // POST: api/Berit
        [HttpPost]
        public BeritTasks Post([FromBody] BeritTasks bT)
        {
            string insertSql = "INSERT INTO BeritTasks(id,task) values (@id, @task)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@task", bT.Task);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return bT;
        }

        // PUT: api/Berit/5
        [HttpPut("{id}")]
        public BeritTasks Put(int id, [FromBody] BeritTasks bT)
        {
            BeritTasks BT = GetSpecific(id);
            BT.Task = bT.Task;

            return GetSpecific(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM BeritTasks WHERE id = @id";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@id", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
