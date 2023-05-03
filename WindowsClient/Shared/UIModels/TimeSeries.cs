using System;
using System.Collections.ObjectModel;

namespace WindowsClient.Shared.UIModels
{
    public class TimeSeries : ObservableCollection<(DateTime Date, double Value)>
    {
        public TimeSeries(bool ignoreZeros)
        {
            IgnoreZeros = ignoreZeros;
        }

        public new void Add((DateTime date, double value) item)
        {
            if (item.value is 0 && IgnoreZeros) return;

            base.Add(item);
        }

        public bool IgnoreZeros { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool ShowLine { get; set; }
        public float LineWidth { get; set; }
        public bool ShowMarker { get; set; } = true;
    }
}
