using ChartUIControls.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChartUIControls.Controls.ChartData;
using ChartUIControls.Controls.Legend;
using ChartUIControls.Controls.Utils;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.Shared.UIModels;
using WindowsClient.Shared.UIModels.MaterialBalance;
using WindowsClient.Shared.Visualisation;
using System.Collections.ObjectModel;
using WindowsClient.Services.Calculation;
using WindowsClient.Shared.Utils;
using WindowsClient.ApplicationLayout.ToastNotification;

namespace WindowsClient.Shared.Visualisation
{
    public class ChartWrapper : ExternalYChartControl, INotifyPropertyChanged, IChartWrapper
    {
        public ChartWrapper()
        {
            InitializeChart();
            SetupItemSelection();
            SubscribeToEvents();
            SelectedRegressionResult.SelectedDataPoint = new List<DataPointModel2>();
            TurnedOnPoints = new (true, true);
            TurnedOffPoints = new (true, true);
            _calculationServices = Ioc.Resolve<ICalculationServices>();
            _toastNotification = Ioc.Resolve<IToastNotification>();
        }

        void InitializeChart()
        {
            LegendPosition = LegendPositionEnum.NorthEastInside;
            Title = "Title";
            RightAxisCount = 0;
            GetXAxis().AxisType = DataTypeEnum.Linear;
            GetXAxis().AutomaticTickGeneration = true;

        }
        void SetupItemSelection()
        {
            itemSelector = new ChartItemSelector();
            itemSelector.AddItem(this);
        }

        void SubscribeToEvents()
        {
            ChartArea.MouseLeftButtonDown += ChartArea_MouseLeftButtonDown;
            ChartArea.ChartCanvas.MouseMove += ChartCanvas_MouseMove; ;
            ChartArea.ChartCanvas.PreviewMouseLeftButtonDown += ChartCanvasOnPreviewMouseLeftButtonDown;
            ChartArea.ChartCanvas.MouseRightButtonDown += ChartCanvas_MouseRightButtonDown;
            ChartArea.ChartCanvas.MouseLeftButtonUp += ChartCanvas_MouseLeftButtonUp;
            getSelectedDataCollectionDelegate += Chart_getSelectedDataCollectionDelegate;
            RaisePointSelectionEvent += Chart_raisePointSelectionEvent;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private LineChartDataSeries CreateLineSeries(Brush color, string SeriesName)
        {
            LineChartDataSeries dataseries = new LineChartDataSeries();
            dataseries.LineType = LineTypeEnum.Spline;
            dataseries.SymbolColor = color;
            dataseries.SymbolType = SymbolTypeEnum.Circle;
            dataseries.LineColor = color;
            dataseries.LineThickness = 2;
            dataseries.SymbolSize = 4;
            dataseries.SeriesName = SeriesName;
            dataseries.XaxisDataType = DataTypeEnum.Date;
            dataseries.YaxisDataType = DataTypeEnum.Linear;

            return dataseries;
        }
        public void RefreshPlot()
        {
            this.Dispatcher.Invoke(() =>
            {
                Data.Clear();
                LeftAxisCount = 1;
                RightAxisCount = 0;
                GetLeftYAxis(1).Title = "YAxis";
                GetLeftYAxis(1).FontColor = Brushes.Black;
                GetXAxis().RemoveLabels();
                GetXAxis().Title = "Date";
                vacantYAxis.Clear();
            });
        }


        #region updateAxes
        private DateTime GetMinDate(DataSeriesCollection dsCollection)
        {
            DateTime minDate = DateTime.MaxValue;

            if (Data.Count > 0)
            {
                foreach (DataPoint dataPoint in Data[0].Data)
                {
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

            //foreach (DataSeries dataSeries in dsCollection)
            //{
            //    foreach (DataPoint dataPoint in dataSeries.Data)
            //    {
            //        try
            //        {
            //            var dateVal = (DateTime)dataPoint.XValue;
            //            if (dateVal < minDate)
            //            {
            //                minDate = dateVal;
            //            }

            //        }
            //        catch
            //        {
            //            continue;
            //        }

            //    }
            //}
            return minDate;
        }

        private DateTime GetMaxDate(DataSeriesCollection dsCollection)
        {
            DateTime maxDate = DateTime.MinValue;
            DateTime previousDate = DateTime.MinValue;
            List<double> differences = new List<double>();
            if (Data.Count > 0)
            {
                foreach (DataPoint dataPoint in Data[0].Data)
                {
                    try
                    {
                        var dateVal = (DateTime)dataPoint.XValue;

                        if (previousDate != DateTime.MinValue && Data[0].Data.Last() != dataPoint)
                        {
                            differences.Add(Math.Abs((dateVal - previousDate).TotalDays / 365));
                        }
                        if (dateVal > maxDate)
                        {
                            maxDate = dateVal;
                        }
                        previousDate = dateVal;
                    }
                    catch
                    {
                        continue;
                    }
                }
                currentTick = differences.Sum() / differences.Count;
                differences.Clear();
            }
            //foreach (DataSeries dataSeries in dsCollection)
            //{
            //    foreach (DataPoint dataPoint in dataSeries.Data)
            //    {
            //        try
            //        {
            //            var dateVal = (DateTime)dataPoint.XValue;

            //            if (previousDate != DateTime.MinValue && dataSeries.Data.Last() != dataPoint)
            //            {
            //                differences.Add(Math.Abs((dateVal - previousDate).TotalDays / 365));
            //            }
            //            if (dateVal > maxDate)
            //            {
            //                maxDate = dateVal;
            //            }
            //            previousDate = dateVal;
            //        }
            //        catch
            //        {
            //            continue;
            //        }
            //    }
            //}

            return maxDate;
        }
        #endregion

        #region DataSelectionAlgorithms
        private void ChartCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPolygonSelection)
            {
                if (polygon != null)
                {
                    chartArea.ChartCanvas.CaptureMouse();
                    segment.Points[1] = e.GetPosition(ChartArea.ChartCanvas);
                    segment.Stroke = Brushes.Red;
                    segment.StrokeThickness = 2;
                    segment.StrokeDashArray = new DoubleCollection() { 2 };
                }
            }

            if (isBoxSelection)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    e.Handled = false;
                    chartArea.ChartCanvas.CaptureMouse();

                    if (_startPoint.Equals(default(Point)))
                    {

                    }
                    else
                    {
                        _endPoint = e.GetPosition(chartArea.ChartCanvas);
                        //set position of rectangle
                        var x = Math.Min(_endPoint.X, _startPoint.X);
                        var y = Math.Min(_endPoint.Y, _startPoint.Y);
                        //set dimension of rectangle so that you can draw from any sides
                        var w = Math.Max(_endPoint.X, _startPoint.X) - x;
                        var h = Math.Max(_endPoint.Y, _startPoint.Y) - y;
                        _rectangle.Width = w;
                        _rectangle.Height = h;
                        Canvas.SetLeft(_rectangle, x);
                        Canvas.SetTop(_rectangle, y);
                    }
                }
                else
                {

                }
            }
        }

        private void ChartCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isPolygonSelection)
            {
                if (polygon != null)
                {
                    segment.Points[1] = e.GetPosition(ChartArea.ChartCanvas);
                    polygon.Points.Add(segment.Points[1]);
                    segment.Points[0] = segment.Points[1];
                }
            }


            if (isBoxSelection)
            {
                e.Handled = true;
                chartArea.ChartCanvas.ReleaseMouseCapture();
                if (_rectangle != null)
                {
                    if (Data.Count > 0)
                    {
                        var derivativeSeries = GetSeriesToBeSelected();
                        if (derivativeSeries != null)
                        {
                            LineChartDataSeries thisSeries = (LineChartDataSeries)derivativeSeries;
                            List<DataPointModel2> dpsSelected = new List<DataPointModel2>();
                            List<DataPointModel2> unSelected = new List<DataPointModel2>();
                            for (int i = 0; i < thisSeries.Data.Count; i++)
                            {
                                DataPointModel2 thisPoint = thisSeries.LineSeries.DataPoints2[i];
                                if (thisPoint.YValue != null && thisPoint.XValue != null)
                                {
                                    if (thisPoint.X == null || thisPoint.Y == null)
                                    {
                                        //return;
                                    }
                                    else
                                    {
                                        if ((double)thisPoint.X >= _startPoint.X && (double)thisPoint.Y >= _startPoint.Y &&
                                                                            (double)thisPoint.X <= _endPoint.X && (double)thisPoint.Y <= _endPoint.Y)
                                        {
                                            var selectedPoint = SelectedRegressionResult.SelectedDataPoint.Where(x => x.ToolTip.ToString() == thisPoint.ToolTip.ToString()).FirstOrDefault();
                                            if (selectedPoint == null) unSelected.Add(thisPoint);
                                            else dpsSelected.Add(selectedPoint);
                                        }
                                        else if ((double)thisPoint.X >= _endPoint.X && (double)thisPoint.Y >= _endPoint.Y &&
                                                                            (double)thisPoint.X <= _startPoint.X && (double)thisPoint.Y <= _startPoint.Y)
                                        {
                                            var selectedPoint = SelectedRegressionResult.SelectedDataPoint.Where(x => x.ToolTip.ToString() == thisPoint.ToolTip.ToString()).FirstOrDefault();
                                            if (selectedPoint == null) unSelected.Add(thisPoint);
                                            else dpsSelected.Add(selectedPoint);
                                        }
                                    }

                                }
                            }
                            AddOrRemoveSelectedPoints(dpsSelected, unSelected);

                            if (isTurnedOn) TurnOnPointsOnChart().Await();
                            if (isTurnedOff) TurnOffPointsOnChart().Await();

                            e.Handled = true;
                            //SendSelectedDataPoints();
                        }

                    }
                }

                _startPoint = new Point();

                chartArea.ChartCanvas.Children.Remove(_rectangle);
                ReplotChart();
                _rectangle = null;
                //SendSelectedDataPoints();
            }

        }

        private void ChartCanvasOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(ChartArea.ChartCanvas);

            //polygon
            if (isPolygonSelection)
            {
                if (polygon == null)
                {
                    //chartArea.ChartCanvas.CaptureMouse();

                    polygon = new Polygon();
                    polygon.Stroke = Brushes.Red;

                    polygon.StrokeThickness = 2;
                    polygon.StrokeDashArray = new DoubleCollection() { 2 };
                    polygon.Points.Add(point);
                    ChartArea.ChartCanvas.Children.Add(polygon);

                    segment.Stroke = Brushes.Red;
                    segment.StrokeThickness = 2;
                    segment.StrokeDashArray = new DoubleCollection() { 2 };
                    segment.Points.Add(point);
                    segment.Points.Add(point);
                    ChartArea.ChartCanvas.Children.Add(segment);


                }
            }

            if (isPointSelection)
            {
                e.Handled = true;
                if (!point.Equals(default(Point)))
                {
                    if (Data.Count > 0)
                    {
                        var derivativeSeries = GetSeriesToBeSelected();
                        if (derivativeSeries != null)
                        {
                            LineChartDataSeries thisSeries = (LineChartDataSeries)derivativeSeries;
                            List<DataPointModel2> dpsSelected = new List<DataPointModel2>();
                            List<DataPointModel2> unSelected = new List<DataPointModel2>();
                            if (thisSeries.Data.Count == 1) return;
                            for (int i = 0; i < thisSeries.Data.Count; i++)
                            {
                                var closestPoint = thisSeries.LineSeries.DataPoints2.OrderBy(item => Math.Abs(point.X - (double)item.X)).First();
                                DataPointModel2 thisPoint = thisSeries.LineSeries.DataPoints2[i];
                                if (thisPoint.YValue != null && thisPoint.XValue != null)
                                {
                                    if (closestPoint == thisPoint)
                                    {
                                        var selectedPoint = SelectedRegressionResult.SelectedDataPoint.Where(x => x.ToolTip.ToString() == thisPoint.ToolTip.ToString()).FirstOrDefault();
                                        if (selectedPoint == null) unSelected.Add(thisPoint);
                                        else dpsSelected.Add(selectedPoint);
                                    }
                                }
                            }
                            AddOrRemoveSelectedPoints(dpsSelected, unSelected);

                            if (isTurnedOn) TurnOnPointsOnChart().Await();
                            if (isTurnedOff) TurnOffPointsOnChart().Await();

                            e.Handled = true;
                        }
                        ReplotChart();
                        //SendSelectedDataPoints();
                    }
                }
            }
        }

        private void ChartArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(ChartArea.ChartCanvas);
            if (isPolygonSelection)
            {
                ChartArea.ChartCanvas.CaptureMouse();
                if (polygon == null)
                {
                    polygon = new Polygon();
                    polygon.Stroke = Brushes.Red;
                    polygon.StrokeThickness = 2;
                    polygon.StrokeDashArray = new DoubleCollection() { 2 };
                    polygon.Points.Add(point);
                    ChartArea.ChartCanvas.Children.Add(polygon);
                    segment.Stroke = Brushes.Red;
                    segment.StrokeThickness = 2;
                    segment.StrokeDashArray = new DoubleCollection() { 2 };
                    segment.Points.Add(point);
                    segment.Points.Add(point);
                    ChartArea.ChartCanvas.Children.Add(segment);
                }
                e.Handled = true;
            }
            if (isBoxSelection)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    chartArea.ChartCanvas.CaptureMouse();

                    if (_startPoint.Equals(default(Point)))
                    {
                        // SelectedRegressionResult.SelectedDataPoint.Clear();
                        _rectangle = new Rectangle();
                        _rectangle.Stroke = Brushes.Red;
                        _rectangle.StrokeThickness = 2;
                        _rectangle.StrokeDashArray = new DoubleCollection() { 2 };
                        _rectangle.Opacity = 0.3;
                        _startPoint = e.GetPosition(chartArea.ChartCanvas);
                        Canvas.SetTop(_rectangle, _startPoint.Y);
                        Canvas.SetLeft(_rectangle, _startPoint.X);
                        chartArea.ChartCanvas.Children.Add(_rectangle);
                    }
                }
                e.Handled = true;
            }
        }

        private void ChartCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isPolygonSelection)
            {
                e.Handled = true;
                if (polygon != null)
                {
                    // polygon = null;
                    segment.Points.Clear();
                    ChartArea.ChartCanvas.Children.Remove(segment);

                    if (Data.Count > 0)
                    {
                        var derivativeSeries = GetSeriesToBeSelected();
                        if (derivativeSeries != null)
                        {

                            A = new List<DataPointModel2>();
                            B = new List<DataPointModel2>();
                            LineChartDataSeries thisSeries = (LineChartDataSeries)derivativeSeries;

                            for (int x = 0; x < thisSeries.Data.Count; x++)
                            {
                                DataPointModel2 thisPoint = thisSeries.LineSeries.DataPoints2[x];
                                if (thisPoint.YValue != null && thisPoint.XValue != null)
                                {
                                    PointInPolygon(thisPoint, polygon.Points, thisSeries.Data[x]);
                                }
                            }
                            var hasSameElements = A.Intersect(B);

                            List<DataPointModel2> dpsSelected = new List<DataPointModel2>();
                            List<DataPointModel2> unSelected = new List<DataPointModel2>();
                            foreach (var item in hasSameElements)
                            {
                                var selectedPoint = SelectedRegressionResult.SelectedDataPoint.Where(x => x.ToolTip.ToString() == item.ToolTip.ToString()).FirstOrDefault();
                                if (selectedPoint == null) unSelected.Add(item);
                                else dpsSelected.Add(selectedPoint);
                            }

                            AddOrRemoveSelectedPoints(dpsSelected, unSelected);

                            if (isTurnedOn) TurnOnPointsOnChart();
                            if (isTurnedOff) TurnOffPointsOnChart();

                            e.Handled = true;
                        }
                    }

                    ChartArea.ChartCanvas.Children.Remove(polygon);
                    ReplotChart();
                    polygon = null;
                    ChartArea.ChartCanvas.ReleaseMouseCapture();

                    //SendSelectedDataPoints();
                }
            }
        }

        private void AddOrRemoveSelectedPoints(List<DataPointModel2> dpsSelected, List<DataPointModel2> unSelected)
        {
            if (dpsSelected.Count > 0)
            {
                foreach (var dataPoint in dpsSelected)
                {
                    var check = SelectedRegressionResult.SelectedDataPoint.Contains(dataPoint);
                    if (check)
                    {
                        SelectedRegressionResult.SelectedDataPoint.Remove(dataPoint);
                        RemoveSelectedPointSeries(dataPoint);
                        RemoveSomeSelectedDataPoints(new List<DataPointModel2> { dataPoint });
                    }
                }
            }
            if (unSelected.Count > 0)
            {
                foreach (var dataPoint in unSelected)
                {
                    var check = SelectedRegressionResult.SelectedDataPoint.Exists(x => x.YValue == dataPoint.YValue && x.XValue == dataPoint.XValue);
                    if (!check)
                    {
                        SelectedRegressionResult.SelectedDataPoint.Add(dataPoint);
                        AddSelectedPointSeries(dataPoint);
                    }
                    SendSelectedDataPoints();
                }
            }
        }

        private List<DataPointModel2> A { get; set; }
        private List<DataPointModel2> B { get; set; }

        private void PointInPolygon(DataPointModel2 p, PointCollection poly, DataPoint dataPoint)
        {
            Point p1, p2;
            Point oldPoint = new Point(poly[poly.Count - 1].X, poly[poly.Count - 1].Y);
            if (p.X == null || p.Y == null)
            {
                return;
            }
            for (int i = 0; i < poly.Count; i++)
            {
                Point newPoint = new Point(poly[i].X, poly[i].Y);
                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }
                if ((newPoint.X < p.X) == (p.X <= oldPoint.X) && (((double)p.Y - p1.Y) * (p2.X - p1.X) > (p2.Y - p1.Y) * (double)(p.X - p1.X)))
                {
                    var pt = new Point(p.X.Value, p.Y.Value);
                    //AddPointSymbol(p.X.Value, p.Y.Value);
                    A.Add(p);
                }
                if ((newPoint.X < p.X) == (p.X <= oldPoint.X) && (((double)p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (double)(p.X - p1.X)))
                {
                    var pt = new Point(p.X.Value, p.Y.Value);
                    //AddPointSymbol(p.X.Value, p.Y.Value);
                    B.Add(p);
                }
                oldPoint = newPoint;
            }

        }

        private DataSeries GetSeriesToBeSelected()
        {
            return Data.Where(x => x.GetType() == typeof(LineChartDataSeries) && x.SeriesName.Contains("Measured")).FirstOrDefault();
        }

        public void RemoveAllSelectedPoints()
        {
            SelectedRegressionResult.SelectedDataPoint.Clear();
            SelectedRegressionResult.SelectedPoints.Data.Clear();
            ReplotChart();
        }
        private void AddSelectedPointSeries(DataPointModel2 dataPoint)
        {
            DataPoint thisDataPoint = new DataPoint();
            thisDataPoint.XValue = dataPoint.XValue;
            thisDataPoint.YValue = dataPoint.YValue;
            thisDataPoint.IsSymbolVisible = dataPoint.IsSymbolVisible;
            thisDataPoint.UseAsSymbolOnly = dataPoint.UseAsSymbolOnly;
            thisDataPoint.Tooltip = dataPoint.ToolTip;
            if (SelectedRegressionResult.SelectedPoints == null)
                SelectedRegressionResult.SelectedPoints = new();
            SelectedRegressionResult.SelectedPoints.Data.Add(thisDataPoint);

            //ReplotChart();
        }
        private void RemoveSelectedPointSeries(DataPointModel2 dataPoint)
        {
            //DataPoint dataPointToRemove = (DataPoint)SelectedRegressionResult.SelectedPoints.Data.Where(x => x.XValue == xValues && x.YValue == yValues).FirstOrDefault();
            DataPoint dataPointToRemove = (DataPoint)SelectedRegressionResult.SelectedPoints.Data.Where(x => x.Tooltip.ToString() == dataPoint.ToolTip.ToString()).FirstOrDefault();
            SelectedRegressionResult.SelectedPoints.Data.Remove(dataPointToRemove);
        }

        private void SendSelectedDataPoints()
        {
            if (RaisePointSelectionEvent != null)
            {
                RaisePointSelectionEvent.Invoke(SelectedRegressionResult.SelectedDataPoint);
            }
        }

        private void RemoveSomeSelectedDataPoints(List<DataPointModel2> dataPoints)
        {
            if (getSelectedDataCollectionDelegate != null)
            {
                getSelectedDataCollectionDelegate.Invoke(dataPoints);
            }
        }

        #endregion

        #region TehraniMethods
        public void PlotMeasuredNp()
        {
            Title = $"Tehrani Analysis Plot for {tank.Name}";
            XAxisLabel = "Cumulative Production";
            YAxisLabel = "Pressure"; //TODO: Axis to include units
            GetLeftYAxis(1).Title = "Pressure (Psia)";
            GetXAxis().Title = tank.FlowingFluid == FluidType.Oil ? "Cumulative Production (MMSTB)" : "Cumulative Production (MMSCF)";
            GetXAxis().ExtendAxisRange = false;
            HistoryPlot = tank.GetPressureVsCummulativeProduction();
            if (HistoryPlot.Count == 0) return;
            //GetXAxis().AutomaticTickGeneration = false;
            //GetXAxis().Xmin = 0;
            //GetXAxis().XTick = 5;
            //GetXAxis().Xmax = HistoryPlot.Select(x => x.X).Max() + 10;
            HistoryPlot.Color = "Blue";
            HistoryPlot.Name = tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = tank.FlowingFluid == FluidType.Oil ? "Measured Np" : "Measured Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.SymbolSize = 5;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("Blue");
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineThickness = 0;
            lineDataSeries.LineType = LineTypeEnum.StairStep;

            foreach (var item in HistoryPlot)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            Data.Add(lineDataSeries);
            itemSelector.AddItem(this);
            ReplotChart();
        }

        public void PlotSelectedPoints()
        {
            GetXAxis().AxisType = DataTypeEnum.Linear;
            SelectedRegressionResult.SelectedPoints = new ScatterChartDataSeries();
            SelectedRegressionResult.SelectedPoints.YAxis = GetLeftYAxis(1).ID.ToString();
            SelectedRegressionResult.SelectedPoints.YaxisDataType = DataTypeEnum.Logarithmic;
            SelectedRegressionResult.SelectedPoints.SymbolSize = 5;
            SelectedRegressionResult.SelectedPoints.FillPattern = FillPatternEnum.Solid;
            SelectedRegressionResult.SelectedPoints.SymbolColor = ConverterUtils.StringToBrush("LightGray");
            SelectedRegressionResult.SelectedPoints.ScatterSeries.IsContextMenuVisible = false;
            SelectedRegressionResult.SelectedPoints.IsXAxisExtended = false;
            SelectedRegressionResult.SelectedPoints.SeriesName = "Selected Point";

            SelectedRegressionResult.SelectedPoints.GetLegendItem().Visibility = ConverterUtils.GetVisibility("Collapsed");
        }

        public void PlotSelectedPoints1()
        {
            if (TurnedOffPoints.Count == 0)
            {
                if (SelectedRegressionResult.SelectedDataPoint.Count == 0) 
                {
                    ResultAccepted?.Invoke(SelectedRegressionResult);
                    return;
                } 
                foreach (var item in SelectedRegressionResult.SelectedDataPoint)
                    TurnedOffPoints.Add(((double)item.XValue, (double)item.YValue));
            }

            GetXAxis().AxisType = DataTypeEnum.Linear;
            SelectedRegressionResult.SelectedPoints = new ScatterChartDataSeries();
            SelectedRegressionResult.SelectedPoints.YAxis = GetLeftYAxis(1).ID.ToString();
            SelectedRegressionResult.SelectedPoints.YaxisDataType = DataTypeEnum.Logarithmic;
            SelectedRegressionResult.SelectedPoints.SymbolSize = 5;
            SelectedRegressionResult.SelectedPoints.FillPattern = FillPatternEnum.Solid;
            SelectedRegressionResult.SelectedPoints.SymbolColor = ConverterUtils.StringToBrush("LightGray");
            SelectedRegressionResult.SelectedPoints.IsXAxisExtended = false;
            SelectedRegressionResult.SelectedPoints.SeriesName = "Selected Point";

            SelectedRegressionResult.SelectedPoints.GetLegendItem().Visibility = ConverterUtils.GetVisibility("Collapsed");

            //----------------------------//
            SelectedRegressionResult.SelectedDataPoint.Clear();
            //----------------------------//

            for (int i = 0; i < TurnedOffPoints.Count; i++)
            {
                DataPoint thisDataPoint = new DataPoint();
                thisDataPoint.XValue = TurnedOffPoints[i].X;
                thisDataPoint.YValue = TurnedOffPoints[i].Y;
                thisDataPoint.Tooltip = $"X: {TurnedOffPoints[i].X}   Y: {TurnedOffPoints[i].Y}";
                SelectedRegressionResult.SelectedPoints.Data.Add(thisDataPoint);

                //Added this
                //----------------------------//
                DataPointModel2 modelPoint = new DataPointModel2();
                modelPoint.XValue = TurnedOffPoints[i].X;
                modelPoint.YValue = TurnedOffPoints[i].Y;
                modelPoint.ToolTip = $"X: {TurnedOffPoints[i].X}   Y: {TurnedOffPoints[i].Y}";
                SelectedRegressionResult.SelectedDataPoint.Add(modelPoint);
                //----------------------------//
            }

            TurnedOffPoints.Clear();
            Data.Add(SelectedRegressionResult.SelectedPoints);

            itemSelector.AddItem(this);

            ResultAccepted?.Invoke(SelectedRegressionResult);
        }

        public void OnResultAccepted(RegressionResult result)
        {
            result.Name = $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}";
            if (!RegressionResults.Any(x => x.Id == result.Id))
                RegressionResults.Add(result);

            result.EstimatedNpSeries.Color = "firebrick";
            result.EstimatedNpSeries.Name = tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            result.EstimatedNpSeries.ShowLine = true;
            result.EstimatedNpSeries.LineWidth = 3;

            SelectedRegressionResult = RegressionResults.FirstOrDefault(x => x == result);
        }

        private void PlotEstimatedChartOnSelection(RegressionResult result)
        {
            SetCurrentTankResult(result);
            RefreshPlot();
            PlotMeasuredNp();

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.SymbolSize = 0;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineType = LineTypeEnum.Spline;
            lineDataSeries.LineThickness = 2;



            foreach (var item in result.EstimatedNpSeries)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }



            Data.Add(lineDataSeries);
            itemSelector.AddItem(this);
            if (result.TurnOffPoints != null && result.TurnOffPoints.Count > 0)
            {
                foreach (var item in SelectedRegressionResult.TurnOffPoints)
                {
                    TurnedOffPoints.Add((item.X, item.Y));
                }
                PlotSelectedPoints1();
            }
            else 
            {
                foreach (var item in result.SelectedDataPoint)
                    TurnedOffPoints.Add(((double)item.XValue, (double)item.YValue));

                PlotSelectedPoints1();
            }
            ReplotChart();
        }

        private void SetCurrentTankResult(RegressionResult result)
        {
            tank.STOIP.CurrentValue = result.HistoryMatchingVariables.STOIP.BestFitValue;
            tank.Thickness.CurrentValue = result.HistoryMatchingVariables.Thickness.BestFitValue;
            tank.Radius.CurrentValue = result.HistoryMatchingVariables.Radius.BestFitValue;
            tank.Length.CurrentValue = result.HistoryMatchingVariables.Length.BestFitValue;
            tank.Width.CurrentValue = result.HistoryMatchingVariables.Width.BestFitValue;
            tank.GasCap.CurrentValue = result.HistoryMatchingVariables.GasCap.BestFitValue;
            tank.Aquifer.EncroachmentAngle.CurrentValue = result.HistoryMatchingVariables.EncroachmentAngle.BestFitValue;
            tank.Aquifer.OuterInnerRadiusRatio.CurrentValue = result.HistoryMatchingVariables.OuterInnerRadius.BestFitValue;
            tank.Aquifer.Volume.CurrentValue = result.HistoryMatchingVariables.Volume.BestFitValue;
            tank.Rock.Permeability.CurrentValue = result.HistoryMatchingVariables.Permeability.BestFitValue;
            tank.Rock.Anisotropy.CurrentValue = result.HistoryMatchingVariables.Anisotropy.BestFitValue;
            tank.Rock.Porosity.CurrentValue = result.HistoryMatchingVariables.Porosity.BestFitValue;
            tank.GIIP.CurrentValue = result.HistoryMatchingVariables.GIIP.BestFitValue;
        }

        public void EstimatedNpPlot(EstimateResult result, Tank _tank = null)
        {
            if (SelectedRegressionResult != null)
            {
                SelectedRegressionResult.EstimatedNpSeries = result.EstimatedNpSeries;
                SelectedRegressionResult.SelectedTdSeries = result.SelectedTdSeries;
                SelectedRegressionResult.WdSeriesAtOtherRadi = result.WdSeriesAtOtherRadi;
                SelectedRegressionResult.NewWdSeriesAtOtherRadi = result.NewWdSeriesAtOtherRadi;
                SelectedRegressionResult.TdSeriesForReservoirRadiusSeries = result.TdSeriesForReservoirRadiusSeries;
                SelectedRegressionResult.ProductionEstimationResult = result.ProductionEstimationResult;
            }

            result.EstimatedNpSeries.Color = "firebrick";
            if (tank == null) tank = _tank;
            result.EstimatedNpSeries.Name = tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            result.EstimatedNpSeries.ShowLine = true;
            result.EstimatedNpSeries.LineWidth = 3;

            LineChartDataSeries lineDataSeries = new LineChartDataSeries();
            lineDataSeries.LineSeries.Tag = lineDataSeries;
            lineDataSeries.SeriesName = tank.FlowingFluid == FluidType.Oil ? "Estimated Np" : "Estimated Gp";
            lineDataSeries.SymbolColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.SymbolSize = 0;
            lineDataSeries.LineColor = ConverterUtils.StringToBrush("firebrick");
            lineDataSeries.LinePattern = LinePatternEnum.Solid;
            lineDataSeries.LineType = LineTypeEnum.Spline;
            lineDataSeries.LineThickness = 2;

            foreach (var item in result.EstimatedNpSeries)
            {
                lineDataSeries.Data.Add(new DataPoint { XValue = item.X, YValue = item.Y, Tooltip = $"X: {item.X}   Y: {item.Y}" });
            }

            Data.Add(lineDataSeries);
            itemSelector.AddItem(this);
        }

        private void Chart_raisePointSelectionEvent(List<DataPointModel2> selectedDataPoints)
        {
            isTurnedOff = true;
            SelectedRegressionResult.TurnOffPoints = new(true, true);
            TurnedOffPoints = GetTurnedOffNPCum_TurnedOffPoints(selectedDataPoints);
        }

        private void Chart_getSelectedDataCollectionDelegate(List<DataPointModel2> selectedDataPoints)
        {
            isTurnedOn = true;
            foreach (var item in selectedDataPoints)
            {
                TurnedOnPoints.Add(((double)item.XValue, (double)item.YValue));
            }
        }

        private XYDataSeries GetTurnedOffNPCum_TurnedOffPoints(List<DataPointModel2> selectedDataPoints)
        {
            var series = new XYDataSeries(true, true);
            AllItems = new List<DataPointModel2>();
            var historyPlot = tank.GetPressureVsCummulativeProduction();
            foreach (var item in historyPlot)
            {
                DataPointModel2 modelPoint = new DataPointModel2();
                modelPoint.XValue = item.X;
                modelPoint.YValue = item.Y;
                modelPoint.ToolTip = $"X: {item.X}   Y: {item.Y}";
                AllItems.Add(modelPoint);
            }
            foreach (var item in AllItems)
            {
                var check = selectedDataPoints.Exists(x => x.ToolTip.Equals(item.ToolTip));
                if (check)
                {
                    var index = AllItems.FindIndex(x => x == item);
                    series.Add(((double)AllItems[index].XValue, (double)AllItems[index].YValue));//AllPressures
                }
            }
            return series;
        }

        private async Task TurnOnPointsOnChart()
        {
            var count = TurnedOnPoints.Count;
            foreach (var item in TurnedOnPoints)
            {
                if (SelectedRegressionResult.TurnOffPoints.Contains(item))
                    SelectedRegressionResult.TurnOffPoints.Remove(item);
            }
            try
            {
                var estimateResult = await _calculationServices.HistoryMatchingService.SingleTankEstimateAsync(tank, this, TurnedOnPoints, true);
                RefreshPlot();
                PlotMeasuredNp();
                EstimatedNpPlot(estimateResult);
                PlotSelectedPoints1();
                ReplotChart();
                TurnedOnPoints = new(true, true);
                isTurnedOn = false;
            }
            catch { }
            
        }

        private async Task TurnOffPointsOnChart()
        {
            var count = TurnedOffPoints.Count;
            foreach (var item in TurnedOffPoints)
            {
                var check = RegressionResults.Any(x => x.TurnOffPoints != null && x.TurnOffPoints.Contains(item));
                SelectedRegressionResult.TurnOffPoints.Add(item);
            }

            try
            {
                var estimateResult = await _calculationServices.HistoryMatchingService.SingleTankEstimateAsync(tank, this, TurnedOffPoints);
                RefreshPlot();
                PlotMeasuredNp();
                EstimatedNpPlot(estimateResult);
                PlotSelectedPoints1();
                ReplotChart();
                isTurnedOff = false;
                TurnedOffPoints = new(true, true);
            }
            catch { }
        }
        #endregion

        public void PlotEnergyAction(List<DateTime> xValues, List<double> yValues, string seriesName, string currentXAxisTitle, string currentYAxisTitle, Brush color)
        {
            //GetXAxis().AxisType = DataTypeEnum.Date;
            //GetXAxis().Title = currentXAxisTitle;
            //GetXAxis().ExtendAxisRange = false;
            //GetLeftYAxis(1).Title = currentYAxisTitle;
            //GetLeftYAxis(1).IsStackingEnabled = true;

            //GetLeftYAxis(1).AutomaticTickGeneration = false;
            //GetLeftYAxis(1).Ymax = 1;
            //GetLeftYAxis(1).Ymin = 0;
            //GetLeftYAxis(1).YTick = 0.2;

            //var xAxis = GetXAxis();
            //xAxis.AutomaticTickGeneration = false;
            //xAxis.Xmin = xValues.Min().Ticks;
            //xAxis.Xmax = xValues.Max().Ticks;

            AreaChartDataSeries thisSeries = new AreaChartDataSeries();
            thisSeries.YAxis = GetLeftYAxis(1).ID.ToString();
            thisSeries.YaxisDataType = DataTypeEnum.Linear;
            thisSeries.XaxisDataType = DataTypeEnum.Date;
            thisSeries.AreaFillColor = color;
            thisSeries.AreaLineType = AreaLineTypeEnum.Spline;
            thisSeries.Stroke = color;
            thisSeries.SeriesName = seriesName;

            for (int i = 0; i < xValues.Count; i++)
            {
                DataPoint thisDataPoint = new DataPoint();
                thisDataPoint.XValue = xValues[i];
                thisDataPoint.YValue = yValues[i];
                thisSeries.Data.Add(thisDataPoint);
            }
            Data.Add((thisSeries));

            itemSelector.AddItem(this);
            //ChartHelper.GetInstance().UpdateAxes(this, GetXAxis().AxisType);
            //ReplotChart();
        }

        public void PlotLineAction(
            List<double> xValues, List<double> yValues,
            string seriesName, string currentXAxisTitle,
            string currentYAxisTitle, Brush color,
            int lineThickness = 2, int symbolSize = 2,
            string xMarker = "NP", string yMarker = "Pressure")
        {
            GetXAxis().AxisType = DataTypeEnum.Linear;
            GetXAxis().Title = currentXAxisTitle;
            GetXAxis().ExtendAxisRange = false;
            GetLeftYAxis(1).Title = currentYAxisTitle;
            LineChartDataSeries thisSeries = new LineChartDataSeries();
            thisSeries.YAxis = GetLeftYAxis(1).ID.ToString();
            thisSeries.YaxisDataType = DataTypeEnum.Linear;
            thisSeries.XaxisDataType = DataTypeEnum.Linear;
            thisSeries.LineColor = color;
            thisSeries.LineThickness = lineThickness;
            thisSeries.LineType = LineTypeEnum.Normal;


            thisSeries.SymbolSize = symbolSize;
            thisSeries.SymbolColor = thisSeries.LineColor;
            thisSeries.IsXAxisExtended = false;
            thisSeries.SeriesName = seriesName;
            thisSeries.AreMarkersVisible = true;

            ModifyLegend(seriesName, thisSeries);

            for (int i = 0; i < xValues.Count; i++)
            {
                if (xValues[i] == double.NaN || xValues[i] < 0)
                    continue;

                DataPoint thisDataPoint = new DataPoint
                {
                    XValue = xValues[i],
                    YValue = yValues[i],
                    Tooltip = $"{yMarker} = {yValues[i]}\n{xMarker} = {xValues[i]}"
                };
                thisSeries.Data.Add(thisDataPoint);
            }
            Data.Add((thisSeries));
            itemSelector.AddItem(this);
        }

        private static void ModifyLegend(string seriesName, LineChartDataSeries thisSeries)
        {
            var legendItem = thisSeries.GetLegendItem();
            if (seriesName.ToLower().Contains("calculated"))
                legendItem.Children.OfType<Canvas>()
                        .First().Children.OfType<Symbol>()
                        .First().FillColor = Brushes.Transparent;
            else
                legendItem.Children.OfType<Canvas>()
                        .First().Children.OfType<Line>()
                        .First().Stroke = Brushes.Transparent;
        }

        #region TehraniProps
        private bool isTurnedOff { get; set; }
        private bool isTurnedOn { get; set; }
        private List<DataPointModel2> AllItems;
        private RegressionResult Result { get; set; }
        private XYDataSeries HistoryPlot { get; set; }
        public Tank tank { get; set; }
        public XYDataSeries TurnedOnPoints { get; set; }
        private List<ScatterChartDataSeries> NewSelectedPoints { get; set; }
        public XYDataSeries TurnedOffPoints { get; set; }
        public ObservableCollection<RegressionResult> RegressionResults { get; set; } = new();
        public event Action<RegressionResult> ResultAccepted;
        #endregion

        private double currentTick = 1;
        
        private List<int> vacantYAxis = new List<int>();

        private Rectangle _rectangle;
        private Polygon polygon;
        private Polyline segment = new Polyline();
        private Polyline polyline = new Polyline();
        private Polyline polycurve = new Polyline();
        private Point _startPoint;
        private Point _endPoint;

        public ChartItemSelector itemSelector { get; set; }
        //private ScatterChartDataSeries SelectedRegressionResult.SelectedPoints { get; set; }
        //private List<DataPointModel2> SelectedRegressionResult.SelectedDataPoint { get; set; }


        private bool isBoxSelection1;
        public bool isBoxSelection
        {
            get => isBoxSelection1; set
            {
                isBoxSelection1 = value;
                OnPropertyChanged("isBoxSelection");
            }
        }

        private bool isPolygonSelection1;
        public bool isPolygonSelection
        {
            get => isPolygonSelection1; set
            {
                isPolygonSelection1 = value;
                OnPropertyChanged("isPolygonSelection");

            }
        }

        private bool isPointSelection1;
        public bool isPointSelection
        {
            get => isPointSelection1; set
            {
                isPointSelection1 = value;
                OnPropertyChanged("isPointSelection");

            }
        }

        private bool loadingChart = false;
        public bool LoadingChart
        {
            get { return loadingChart; }
            set { loadingChart = value; OnPropertyChanged(); }
        }

        private RegressionResult _selectedRegressionResult = new();
        public RegressionResult SelectedRegressionResult
        {
            get { return _selectedRegressionResult; }
            set
            {
                _selectedRegressionResult = value;
                OnPropertyChanged(nameof(SelectedRegressionResult));
                if (value is not null)
                    PlotEstimatedChartOnSelection(value);
            }
        }

        public delegate void GetSelectedDataCollectionDelegate(List<DataPointModel2> selectedDataPoints);
        public event GetSelectedDataCollectionDelegate getSelectedDataCollectionDelegate;
        public Action<List<DataPointModel2>> RaisePointSelectionEvent;
        private readonly ICalculationServices _calculationServices;
        private readonly IToastNotification _toastNotification;
    }
}
