using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit
{
    public class BeritTasks
    {
        public int Id { get; set; }

        public string Task { get; set; }

        public BeritTasks(int id, string task)
        {
            Id = id;
            Task = task;
        }

        public BeritTasks()
        {

        }

    }
}
