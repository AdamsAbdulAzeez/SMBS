using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WindowsClient.Shared.Utils
{
    public class Picasso
    {
        private Random random = new Random();
        private List<Brush> brushes = new List<Brush>();
        private int counter = 0;

        public Picasso(short brushCount)
        {
            Initialize();
        }

        private void Initialize()
        {
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Green);
            brushes.Add(Brushes.Black);
            brushes.Add(Brushes.Gold);
            brushes.Add(Brushes.Brown);
            brushes.Add(Brushes.Gray);
            brushes.Add(Brushes.Orange);
            brushes.Add(Brushes.Aqua);
            brushes.Add(Brushes.DarkBlue);
            brushes.Add(Brushes.BurlyWood);
            brushes.Add(Brushes.Coral);
            brushes.Add(Brushes.LightYellow);
            brushes.Add(Brushes.LimeGreen);
            brushes.Add(Brushes.Navy);
            brushes.Add(Brushes.Purple);
            brushes.Add(Brushes.Tan);

            BrushConverter bc = new BrushConverter();
            PropertyInfo[] propInfos = typeof(Brushes).GetProperties();
            foreach (PropertyInfo propInfo in propInfos)
            {
                var _brush = (SolidColorBrush)bc.ConvertFromString(propInfo.Name);
                if (!brushes.Contains(_brush))
                    brushes.Add(_brush);
            }
            if (brushes.Contains(Brushes.Transparent))
                brushes.Remove(Brushes.Transparent);
            if (brushes.Contains(Brushes.White))
                brushes.Remove(Brushes.White);
        }

        public Brush GetBrush()
        {
            if (counter - 1 == brushes.Count)
                counter = 0;

            Brush brush = brushes[counter];
            counter++;
            return brush;
        }

        public Brush GetBrush(int offset)
        {
            if (counter + offset - 1 == brushes.Count)
                counter = 0;

            var count = counter + offset;

            var brush = brushes[count == brushes.Count ? count - 1 : count];
            counter++;
            return brush;
        }

    }
}
