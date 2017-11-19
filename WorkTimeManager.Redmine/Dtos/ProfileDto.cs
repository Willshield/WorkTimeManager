using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Model.Models;

namespace WorkTimeManager.Redmine.Dtos
{
    public class ProfileDto
    {
        public User user { get; set; }
        public class User
        {
            public int id { get; set; }
            public string login { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string mail { get; set; }
            public DateTime created_on { get; set; }
            public DateTime last_login_on { get; set; }
            public string api_key { get; set; }
            public int status { get; set; }
        }

        public Profile ToEntity()
        {
            var tmp = new Profile();
            tmp.ConnectionKey = user.api_key;
            tmp.Email = user.mail;
            tmp.ID = user.id;
            tmp.Name = string.Join(" ", user.firstname, user.lastname);
            tmp.UserName = user.login;
            return tmp;
        }
    }
}
