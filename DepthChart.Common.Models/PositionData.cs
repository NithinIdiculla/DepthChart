using System;
using System.Collections.Generic;
using System.Text;

namespace DepthChart.Common.Models
{
    public class PositionData
    {
        public string PositionName { get; set; }
        public IList<Player> Players { get; set; }
    }
}
