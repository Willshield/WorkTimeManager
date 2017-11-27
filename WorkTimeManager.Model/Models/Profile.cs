﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string ConnectionKey { get; set; }
    }
}
