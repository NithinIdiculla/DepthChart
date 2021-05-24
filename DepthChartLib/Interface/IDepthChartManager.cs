using System;
using System.Collections.Generic;
using System.Text;
using DepthChart.Common.Models;

namespace DepthChartLib.Interface
{
    public interface IDepthChartManager
    {
        string Sport { get; }
        IList<PositionData> PositionsDepthChart { get; }
        PositionData AddPlayerToDepthChart(Player player, string position, int depth = -1);
        PositionData RemovePlayerFromDepthChart(Player player, string position);
    }
}
