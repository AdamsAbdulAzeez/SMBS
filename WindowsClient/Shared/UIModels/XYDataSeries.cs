using System.Collections.Generic;

namespace WindowsClient.Shared.UIModels
{
    public class XYDataSeries: List<(double X, double Y)>
    {
        private XYDataSeries() { }
        public XYDataSeries(bool ignoreZeroX = false, bool ignoreZeroY = false)
        {
            _ignoreZeroX = ignoreZeroX;
            _ignoreZeroY = ignoreZeroY;
        }

        public new void Add((double x, double y) item)
        {
            if (_ignoreZeroX && item.x == 0) return;
            if (_ignoreZeroY && item.y == 0) return;

            if (item.x is double.PositiveInfinity || item.x is double.NegativeInfinity ||
                item.y is double.PositiveInfinity || item.y is double.NegativeInfinity) return;

            base.Add(item);
        }

        public string Name { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string Color { get; set; }
        public bool ShowLine { get; set; }
        public float LineWidth { get; set; }
        public float MarkerSize { get; set; } = 5F;
        public bool IsInAxesTwo { get; set; }
        public bool ShowMarker { get; set; } = true;
        private readonly bool _ignoreZeroY;
        private readonly bool _ignoreZeroX;
    }
}
