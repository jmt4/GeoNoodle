using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcAuth.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Attributes;
using DotNet.Highcharts.Enums;
using System.Drawing;

namespace MvcAuth.Controllers
{
    public class StatisticsController : Controller
    {
        private StatisticsDBContext db = new StatisticsDBContext();

        public ActionResult PayStats(string jobName, string categoryName)
        {
            List<String> statsPages = new List<String> { "TrendingJobs", "PayStats", "Location" };
            ViewBag.statsPage = statsPages;
            /* We use this list to create the jobs dropdown list */
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();

            Job job;
            List<Job> jobs = new List<Job>();
            var series = new List<Series>(); // data series for high chart

            if (Request.Form["job"] != null) // Check which button was pressed here
            {
                job = db.Jobs.FirstOrDefault(j => j.Name == jobName); // Check if Job is in database
                if (job == null)
                {
                    jobs.Add(db.Jobs.First());
                }
                else
                {
                    jobs.Add(job);
                }
            }
            else
            {
                Category ourCategory;
                if (Enum.TryParse(categoryName, out ourCategory)) // Check if Category is in database
                {
                    jobs = db.Jobs.Where(j => j.Category == ourCategory).ToList();
                }
                else
                {
                    jobs.Add(db.Jobs.First());
                }
            }
            foreach (Job j in jobs) // Add each job to data series
            {
                series.Add(new Series
                {
                    Name = j.Name,
                    Data = new Data(new object[] { j.Bottom10Pay, j.MedianPay, j.Top10Pay, })
                });
            } 
            
            Highcharts chart = new Highcharts("chart")
              .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
              .SetTitle(new Title { Text = "Salaries For Different Career Levels" })
                //.SetSubtitle(new Subtitle { Text = "Source: Wikipedia.org" })
              .SetXAxis(new XAxis
              {
                  Categories = new[] { "Bottom 10%", "Medium", "Top 10%" },
                  Title = new XAxisTitle { Text = string.Empty }
              })
              .SetYAxis(new YAxis
              {
                  Min = 0,
                  Title = new YAxisTitle
                  {
                      Text = "Salary",
                      Align = AxisTitleAligns.High
                  }
              })
              .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y ; }" })
              .SetPlotOptions(new PlotOptions
              {
                  Bar = new PlotOptionsBar
                  {
                      DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
                  }
              })
              .SetLegend(new Legend
              {
                  Layout = Layouts.Vertical,
                  Align = HorizontalAligns.Right,
                  VerticalAlign = VerticalAligns.Top,
                  X = -100,
                  Y = 100,
                  Floating = true,
                  BorderWidth = 1,
                  BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                  Shadow = true
              })
              .SetCredits(new Credits { Enabled = false })
              .SetSeries(series.ToArray());
            return View(chart);
        }

       
        public ActionResult TrendingJobs(string jobName, string categoryName)
        {
            List<String> statsPages = new List<String> { "TrendingJobs", "PayStats", "Location" };
            ViewBag.statsPage = statsPages;
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();
            Random rand = new Random();
            JobCount jobCount;
            int counter = 0;
            Job job,name;
            List<JobCount> g;           
            var series = new List<Series>();
            List<int> jobids = db.Jobs.Select(j => j.ID).ToList();
            Dictionary<string, double> unsortedList = new Dictionary<string, double>();
            int noOfJobs = db.JobCounts.Sum(p => p.Count);
            int b, d;
            for (int k = 0; k <= jobids.Count - 1; k++)
            {
                counter = 0;
                b = jobids.ElementAt(k);
                job = db.Jobs.SingleOrDefault(a => a.ID == b);
                name = db.Jobs.SingleOrDefault(a => a.ID == b);
                g = job.JobCounts.ToList();
                foreach (JobCount jbs in g)
                {
                    counter =counter +jbs.Count;
                }
                double percentage = ((double)counter/noOfJobs)*100;
                unsortedList.Add(name.Name, percentage);
            }
            if (string.IsNullOrEmpty(jobName) && string.IsNullOrEmpty(categoryName))
            {
                /* Render default graph ie the first job in database */
                //iterate through all db vals
               
                var sortedlist =( from pair in unsortedList
                           orderby pair.Value descending
                           select pair).Take(10);
                foreach(KeyValuePair<string,double>pair in sortedlist)
                {
                    series.Add(new Series
                    {
                        Name = pair.Key,
                        Data = new Data(new object[] { pair.Value })
                    });
                }
            }
            else if (!string.IsNullOrEmpty(jobName))
            {
                try
                {
                    /* User hit submit button next to job list or GET with jobName query string */
                    /* try-catch here in case query string is not a job in our database */
                    job = db.Jobs.SingleOrDefault(j => j.Name == jobName);
                    jobCount = db.JobCounts.First();
                    Dictionary<string, int> compareList = new Dictionary<string, int>();
                    var mostPopularJob = (from pair in unsortedList
                                      orderby pair.Value descending
                                      select pair).First();

                    var leastPopularJob = (from pair in unsortedList
                                      orderby pair.Value descending
                                      select pair).Last();
                    var thisJob = (from pair in unsortedList
                                   where pair.Key == jobName
                                   select pair).First();
                    
                    series.Add(new Series
                    {
                        Name = "Least Popular Job "+leastPopularJob.Key,
                        Data = new Data(new object[] { leastPopularJob.Value })
                    });
                    series.Add(new Series
                    {
                        Name = thisJob.Key,
                        Data = new Data(new object[] { thisJob.Value})
                    });
                    series.Add(new Series
                    {
                        Name = "Most Popular Job "+mostPopularJob.Key,
                        Data = new Data(new object[] { mostPopularJob.Value })
                    });

                }
                catch (System.ArgumentNullException)
                {
                    Console.WriteLine("Job does not exist.");
                    /* Add default data */
                    job = db.Jobs.SingleOrDefault(j => j.Name == jobName);
                    jobCount = db.JobCounts.First();

                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { db.Jobs.Select(a => a.Name).ToArray() })
                    });
                }
            }
            else
            {
                try
                {
                    Category ourCategory = (Category)Enum.Parse(typeof(Category), categoryName);
                    foreach (Job j in db.Jobs)
                    {
                        if (j.Category == ourCategory)
                        {
                            series.Add(new Series
                            {
                                Name = j.Name,
                                Data = new Data(new object[] { j.Name })
                            });
                        }
                    }
                }
                catch (System.ArgumentNullException)
                {
                    Console.WriteLine("Category does not exist.");
                    job = db.Jobs.SingleOrDefault(j => j.Name == jobName);
                    jobCount = db.JobCounts.First();

                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { db.Jobs.Select(a => a.Name).ToArray() })
                    });
                }
            }          

            /* create Highchart type */
            var chart = new Highcharts("chart")
                /* Define chart type -- specify pie, heat, etc here */
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                /* Main title of chart */
                        .SetTitle(new Title { Text = "Trending Jobs" })
                /* Small title below main title */
                        .SetSubtitle(new Subtitle { Text = "Statistics" })
                /* Load x values */
                        .SetXAxis(new XAxis
                        {
                            Categories = new[] { "Popular Jobs" },
                            Title = new XAxisTitle { Text = string.Empty }
                        })
                /* Title of Y axis */
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Percantage of Jobs (%)" } })
                        .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y ; }" })
              .SetPlotOptions(new PlotOptions
              {
                  Bar = new PlotOptionsBar
                  {
                      DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
                  }
              })
              .SetLegend(new Legend
              {
                  Layout = Layouts.Vertical,
                  Align = HorizontalAligns.Right,
                  VerticalAlign = VerticalAligns.Top,
                  X = -100,
                  Y = 100,
                  Floating = true,
                  BorderWidth = 1,
                  BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                  Shadow = true
              })
              .SetCredits(new Credits { Enabled = false })
                .SetSeries(series.ToArray());

            return View(chart);

        }

      /*  [HttpPost]
        [ActionName("TrendingJobs")]
        [ValidateAntiForgeryToken]
        public ActionResult TrendingJobsPost(string jobName, string categoryName) 
        {
              if (Request.Form["category"] != null)
            {
                return RedirectToAction("TrendingJobs", new { CategoryName = categoryName });
            }
            else
            {
                return RedirectToAction("TrendingJobs", new { jobName = jobName });
            }
        }*/

        public ActionResult Location(string jobName, string categoryName)
        {
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();
            
            Job job;
            List<Job> jobs = new List<Job>();
            
            //Based on user query, build list of jobs to consider
            if (Request.Form["job"] != null) // Check which button was pressed here
            {
                job = db.Jobs.FirstOrDefault(j => j.Name == jobName); // Check if Job is in database
                if (job == null)
                {
                    jobs.Add(db.Jobs.First());
                }
                else
                {
                    jobs.Add(job);
                }
            }
            else
            {
                Category ourCategory;
                if (Enum.TryParse(categoryName, out ourCategory)) // Check if Category is in database
                {
                    jobs = db.Jobs.Where(j => j.Category == ourCategory).ToList();
                }
                else
                {
                    jobs.Add(db.Jobs.First());
                }
            }

            List<string> counties = new List<string> { 
                "Maricopa", "Coconino", "Gila", "Pima", "Pinal", "Yavapai", "Mohave", "Cochise", "Navajo", "Graham", "La Paz", "Apache", "Yuma", "Santa Cruz", "Greenlee" 
            };

            List<Density> densities = db.Densities.ToList();
            Dictionary<string, int> jobAmounts = new Dictionary<string, int>();

            //Iterate through each county and job selected to find the amount of jobs in that county
            foreach (string c in counties)
            {
                int count = 0;

                foreach (Job j in jobs)
                {
                    County co;
                    Enum.TryParse<County>(c, out co);
                    Density dense = db.Densities.FirstOrDefault(x => x.JobID == j.ID && x.County == co);

                    if (dense != null)
                        count += dense.Value;                
                }

                jobAmounts.Add(c, count);
            }

            ViewBag.locationData = jobAmounts;
            return View();
        }

        /*
        [HttpPost]
        [ActionName("Location")]
        [ValidateAntiForgeryToken]
        public ActionResult LocationPost(string jobName, string categoryName)
        {
            if (Request.Form["category"] != null)
            {
                return RedirectToAction("Location", new { CategoryName = categoryName });
            }
            else
            {
                return RedirectToAction("Location", new { JobName = jobName });
            }
        }*/


        /******************************************************************************/
        /*     Methods below are auto-generated by VS and more or less pointless      */
        /******************************************************************************/


        // GET: Statistics
        public ActionResult Index()
        {
            return View(db.Jobs.ToList());
        }

        // GET: Statistics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Statistics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Category,Top10Pay,MedianPay,Bottom10Pay")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Statistics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Statistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Category,Top10Pay,MedianPay,Bottom10Pay")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Statistics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Statistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}