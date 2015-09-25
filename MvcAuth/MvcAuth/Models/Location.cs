using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAuth.Models
{
    public class Location
    {
        public int ID { get; set; }
        public int JobID { get; set; }
        public string name { get; set; }
        public int jobAmount { get; set; }
    }
}