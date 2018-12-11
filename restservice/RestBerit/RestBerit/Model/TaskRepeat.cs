using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit.Model
{
    public class TaskRepeat
    {
        public int trid { get; set; }

        public int uid { get; set; }

        public int count { get; set; }

        public string description { get; set; }

        public DateTime createdate { get; set; }

        public DateTime completedate { get; set; }

        public bool done { get; set; }

        public TaskRepeat(int trid, int uid, int count, string description, DateTime createdate, DateTime completedate, bool done)
        {
            this.trid = trid;
            this.uid = uid;
            this.count = count;
            this.description = description;
            this.createdate = createdate;
            this.completedate = completedate;
            this.done = done;
        }

        public TaskRepeat()
        {
            
        }

    }
}
