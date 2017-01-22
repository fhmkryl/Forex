using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Forex.Core.Entity;
using Forex.Core.MathLib;
using Forex.Mvc.Models;

namespace Forex.Mvc.Controllers {
    public class HomeController : Controller {
        private readonly int _pageSize = 400;
        public ActionResult Index(int pageNumber = 0) {
            var tickData = new List<TickData>();
            var regData = new List<TickData>();
            var regLineData = new List<TickData>();

            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Data/Crude.txt"));
            lines = lines.Where(p => !string.IsNullOrWhiteSpace(p)).Skip(pageNumber * _pageSize).Take(_pageSize).ToArray();

            foreach (var line in lines) {
                if (!string.IsNullOrWhiteSpace(line)) {
                    var data = line.Split(',');
                    double index;
                    double.TryParse(data[0], NumberStyles.Any, CultureInfo.InvariantCulture, out index);

                    double tick;
                    double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out tick);

                    double reg;
                    double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out reg);

                    double regLine;
                    double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out regLine);

                    tickData.Add(new TickData() {Index = index, Price = tick});
                    regData.Add(new TickData() {Index = index, Price = reg});
                    regLineData.Add(new TickData() {Index = index, Price = regLine});

                    index++;
                }
            }

            var chartViewModel = new ChartViewModel {
                ActualData = tickData,
                RegressionData = regData,
                RegressionLineData = regLineData
            };

            return View(chartViewModel);
        }

        //    var chartViewModel = new ChartViewModel {
        //        ActualData = tickData,
        //        RegressionData = regData,
        //        RegressionLineData = regLineData
        //    };

            //    return View(chartViewModel);
            //}

        private List<TickData> GetRegressionData(List<TickData> tickData) {
            var regData = new List<TickData>();

            for (var i = 0; i < tickData.Count; i++) {
                var regression = new LinearRegression(
                    tickData.Where(p => p.Index <= i + 1).Select(p => p.Index).ToList(),
                    tickData.Where(p => p.Index <= i + 1).Select(p => p.Price).ToList());
                var predictedValue = regression.GetPredictedYValue(i + 1);
                if (double.IsNaN(predictedValue)) {
                    predictedValue = tickData.Where(p => p.Index <= i + 1).Select(p => p.Price).Min();
                }

                regData.Add(new TickData {Index = i + 1, Price = predictedValue});
            }

            return regData;
        }

        private List<TickData> GetTickData(int pageNumber) {

            var tickDatas = new List<TickData>();

            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Data/Crude.txt"));
            if (pageNumber > lines.Length / _pageSize) {
                pageNumber = (lines.Length / _pageSize - 1);
            }

            lines = lines.Where(p => !string.IsNullOrWhiteSpace(p)).Skip(pageNumber*_pageSize).Take(_pageSize).ToArray();

            var lineNumber = 1;
            foreach (var line in lines) {
                if (string.IsNullOrWhiteSpace(line)) {
                    continue;
                }

                var lineData = line.Split(',');
                double ask;
                double.TryParse(lineData[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ask);

                var tickData = new TickData {
                    Index = lineNumber,
                    Price = ask
                };

                tickDatas.Add(tickData);
                lineNumber++;
            }

            return tickDatas;
        }

        private string OpenPosition(List<TickData> regData, List<TickData> reglineData) {
            if (regData.Count > 1 && reglineData.Count > 1) {
                if (regData[regData.Count - 1].Price > reglineData[reglineData.Count - 1].Price) {
                    if (regData[regData.Count - 2].Price <= reglineData[reglineData.Count - 2].Price) {
                        return string.Format("Long at {0} {1}", regData[regData.Count - 1].Index,
                            regData[regData.Count - 1].Price);
                    }
                }

                if (regData[regData.Count - 1].Price < reglineData[reglineData.Count - 1].Price) {
                    if (regData[regData.Count - 2].Price >= reglineData[reglineData.Count - 2].Price) {
                        return string.Format("Short at {0} {1}", regData[regData.Count - 1].Index,
                            regData[regData.Count - 1].Price);
                    }
                }
            }

            return "None";
        }

        private void DumpDataToFile(List<TickData> actualData, List<TickData> RegData, List<TickData> ReglineData) {
            var filePath = string.Format("Data/{0}.txt", "All");
            System.IO.File.Delete(Server.MapPath(filePath));

            for (int i = 0; i < actualData.Count; i++) {
                var actData = actualData[i];
                var regData = RegData[i];
                var reglineData = ReglineData[i];
                var line = string.Format("{0},{1},{2},{3}{4}", i, actData.Price, regData.Price,
                    reglineData.Price,
                    Environment.NewLine);

                System.IO.File.AppendAllLines(Server.MapPath(filePath), new List<string> { line });
            }
        }
    }
}