using DepthChart.Common.Models;
using System.Collections.Generic;

namespace DepthChartLib
{
    public class NflDepthChartManager : DepthChartManager
    {
        public NflDepthChartManager(string sport)
        {
            Sport = sport;
        }
        public override IList<PositionData> PositionsDepthChart => PositionsData;

        public override string Sport { get; }
    }
}
