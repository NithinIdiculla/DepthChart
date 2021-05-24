using DepthChart.Common.Models;
using System.Collections.Generic;

namespace DepthChartLib
{
    public class MlbDepthChartManager : DepthChartManager
    {
        public MlbDepthChartManager(string sport)
        {
            Sport = sport;
        }

        public override string Sport { get; }

        public override IList<PositionData> PositionsDepthChart => PositionsData;
    }
}
