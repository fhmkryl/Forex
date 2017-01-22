using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Forex.Core.Entity;
using Forex.Core.MathLib;

namespace Forex.TestWinForm {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
           
        }

        private void btnPredict_Click(object sender, EventArgs e) {
            var tickData = GetTickData();
            var numberOfPoints = Convert.ToInt32(txtNumberOfPoints.Text);
            var regData = tickData.Where(p => p.Index < numberOfPoints).ToList();

            var regression = new LinearRegression(regData.Select(p => p.Index).ToList(),
                regData.Select(p => p.Price).ToList());

            // Clear chart
            chartPrices.Series.Clear();
            chartPrices.ChartAreas[0].AxisY.Interval = 0.01;

            // Actual data
            var tSeries = new Series {
                Name = "tSeries",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            chartPrices.Series.Add(tSeries);

            foreach (var tData in tickData.Where(p => p.Index <= numberOfPoints)) {
                tSeries.Points.AddXY(tData.Index, tData.Price);
            }

            // Regression data
            var rSeries = new Series {
                Name = "rSeries",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };
            chartPrices.Series.Add(rSeries);

            foreach (var rData in regData) {
                rSeries.Points.AddXY(rData.Index, rData.Price);
            }

            // Add predicted point
            var predictedValue = regression.GetPredictedYValue(regData.Count + 1);
            rSeries.Points.AddXY(regData.Count + 1, predictedValue);
        }

        private List<TickData> GetTickData() {

            var tickDatas = new List<TickData>();

            var lines = System.IO.File.ReadAllLines("DAT_ASCII_EURUSD_T_201701.csv");
            int lineNumber = 1;
            foreach (var line in lines) {
                var lineData = line.Split(',');
                double ask;
                double bid;
                double.TryParse(lineData[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ask);
                double.TryParse(lineData[2], NumberStyles.Any, CultureInfo.InvariantCulture, out bid);

                var tickData = new TickData {
                    Index = lineNumber,
                    Price = ask
                };

                tickDatas.Add(tickData);
                lineNumber++;
            }

            return tickDatas;
        }
    }
}
