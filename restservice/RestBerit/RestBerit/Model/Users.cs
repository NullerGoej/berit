using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit.Model
{
    public class Users
    {
        public int uid { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public int asid { get; set; }

        public Users(int uid, string username, string password, string firstname, string lastname, int asid)
        {
            this.uid = uid;
            this.username = username;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
            this.asid = asid;
        }

        public Users()
        {
            
        }

    }
}
