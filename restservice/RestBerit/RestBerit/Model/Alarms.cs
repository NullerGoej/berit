using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace RestBerit.Model
{
    public class Alarms
    {
        public int aid { get; set; }

        public int uid { get; set; }

        public DateTime timestamp { get; set; }

        public int asid { get; set; }

        public Alarms(int aid, int uid, DateTime timestamp, int asid)
        {
            this.aid = aid;
            this.uid = uid;
            this.timestamp = timestamp;
            this.asid = asid;
        }

        public Alarms()
        {
            
        }
    }
}
