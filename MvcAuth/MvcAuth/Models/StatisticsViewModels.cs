using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MvcAuth.Models
{
    public enum Category
    {
        Science, Technology, Trade, Health
    }
    public class Job
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public int Top10Pay { get; set; }
        public int MedianPay { get; set; }
        public int Bottom10Pay { get; set; }
        /* Added for job Density stats page */
        /* Create Density models -- County Value Job */
        public virtual ICollection<Density> Densities { get; set; }
        /* Added for trending jobs */
        /* Create JobCount model -- Year Count Job */
        public virtual ICollection<JobCount> JobCounts { get; set; }
    }

    public class StatisticsDBContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Density> Densities { get; set; }
        public DbSet<JobCount> JobCounts { get; set; }
    }
}