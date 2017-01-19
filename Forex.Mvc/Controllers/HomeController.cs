using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Forex.Core.MathLib;
using Forex.Experts;
using Forex.Mvc.Models;

namespace Forex.Mvc.Controllers {
    public class HomeController : Controller {
        private readonly int _pageSize = 500;
        public ActionResult Index(int pageNumber = 0) {
            var tickData = GetTickData(pageNumber);
            var regData = GetRegressionData(tickData);

            var chartViewModel = new ChartViewModel {
                ActualData = tickData,
                RegressionData = regData
            };

            return View(chartViewModel);
        }

        private List<TickData> GetRegressionData(List<TickData> tickData) {
            var regData = new List<TickData>();

            for (var i = 0; i < tickData.Count; i++) {
                var regression = new LinearRegression(tickData.Where(p => p.Time < i).Select(p => p.Price).ToList(),
                    tickData.Where(p => p.Time < i).Select(p => p.Price).ToList());
                var predictedValue = regression.GetPredictedYValue(i + 1);
                if (double.IsNaN(predictedValue)) {
                    predictedValue = 0;
                }

                regData.Add(new TickData {Time = i + 1, Price = predictedValue});
            }

            return regData;
        }

        private List<TickData> GetTickData(int pageNumber) {

            var tickDatas = new List<TickData>();

            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Data/Crude.txt"));
            lines = lines.Skip(pageNumber*_pageSize).Take(_pageSize).ToArray();

            var lineNumber = 1;
            foreach (var line in lines) {
                var lineData = line.Split(',');
                double ask;
                //double bid;
                double.TryParse(lineData[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ask);
                //double.TryParse(lineData[2], NumberStyles.Any, CultureInfo.InvariantCulture, out bid);

                var tickData = new TickData {
                    Time = lineNumber,
                    Price = ask
                };

                tickDatas.Add(tickData);
                lineNumber++;
            }

            return tickDatas;
        }
    }
}