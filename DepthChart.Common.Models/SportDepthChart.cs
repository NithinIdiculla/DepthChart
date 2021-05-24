using System;
using System.Collections.Generic;
using System.Text;

namespace DepthChart.Common.Models
{
    public class SportDepthChart
    {
        public string Sport { get; set; }
        public IList<PositionData> PositionData { get; set; }
    }
}
