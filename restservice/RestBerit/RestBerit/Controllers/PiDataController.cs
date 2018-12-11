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
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

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

        // GET: api/PiData/5
        [HttpGet("{id}", Name = "GetPiData")]
        public PiData GetPieData(int id)
        {
            var result = new List<PiData>();

            string getSql = "SELECT * FROM PiData WHERE pid =" + id;

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

                                result.Add(pie);
                            }
                        }
                    }    
                }
            }
            return result.Single(x => x.pid.Equals(id));
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

        // PUT: api/PiData/5
        [HttpPut("{id}")]
        public PiData Put(int id, [FromBody] PiData pie)
        {
            PiData tempPie = GetPieData(id);
            string updateSql = "UPDATE PiData SET timestamp = @timestamp, temperatur = @temperatur Where pid = @pid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {


                    updateCommand.Parameters.AddWithValue("@pid", id);

                    if (tempPie.timestamp != pie.timestamp)
                    {
                        updateCommand.Parameters.AddWithValue("@timestamp", pie.timestamp);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@timestamp", tempPie.timestamp);
                    }

                    if (tempPie.temperatur != pie.temperatur)
                    {
                        updateCommand.Parameters.AddWithValue("@temperatur", pie.temperatur);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@temperatur", tempPie.temperatur);
                    }

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetPieData(id);
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
