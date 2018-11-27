using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestBerit.Model
{
    public class BeritUsers
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


        public BeritUsers(int id, string userName, string password)
        {
            Id = id;
            UserName = userName;
            Password = password;
        }



    }
}
