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
    public class AlarmsoundController : ControllerBase
    {
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        // GET: api/Alarmsound
        [HttpGet]
        public List<Alarmsounds> Get()
        {
            var result = new List<Alarmsounds>();

            string getSql = "SELECT * FROM Alarmsounds";

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
                                int asid = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                string soundfile = reader.GetString(2);

                                var bAlarmsounds = new Alarmsounds(asid,title, soundfile);

                                result.Add(bAlarmsounds);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // GET: api/Alarmsound/id
        [HttpGet("{id}", Name = "Get")]
        public Alarmsounds GetSpecific(int id)
        {
            var result = new List<Alarmsounds>();

            string getSql = "SELECT * FROM Alarmsounds WHERE asid =" + id;

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
                                int asid = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                string soundfile = reader.GetString(2);

                                var bAlarmsounds = new Alarmsounds(asid, title, soundfile);

                                result.Add(bAlarmsounds);
                            }
                        }
                    }
                }
            }

            return result.Single(x => x.asid.Equals(id));
        }

        // POST: api/Alarmsound
        [HttpPost]
        public void Post([FromBody] Alarmsounds alarmsound)
        {
            string insertSql = "INSERT INTO Alarmsounds(asid, title, soundfile) values (@asid, @title, @soundfile)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@asid", alarmsound.asid);
                    insertCommand.Parameters.AddWithValue("@title", alarmsound.title);
                    insertCommand.Parameters.AddWithValue("@soundfile", alarmsound.soundfile);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // PUT: api/Alarmsound/id
        [HttpPut("{id}")]
        public Alarmsounds Put(int id, [FromBody] Alarmsounds alarmsound)
        {
            string updateSql = "UPDATE Alarmsounds SET (title, soundfile) values (@title, @soundfile) Where tid = @asid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    updateCommand.Parameters.AddWithValue("@asid", id);

                    if (alarmsound.title != null)
                    { updateCommand.Parameters.AddWithValue("@title", alarmsound.title); }

                    if (alarmsound.soundfile != null)
                    { updateCommand.Parameters.AddWithValue("@soundfile", alarmsound.soundfile); }

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
            string deleteSql = "DELETE FROM Alarmsounds WHERE asid = @asid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@asid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
