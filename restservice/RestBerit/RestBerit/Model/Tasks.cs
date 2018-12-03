using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit
{
    public class Tasks
    {
        public int tid { get; set; }

        public int uid { get; set; }

        public DateTime timestamp { get; set; }

        public DateTime endstamp { get; set; }

        public string description { get; set; }

        public bool done { get; set; }

        public bool repeat { get; set; }

        public Tasks(int tid, int uid, DateTime timestamp, DateTime endstamp, string description, bool done, bool repeat)
        {
            this.tid = tid;
            this.uid = uid;
            this.timestamp = timestamp;
            this.endstamp = endstamp;
            this.description = description;
            this.done = done;
            this.repeat = repeat;
        }

        public Tasks()
        {

        }

    }
}
