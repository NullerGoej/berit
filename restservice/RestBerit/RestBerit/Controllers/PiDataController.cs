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
    public class PiDataController : ControllerBase
    {
        //connection string
        private const string connection = "Server=tcp:berit2.database.windows.net,1433;Initial Catalog=Berit2;Persist Security Info=False;User ID=berit;Password=Venadux123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // GET: api/PiData
        [HttpGet]
        public List<PiData> Get()
        {
            var result = new List<PiData>();

            string getsql = "SELECT * FROM PiData";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand getSpecificCommand = new SqlCommand(getsql, dbConnection))
                {
                    using (SqlDataReader reader = getSpecificCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int pid = 0;
                                if (!reader.IsDBNull(0))
                                    pid = reader.GetInt32(0);

                                DateTime timestamp = DateTime.MinValue;
                                if (!reader.IsDBNull(1))
                                    timestamp = reader.GetDateTime(1);

                                int temperatur = 0;
                                if (!reader.IsDBNull(2))
                                    temperatur = reader.GetInt32(2);

                                var pie = new PiData(pid, timestamp, temperatur);

                                result.Add(pie);
                            }
                        }
                    }
                }
            }

                return result;
        }

        // GET: api/?????/PiData
        [Route("[action]")]
        [HttpGet]
        public PiData GetNewest()
        {
            string getSql = "SELECT TOP 1 * FROM PiData ORDER BY pid DESC";

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
                                int pid = 0;
                                if (!reader.IsDBNull(0))
                                    pid = reader.GetInt32(0);

                                DateTime timestamp = DateTime.MinValue;
                                if (!reader.IsDBNull(1))
                                    timestamp = reader.GetDateTime(1);

                                int temperatur = 0;
                                if (!reader.IsDBNull(2))
                                    temperatur = reader.GetInt32(2);

                                var pie = new PiData(pid, timestamp, temperatur);

                                return pie;
                            }
                        }
                    }
                }
            }

            return new PiData();
        }

        // POST: api/PiData
        [HttpPost]
        public void Post([FromBody] PiData pie)
        {
            string insertSql = "INSERT INTO PiData( timestamp, temperatur) values (@timestamp, @temperatur)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@temperatur", pie.temperatur);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM PiData WHERE pid = @pid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@pid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
