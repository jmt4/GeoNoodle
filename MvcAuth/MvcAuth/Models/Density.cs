using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAuth.Models
{
    public enum County
    {
        Maricopa, Coconino, Gila, Pima, Pinal, Yavapai, Mohave, Cochise, Najavo, Graham, LaPaz, Apache, Yuma, SantaCruz, Greenlee
    }
    public class Density
    {
        public int ID { get; set; }
        /* JobID is a foreign key */
        public int JobID { get; set; }
        public County County { get; set; }
        public double Value { get; set; }

        public virtual Job Job { get; set; }
    }
}