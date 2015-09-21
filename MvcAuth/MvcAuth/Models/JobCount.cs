using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAuth.Models
{
    public class JobCount
    {
        public int ID { get; set; }
        /* JobID is a foreign key */
        public int JobID { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }

        public virtual Job Job { get; set; }
    }
}