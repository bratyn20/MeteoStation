using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_V.Controllers
{
    public class Statistics
    {
        public Trendline CalculateLinearRegression(Double[] values, Double[] values2)
        {
            var yAxisValues = new List<Double>();
            var xAxisValues = new List<Double>();

            for (int i = 0; i < values.Length; i++)
            {
                yAxisValues.Add(values[i]);
                xAxisValues.Add(values2[i]);
            }

            return new Trendline(yAxisValues, xAxisValues);
        }
    }

    public class Trendline
    {
        private readonly List<Double> xAxisValues;
        private readonly List<Double> yAxisValues;
        private int count;
        private Double xAxisValuesSum;
        private Double xxSum;
        private Double xySum;
        private Double yAxisValuesSum;

        public Trendline(List<Double> yAxisValues, List<Double> xAxisValues)
        {
            this.yAxisValues = yAxisValues;
            this.xAxisValues = xAxisValues;

            this.Initialize();
        }

        public Double Slope { get; private set; }
        public Double Intercept { get; private set; }
        public Double Start { get; private set; }
        public Double End { get; private set; }

        private void Initialize()
        {
            this.count = this.yAxisValues.Count;
            this.yAxisValuesSum = this.yAxisValues.Sum();
            this.xAxisValuesSum = this.xAxisValues.Sum();
            this.xxSum = 0;
            this.xySum = 0;

            for (int i = 0; i < this.count; i++)
            {
                this.xySum += (this.xAxisValues[i] * this.yAxisValues[i]);
                this.xxSum += (this.xAxisValues[i] * this.xAxisValues[i]);
            }

            this.Slope = this.CalculateSlope();
            this.Intercept = this.CalculateIntercept();
            this.Start = this.CalculateStart();
            this.End = this.CalculateEnd();
        }

        private Double CalculateSlope()
        {
            try
            {
                return ((this.count * this.xySum) - (this.xAxisValuesSum * this.yAxisValuesSum)) / ((this.count * this.xxSum) - (this.xAxisValuesSum * this.xAxisValuesSum));
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        private Double CalculateIntercept()
        {
            return (this.yAxisValuesSum - (this.Slope * this.xAxisValuesSum)) / this.count;
        }

        private Double CalculateStart()
        {
            return (this.Slope * this.xAxisValues.First()) + this.Intercept;
        }

        private Double CalculateEnd()
        {
            return (this.Slope * this.xAxisValues.Last()) + this.Intercept;
        }
    }
}