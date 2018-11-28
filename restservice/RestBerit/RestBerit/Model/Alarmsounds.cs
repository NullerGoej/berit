using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RestBerit.Model
{
    public class Alarmsounds
    {
        public int asid { get; set; }

        public string title { get; set; }

        public string soundfile { get; set; }

        public Alarmsounds(int asid, string title, string soundfile)
        {
            this.asid = asid;
            this.title = title;
            this.soundfile = soundfile;
        }

        public Alarmsounds()
        {
            
        }

    }
}
