using ChartUIControls.Controls;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsClient.Shared.Utils
{
    public class ChartHelper
    {
        private static ChartHelper Instance;
        public static ChartHelper GetInstance()
        {
            if (Instance != null) return Instance;
            Instance = new ChartHelper();
            return Instance;
        }

        private ChartHelper()
        {

        }

        public double GetMinValue(DataSeriesCollection dsCollection)
        {
            double minVal = double.MaxValue;

            foreach (DataSeries dataSeries in dsCollection)
            {
                foreach (DataPoint dataPoint in dataSeries.Data)
                {
                    // if((dataPoint.XValue as DateTime).CompareTo(minDate) >
                    try
                    {
                        var val = (double)dataPoint.XValue;
                        if (val < minVal)
                        {
                            minVal = val;
                            //minVal = 0;
                        }

                    }
                    catch
                    {
                        continue;
                    }

                }
            }
            return minVal;
        }

        public double GetMaxValue(DataSeriesCollection dsCollection)
        {
            double maxVal = double.MinValue;

            foreach (DataSeries dataSeries in dsCollection)
            {
                foreach (DataPoint dataPoint in dataSeries.Data)
                {
                    // if((dataPoint.XValue as DateTime).CompareTo(minDate) >
                    try
                    {
                        var val = (double)dataPoint.XValue;
                        if (val > maxVal)
                        {
                            maxVal = val;
                        }

                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return maxVal;
        }

        public DateTime GetMinDate(DataSeriesCollection dsCollection)
        {
            DateTime minDate = DateTime.MaxValue;

            foreach (DataSeries dataSeries in dsCollection)
            {
                foreach (DataPoint dataPoint in dataSeries.Data)
                {
                    // if((dataPoint.XValue as DateTime).CompareTo(minDate) >
                    try
                    {
                        var dateVal = (DateTime)dataPoint.XValue;
                        if (dateVal < minDate)
                        {
                            minDate = dateVal;
                        }

                    }
                    catch
                    {
                        continue;
                    }

                }
            }
            return minDate;
        }

        public DateTime GetMaxDate(DataSeriesCollection dsCollection)
        {
            DateTime maxDate = DateTime.MinValue;

            foreach (DataSeries dataSeries in dsCollection)
            {
                foreach (DataPoint dataPoint in dataSeries.Data)
                {
                    // if((dataPoint.XValue as DateTime).CompareTo(minDate) >
                    try
                    {
                        var dateVal = (DateTime)dataPoint.XValue;
                        if (dateVal > maxDate)
                        {
                            maxDate = dateVal;
                        }

                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return maxDate;
        }

        public String SetupYAxis(ExternalYChartControl CartesianChart, string GivenYAxisTitle, Brush color, DataTypeEnum SeriesType, int current_left_yaxis_index)
        {
            current_left_yaxis_index++;

            if (current_left_yaxis_index <= 4)
            {
                CartesianChart.LeftAxisCount = current_left_yaxis_index;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).AxisType = SeriesType;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).Title = GivenYAxisTitle;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).FontColor = color;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).ExtendAxisRange = true;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).AutomaticTickGeneration = true;
                return CartesianChart.GetLeftYAxis(current_left_yaxis_index).ID.ToString();
            }
            else
            {
                if (current_left_yaxis_index <= 7)
                {
                    current_left_yaxis_index = current_left_yaxis_index - 4;
                }
                else
                {
                    current_left_yaxis_index = 3;
                }
                CartesianChart.RightAxisCount = current_left_yaxis_index;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).AxisType = SeriesType;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).Title = GivenYAxisTitle;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).FontColor = color;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).ExtendAxisRange = true;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).AutomaticTickGeneration = true;
                return CartesianChart.GetRightYAxis(current_left_yaxis_index).ID.ToString();
            }

        }

        public String UseExistingYAxis(ExternalYChartControl CartesianChart, string GivenYAxisTitle, Brush color, DataTypeEnum SeriesType, int current_left_yaxis_index)
        {
            //current_left_yaxis_index++;

            if (current_left_yaxis_index <= 4)
            {
                //CartesianChart.LeftAxisCount = current_left_yaxis_index;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).AxisType = SeriesType;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).Title = GivenYAxisTitle;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).FontColor = color;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).ExtendAxisRange = true;
                CartesianChart.GetLeftYAxis(current_left_yaxis_index).AutomaticTickGeneration = true;
                return CartesianChart.GetLeftYAxis(current_left_yaxis_index).ID.ToString();
            }
            else
            {
                if (current_left_yaxis_index <= 7)
                {
                    current_left_yaxis_index = current_left_yaxis_index - 4;
                }
                else
                {
                    current_left_yaxis_index = 3;
                }
                //CartesianChart.RightAxisCount = current_left_yaxis_index;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).AxisType = SeriesType;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).Title = GivenYAxisTitle;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).FontColor = color;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).ExtendAxisRange = true;
                CartesianChart.GetRightYAxis(current_left_yaxis_index).AutomaticTickGeneration = true;
                return CartesianChart.GetRightYAxis(current_left_yaxis_index).ID.ToString();
            }

        }

        public void UpdateAxes(ExternalYChartControl CartesianChart, DataTypeEnum dataTypeEnum)
        {

            var xAxis = CartesianChart.GetXAxis();
            xAxis.AxisType = dataTypeEnum;
            xAxis.ExtendAxisRange = false;
            xAxis.AutomaticTickGeneration = false;

            switch (dataTypeEnum)
            {
                case DataTypeEnum.Date:

                    ((XAxisDateFormatter)xAxis.AxisFormatter).DateFormat = DateFormatEnum.DayMonthYear;

                    var minValue = GetMinDate(CartesianChart.Data);
                    var maxValue = GetMaxDate(CartesianChart.Data);

                    xAxis.Xmin = minValue.Ticks;
                    xAxis.Xmax = maxValue.Ticks;
                    break;

                case DataTypeEnum.Linear:

                    xAxis.Xmin = GetMinValue(CartesianChart.Data);
                    xAxis.Xmax = GetMaxValue(CartesianChart.Data);

                    break;
            }
        }

        public void UpdateXAxes(ExternalYChartControl CartesianChart, DataTypeEnum dataTypeEnum)
        {

            var xAxis = CartesianChart.GetXAxis();
            xAxis.AxisType = dataTypeEnum;
            xAxis.ExtendAxisRange = false;
            xAxis.AutomaticTickGeneration = false;

            switch (dataTypeEnum)
            {
                case DataTypeEnum.Date:

                    ((XAxisDateFormatter)xAxis.AxisFormatter).DateFormat = DateFormatEnum.DayMonthYear;

                    var minValue = GetMinDate(CartesianChart.Data);
                    var maxValue = GetMaxDate(CartesianChart.Data);

                    xAxis.Xmin = minValue.Ticks;
                    xAxis.Xmax = maxValue.Ticks;
                    break;

                case DataTypeEnum.Linear:

                    xAxis.Xmin = GetMinValue(CartesianChart.Data);
                    xAxis.Xmax = GetMaxValue(CartesianChart.Data);

                    break;
            }
        }

        public void UpdateAxesTitles(ExternalYChartControl CartesianChart)
        {
            if (CartesianChart.GetLeftYAxis(1).IsStackingEnabled)
            {
                String title = "";
                bool isFirst = true;
                foreach (DataSeries dataSeries in CartesianChart.Data)
                {
                    if (isFirst)
                    {
                        title = dataSeries.SeriesName.Split(new string[] { " vs " }, StringSplitOptions.None)[0];
                        isFirst = false;
                    }
                    else
                    {
                        title = title + "," + dataSeries.SeriesName.Split(new string[] { " vs " }, StringSplitOptions.None)[0];
                    }

                }

                CartesianChart.GetLeftYAxis(1).Title = title;
            }

            if (CartesianChart.Data.Count > 0)
            {
                String title = "";
                bool isFirst = true;
                foreach (DataSeries dataSeries in CartesianChart.Data)
                {
                    if (isFirst)
                    {
                        title = dataSeries.SeriesName.Split(new string[] { " vs " }, StringSplitOptions.None)[1];
                        isFirst = false;
                    }
                    else
                    {
                        title = title + "," + dataSeries.SeriesName.Split(new string[] { " vs " }, StringSplitOptions.None)[1];
                    }

                }

                CartesianChart.GetXAxis().Title = title;
            }
        }




    }
}
