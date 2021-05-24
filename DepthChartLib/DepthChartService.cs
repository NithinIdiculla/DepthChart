using System;
using System.Collections.Generic;
using System.Linq;
using DepthChart.Common.Models;
using DepthChart.Common.Models.Extensions;
using DepthChartLib.Interface;

namespace DepthChartLib
{
    public class DepthChartService : IDepthChartService
    {
        private readonly ConfigDepthChart _configDepthChart;
        private readonly IDepthChartManager _depthChartManager;


        public DepthChartService(DepthChart.Common.Models.ConfigDepthChart configDepthChart, IDepthChartManager depthChartManager)
        {
            _configDepthChart = configDepthChart;
            _depthChartManager = depthChartManager;
        }

        public void AddPlayerToDepthChart(Player player, string position, int depth = -1)
        {
            if(player == null || string.IsNullOrEmpty(position) || !_configDepthChart.IsValidPosition(position))
                throw new ArgumentException("Input arguments are invalid");

            _depthChartManager.AddPlayerToDepthChart(player, position, depth);
        }

        public void RemovePlayerFromDepthChart(Player player, string position)
        {
            if (player == null || string.IsNullOrEmpty(position) || !_configDepthChart.IsValidPosition(position))
                throw new ArgumentException("Input arguments are invalid");

            _depthChartManager.RemovePlayerFromDepthChart(player, position);
        }

        public IList<Player> GetPlayersUnderPlayerInDepthChart(Player player, string position)
        {
            if (player == null || string.IsNullOrEmpty(position) || !_configDepthChart.IsValidPosition(position))
                throw new ArgumentException("Input arguments are invalid");

            var existingPosition = _depthChartManager.PositionsDepthChart?
                .FirstOrDefault(x => x.PositionName.ToLower().Equals(position.ToLower()));

            var selectedPlayerPosition = existingPosition?.Players.FirstOrDefault(x => x.Id == player.Id);
            if (selectedPlayerPosition != null)
            {
                return existingPosition.Players
                    .Where(x => x.PositionDepth > selectedPlayerPosition.PositionDepth)
                    .OrderBy(x => x.PositionDepth)
                    .ToList();
            }

            return default;
        }

        public IList<PositionData> GetFullDepthChart()
        {
            _depthChartManager.PositionsDepthChart?.ToList().ForEach(x => x.Players = x.Players.OrderBy(p => p.PositionDepth).ToList());
            return _depthChartManager.PositionsDepthChart?.ToList();
        }
    }
}
