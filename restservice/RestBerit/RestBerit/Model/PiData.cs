using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit.Model
{
    public class PiData
    {
        public int pid { get; set; }

        public DateTime timestamp { get; set; }

        public int temperatur { get; set; }

        public PiData(int pid, DateTime timestamp, int temperatur)
        {
            this.pid = pid;
            this.timestamp = timestamp;
            this.temperatur = temperatur;
        }

        public PiData()
        {
            
        }
    }
}
