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
        [HttpGet]
        public ActionResult PayStats()
        {
            var default_job = db.Jobs.First();
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
                    new Series { Name = default_job.Name, Data = new Data(new object[] { default_job.Bottom10Pay, default_job.MedianPay, default_job.Top10Pay,  }) },
                    //new Series { Name = "Software Engineer", Data = new Data(new object[] { 39000, 89000, 123000 }) },
                    //new Series { Name = "Botanist", Data = new Data(new object[] { 33000, 56000, 79000 }) }
                });

          return View(chart);
        }

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
