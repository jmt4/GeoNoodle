namespace MvcAuth.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using MvcAuth.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcAuth.Models.StatisticsDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MvcAuth.Models.StatisticsDBContext";
        }

        protected override void Seed(MvcAuth.Models.StatisticsDBContext context)
        {
            var jobs = new List<Job>
            {
                new Job {Name="Biologist", Category=Category.Science, Top10Pay=90000, MedianPay=71490, Bottom10Pay=64300},
                new Job {Name="Physicist", Category=Category.Science, Top10Pay=140000, MedianPay=109600, Bottom10Pay=78000},
                new Job {Name="Chemist", Category=Category.Science, Top10Pay=98000, MedianPay=73480, Bottom10Pay=66000},
                new Job {Name="Software Developer", Category=Category.Technology, Top10Pay=150000, MedianPay=102880, Bottom10Pay=60000},
                new Job {Name="Bioinformatics Scientist", Category=Category.Technology, Top10Pay=100000, MedianPay=74720, Bottom10Pay=55000},
                new Job {Name="Computer Programmer", Category=Category.Technology, Top10Pay=105000, MedianPay=77550, Bottom10Pay=62000},
                new Job {Name="Nurse", Category=Category.Trade, Top10Pay=160000, MedianPay=66400, Bottom10Pay=50000},
                new Job {Name="Plumber", Category=Category.Trade, Top10Pay=90000, MedianPay=50660, Bottom10Pay=34000},
                new Job {Name="Electrician", Category=Category.Trade, Top10Pay=90000, MedianPay=51110, Bottom10Pay=45000},
                new Job {Name="Neurologist", Category=Category.Health, Top10Pay=250000, MedianPay=180000, Bottom10Pay=120000},
                new Job {Name="Surgeon", Category=Category.Health, Top10Pay=250000, MedianPay=180000, Bottom10Pay=120000},
                new Job {Name="Physical Therapist", Category=Category.Health, Top10Pay=133000, MedianPay=82390, Bottom10Pay=60000},
            };

            /* Call Add() on DbSet */
            jobs.ForEach(j => context.Jobs.Add(j));
            context.SaveChanges();
            /* Create a list of counties from our enum in Density.cs */
            County[] counties = (County[])Enum.GetValues(typeof(County));
            var years = Enumerable.Range(2000, 15);
            int[] jobDensities;
            int[] jobCounts;
            /* For each job in jobs list, create a new array of random doubles */
            /* then iterate through counties and jobDensities simultaneously */
            /* with Zip() and create a new List<Density> list for EACH job. */
            /* Zip() stops when one iterator is NULL. In this case, both  */
            /* iterators are the same length. */
            foreach (Job job in jobs)
            {
                var rand = new Random();
                /* Create sequence of zeros then select each and turn into randoms */
                jobDensities = Enumerable.Repeat(0, 14).Select(i => rand.Next(100,1000)).ToArray();
                jobCounts = Enumerable.Repeat(0, years.Count()).Select(i => rand.Next(500, 5000)).ToArray();

                var densities = jobDensities.Zip(counties, (d, c) => new  Density { County = c, Value = d, Job = job, JobID = job.ID }).ToList();
                var counts = jobCounts.Zip(years, (c, y) => new JobCount { Year=y, Count=c, Job = job, JobID=job.ID}).ToList();

                densities.ForEach(d => context.Densities.Add(d));
                counts.ForEach(c => context.JobCounts.Add(c));
                context.SaveChanges();
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
