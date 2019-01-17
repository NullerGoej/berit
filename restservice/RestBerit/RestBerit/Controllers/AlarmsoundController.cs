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
        private const string connection = "Server=tcp:berit2.database.windows.net,1433;Initial Catalog=Berit2;Persist Security Info=False;User ID=berit;Password=Venadux123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
                                int asid = 0; 
                                if(!reader.IsDBNull(0))
                                    asid = reader.GetInt32(0);

                                string title = "";
                                if(!reader.IsDBNull(1))
                                    title = reader.GetString(1);

                                string soundfile = "";
                                if(!reader.IsDBNull(2))
                                    soundfile = reader.GetString(2);

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
        [HttpGet("{id}", Name = "GetAlarmsound")]
        public Alarmsounds GetOneAlarmsound(int id)
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
                                int asid = 0;
                                if (!reader.IsDBNull(0))
                                    asid = reader.GetInt32(0);

                                string title = "";
                                if (!reader.IsDBNull(1))
                                    title = reader.GetString(1);

                                string soundfile = "";
                                if (!reader.IsDBNull(2))
                                    soundfile = reader.GetString(2);

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
            string insertSql = "INSERT INTO Alarmsounds(title, soundfile) values (@title, @soundfile)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
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
            Alarmsounds tempAkarAlarmsound = GetOneAlarmsound(id);
            string updateSql = "UPDATE Alarmsounds SET title = @title, soundfile = @soundfile WHERE asid = @asid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    updateCommand.Parameters.AddWithValue("@asid", id);

                    if (alarmsound.title != "")
                    {
                        updateCommand.Parameters.AddWithValue("@title", alarmsound.title);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@title", tempAkarAlarmsound.title);
                    }


                    if (alarmsound.soundfile != "")
                    {
                        updateCommand.Parameters.AddWithValue("@soundfile", alarmsound.soundfile);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@soundfile", tempAkarAlarmsound.soundfile);
                    }

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetOneAlarmsound(id);
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
