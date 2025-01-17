﻿using System;
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
        //connection  string
        private const string connection = "Server=tcp:berit2.database.windows.net,1433;Initial Catalog=Berit2;Persist Security Info=False;User ID=berit;Password=Venadux123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // GET: api/Alarm
        [HttpGet]
        public List<Alarms> Get()
        {
            var result = new List<Alarms>();

            //SQL command
            string getSql = "SELECT * FROM Alarms";

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
                                int aid = 0;
                                if(!reader.IsDBNull(0))
                                    aid = reader.GetInt32(0);

                                int uid = 0;
                                if(!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                DateTime timestamp = DateTime.MinValue;
                                if (!reader.IsDBNull(2))
                                    timestamp = reader.GetDateTime(2);

                                int asid = 0;
                                if(!reader.IsDBNull(3))
                                    asid = reader.GetInt32(3);

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
                                int aid = 0;
                                if (!reader.IsDBNull(0))
                                    aid = reader.GetInt32(0);

                                int uid = 0;
                                if (!reader.IsDBNull(1))
                                    uid = reader.GetInt32(1);

                                DateTime timestamp = DateTime.MinValue;
                                if (!reader.IsDBNull(2))
                                    timestamp = reader.GetDateTime(2);

                                int asid = 0;
                                if (!reader.IsDBNull(3))
                                    asid = reader.GetInt32(3); ;


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
            string insertSql = "INSERT INTO Alarms(uid, timestamp, asid) values (@uid, @timestamp, @asid)";

            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertSql, dbConnection))
                {
                    insertCommand.Parameters.AddWithValue("@uid", alarm.uid);
                    insertCommand.Parameters.AddWithValue("@timestamp", DateTime.Now);
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
            Alarms tempAlarm = GetOneAlarm(id);
            string updateSql;

            updateSql = "UPDATE Alarms SET uid = @uid, timestamp = @timestamp, asid = @asid WHERE aid = @aid";
            


            using (SqlConnection dbConnection = new SqlConnection(connection))
            {
                dbConnection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateSql, dbConnection))
                {


                    updateCommand.Parameters.AddWithValue("@aid", id);

                    if (tempAlarm.uid != alarm.uid)
                    {
                        updateCommand.Parameters.AddWithValue("@uid", alarm.uid);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@uid", tempAlarm.uid);
                    }


                    if (tempAlarm.timestamp != alarm.timestamp)
                    {
                        updateCommand.Parameters.AddWithValue("@timestamp", alarm.timestamp);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@timestamp", tempAlarm.timestamp);
                    }

                    if (tempAlarm.asid != alarm.asid)
                    {
                        updateCommand.Parameters.AddWithValue("@asid", alarm.asid);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@asid", tempAlarm.asid);
                    }


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
