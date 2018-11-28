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
    public class AlarmController : ControllerBase
    {
        private const string connection = "Server = tcp:berit.database.windows.net,1433;Initial Catalog = BeritDB; Persist Security Info=False;User ID = berit; Password=WT8mpDNwljGn;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        // GET: api/Alarm
        [HttpGet]
        public List<Alarms> Get()
        {
            var result = new List<Alarms>();

            string getSql = "SELECT * FROM Alarms";

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
                                int aid = reader.GetInt32(0);
                                int uid = reader.GetInt32(1);
                                DateTime timestamp = reader.GetDateTime(2);
                                int asid = reader.GetInt32(4);

                                var bAlarms = new Alarms(aid, uid, timestamp, asid);

                                result.Add(bAlarms);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // GET: api/Alarm/id
        [HttpGet("{id}", Name = "GetAlarm")]
        public Alarms GetOneAlarm(int id)
        {
            var result = new List<Alarms>();

            string getSql = "SELECT * FROM Alarms WHERE aid =" + id;

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
                                int aid = reader.GetInt32(0);
                                int uid = reader.GetInt32(1);
                                DateTime timestamp = reader.GetDateTime(2);
                                int asid = reader.GetInt32(4);

                                var bAlarms = new Alarms(aid, uid, timestamp, asid);

                                result.Add(bAlarms);
                            }
                        }
                    }
                }
            }

            return result.Single(x => x.aid.Equals(id));
        }

        // POST: api/Alarm
        [HttpPost]
        public void Post([FromBody] Alarms alarm)
        {
            string insertSql = "INSERT INTO Alarms(aid, uid, timestamp, asid) values (@aid, @uid @timestamp, @asid)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@aid", alarm.aid);
                    insertCommand.Parameters.AddWithValue("@uid", alarm.uid);
                    insertCommand.Parameters.AddWithValue("@timestamp", alarm.timestamp);
                    insertCommand.Parameters.AddWithValue("@asid", alarm.asid);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }

        // PUT: api/Alarm/id
        [HttpPut("{id}")]
        public Alarms Put(int id, [FromBody] Alarms alarm)
        {
            string updateSql = "UPDATE Alarms SET (uid, timestamp, asid) values (@uid, @timestamp, @asid) Where tid = @aid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {
                    updateCommand.Parameters.AddWithValue("@aid", id);

                    if (alarm.aid != null)
                    { updateCommand.Parameters.AddWithValue("@uid", alarm.uid); }

                    if (alarm.uid != null)
                    { updateCommand.Parameters.AddWithValue("@timestamp", alarm.timestamp); }

                    if (alarm.asid != null)
                    { updateCommand.Parameters.AddWithValue("@asid", alarm.asid); }

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
            return GetOneAlarm(id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deleteSql = "DELETE FROM Alarms WHERE aid = @aid";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand DeleteCommand = new SqlCommand(deleteSql, dbConnection))
                {
                    DeleteCommand.Parameters.AddWithValue("@aid", id);
                    int rowsAffected = DeleteCommand.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) affected");
                }
            }
        }
    }
}
