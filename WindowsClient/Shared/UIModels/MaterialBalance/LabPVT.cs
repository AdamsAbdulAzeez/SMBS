using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsClient.Shared.UIModels.MaterialBalance
{
    public class LabPVT
    {    
        public PvtDataRow SurfacePVT { get; set; } = new();
       public PvtDataRow BubblePointPVT { get; set; } = new();
        public PvtDataRow DewPointPVT { get; set; } = new();
   }
}
