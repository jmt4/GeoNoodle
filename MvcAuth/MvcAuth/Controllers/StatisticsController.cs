﻿using System;
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

        [HttpGet]
        public ActionResult PayStats(string jobName, string categoryName)
        {
            List<String> statsPages = new List<String> { "TrendingJobs", "PayStats", "Location" };
            ViewBag.statsPage = statsPages;
            /* We use this list to create the jobs dropdown list */
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();

            Job job;
            var series = new List<Series>();
            if (string.IsNullOrEmpty(jobName) && string.IsNullOrEmpty(categoryName))
            {
                /* Render default graph ie the first job in database */
                job = db.Jobs.First();
                series.Add(new Series
                {
                    Name = job.Name,
                    Data = new Data(new object[] { job.Bottom10Pay, job.MedianPay, job.Top10Pay, })
                });
            }
            else if (!string.IsNullOrEmpty(jobName))
            {
                try
                {
                    /* User hit submit button next to job list or GET with jobName query string */
                    /* try-catch here in case query string is not a job in our database */
                    job = db.Jobs.SingleOrDefault(j => j.Name == jobName);
                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { job.Bottom10Pay, job.MedianPay, job.Top10Pay, })
                    });
                }
                catch (System.ArgumentNullException)
                {
                    Console.WriteLine("Job does not exist.");
                    /* Add default data */
                    job = db.Jobs.First();
                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { job.Bottom10Pay, job.MedianPay, job.Top10Pay, })
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
                                Data = new Data(new object[] { j.Bottom10Pay, j.MedianPay, j.Top10Pay, })
                            });
                        }
                    }
                }
                catch (System.ArgumentNullException)
                {
                    Console.WriteLine("Category does not exist.");
                    job = db.Jobs.First();
                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { job.Bottom10Pay, job.MedianPay, job.Top10Pay, })
                    });
                }
            }

            Highcharts chart = new Highcharts("chart")
              .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
              .SetTitle(new Title { Text = "Historic World Population by Region" })
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

        [HttpPost]
        [ActionName("PayStats")]
        [ValidateAntiForgeryToken]
        public ActionResult PayStatsPost(string jobName, string categoryName)
        {
            /* Statistics/_PartialForm.cshtml posts to 'PayStats' hence the [ActionName] decorator. */
            /* VS complains if controller method headers (name, params) are identical. */
            if (Request.Form["category"] != null)
            {
                return RedirectToAction("PayStats", new { CategoryName = categoryName });
            }
            else
            {
                return RedirectToAction("PayStats", new { jobName = jobName });
            }
        }

        public ActionResult CompareSalaries()
        {
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                .SetTitle(new Title { Text = "Historic World Population by Region" })
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
                .SetSeries(new[]
                {
                    new Series { Name = "Physician", Data = new Data(new object[] { 80000, 125000, 215000,  }) },
                    new Series { Name = "Software Engineer", Data = new Data(new object[] { 39000, 89000, 123000 }) },
                    new Series { Name = "Botanist", Data = new Data(new object[] { 33000, 56000, 79000 }) }
                });

            return View(chart);
        }

        [HttpGet]
        public ActionResult TrendingJobs(string jobName, string categoryName)
        {
            List<String> statsPages = new List<String> { "TrendingJobs", "PayStats", "Location" };
            ViewBag.statsPage = statsPages;
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();
            Random rand = new Random();
            JobCount jobCount,count;
            Job job,name;
            var series = new List<Series>();
            if (string.IsNullOrEmpty(jobName) && string.IsNullOrEmpty(categoryName))
            {
                /* Render default graph ie the first job in database */
                //iterate through all db vals
                List<int> jobids = db.Jobs.Select(j => j.ID).ToList();
                int b;
                for(int k=0; k<=6; k++)
                {
                    b = jobids.ElementAt(k);
                    count = db.JobCounts.SingleOrDefault(a => a.JobID == b);
                    name = db.Jobs.SingleOrDefault(j => j.ID == b);
                    series.Add(new Series
                    {
                        Name = name.Name, 
                        Data = new Data(new object[] { count.Count })
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

                    series.Add(new Series
                    {
                        Name = job.Name,
                        Data = new Data(new object[] { db.Jobs.Select(a => a.Name).ToArray() })
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
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "RateOfGrowth (%)" } })
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

        [HttpPost]
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
        }
        

        public ActionResult Location()
        {
            List<String> statsPages = new List<String> { "TrendingJobs", "PayStats", "Location" };
            ViewBag.statsPage = statsPages;
            ViewBag.JobList = db.Jobs.Select(j => j.Name).ToList();
            return View();
        }

        [HttpPost]
        [ActionName("Location")]
        [ValidateAntiForgeryToken]
        public ActionResult LocationPost(string jobName, string categoryName)
        {
            return RedirectToAction("Location");
        }


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