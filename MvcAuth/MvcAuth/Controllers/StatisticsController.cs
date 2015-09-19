using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAuth.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Attributes;
using DotNet.Highcharts.Enums;




namespace MvcAuth.Controllers
{
    public class StatisticsController : Controller
    {
        /* ASP.NET MVC uses the format below to determine what code to invoke */
        /* /[Controller]/[ActionName]/[Parameters] */
        /* https://localhost:44300/PayStatistics/Index */
        /* https://localhost:44300/PayStatistics/Welcome */
        // GET: PayStatistics

        public ActionResult Index()
        {
            List<string> jobs = new List<string> { "Computer Science", "Chemistry", "Construction"};
            ViewBag.JobList = jobs;

            return View();
        }

        public ActionResult PayStats()
        {
            

            var wages = new List<Wages> {
                new Wages(){    date = new DateTime(2013, 1, 10), wage = 400},
                new Wages(){    date = new DateTime(2013, 2, 10), wage = 412},
                new Wages(){    date = new DateTime(2013, 3, 10), wage = 425},
                new Wages(){    date = new DateTime(2013, 4, 10), wage = 445},
                new Wages(){    date = new DateTime(2013, 5, 10), wage = 475},
                new Wages(){    date = new DateTime(2013, 6, 10), wage = 490},
                new Wages(){    date = new DateTime(2013, 7, 10), wage = 500},
                new Wages(){    date = new DateTime(2013, 8, 10), wage = 560},
                new Wages(){    date = new DateTime(2013, 9, 10), wage = 570},
                new Wages(){    date = new DateTime(2013, 10, 10), wage = 575},
                new Wages(){    date = new DateTime(2013, 11, 10), wage = 600},
                new Wages(){    date = new DateTime(2013, 12, 10), wage = 606}
            };

            /* Highcharts only accepts object arrays. Thus strings and DateTime don't need casting */
            var xDateTimes = wages.Select(i => i.date.ToString()).ToArray();
            var yWages = wages.Select(i => new object[] { i.wage }).ToArray();

            /* create Highchart type */
            var chart = new Highcharts("chart")
                /* Define chart type -- specify pie, heat, etc here */
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Area })
                /* Main title of chart */
                        .SetTitle(new Title { Text = "Pay over time" })
                /* Small title below main title */
                        .SetSubtitle(new Subtitle { Text = "Statistics" })
                /* Load x values */
                        .SetXAxis(new XAxis { Categories = xDateTimes })
                /* Title of Y axis */
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Wages (USD)" } })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name + '</b><br/>'+ this.x +': '+ this.y; }"
                        })
                        .SetPlotOptions(new PlotOptions
                        {
                            /*
                            Line = new PlotOptionsLine
                            {
                                DataLabels = new PlotOptionsLineDataLabels
                                {
                                    Enabled = true
                                },
                                EnableMouseTracking = false
                            },
                            */

                            Area = new PlotOptionsArea
                            {
                                FillColor = new BackColorOrGradient(new Gradient
                                {
                                    LinearGradient = new[] { 0, 0, 0, 300},
                                    Stops = new object[,] { { 0, "rgb(116, 116, 116)" }, { 1, Color.Gold } }
                                }),
                                LineWidth = 1,
                                LineColor = Color.BlanchedAlmond,
                            }
                        })
                /* Load Y values */
                        .SetSeries(new[] 
                        {
                            new Series { Name = "Hour", Data = new Data(yWages) },
                            /* add more y data to create a second line */
                            /* new Series { Name = "Other Name", Data = new Data(OtherData) } */
                        });
            
            return View(chart);
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

        public ActionResult Location()
        {
       
          return View();
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}