using System;
using System.Collections.Generic;
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
    public class PayStatisticsController : Controller
    {
        /* ASP.NET MVC uses the format below to determine what code to invoke */
        /* /[Controller]/[ActionName]/[Parameters] */
        /* https://localhost:44300/PayStatistics/Index */
        /* https://localhost:44300/PayStatistics/Welcome */
        // GET: PayStatistics
        public ActionResult Index()
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
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line })
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
                            Line = new PlotOptionsLine
                            {
                                DataLabels = new PlotOptionsLineDataLabels
                                {
                                    Enabled = true
                                },
                                EnableMouseTracking = false
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

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}