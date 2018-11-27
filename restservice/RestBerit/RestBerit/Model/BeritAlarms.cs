using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit.Model
{
    public class BeritAlarms
    {
        public int Id { get; set; }

        public DateTime Alarm { get; set; }

        public BeritAlarms(int id, DateTime alarm)
        {
            Id = id;
            Alarm = alarm;
        }
    }
}
