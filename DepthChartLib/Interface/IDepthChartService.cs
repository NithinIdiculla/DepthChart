using System;
using System.Collections.Generic;
using System.Text;
using DepthChart.Common.Models;

namespace DepthChartLib.Interface
{
    public interface IDepthChartService
    {
        void AddPlayerToDepthChart(Player player, string position, int depth = -1);
        void RemovePlayerFromDepthChart(Player player, string position);
        IList<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position);
        IList<PositionData> GetFullDepthChart();
    }
}
