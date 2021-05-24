using System.Collections.Generic;
using System.Linq;
using DepthChart.Common.Models;
using DepthChartLib.Interface;

namespace DepthChartLib
{
    public abstract class DepthChartManager : IDepthChartManager
    {
        protected IList<PositionData> PositionsData;

        public abstract IList<PositionData> PositionsDepthChart { get; }
        public abstract string Sport { get; }

        public virtual PositionData AddPlayerToDepthChart(Player player, string position, int depth = -1)
        {
            if (player == null || string.IsNullOrEmpty(position)) return null;

            //First record
            if (PositionsData == null || PositionsData.Count == 0)
            {
                var newPositionData = AddPositionData(player, position, depth > -1 ? depth : 0);
                PositionsData = new List<PositionData>
                {
                    newPositionData
                };

                return newPositionData;
            }

            //First record for a position
            var positionData = PositionsData.FirstOrDefault(x => x.PositionName.ToLower().Equals(position.ToLower()));
            if (positionData == null)
            {
                var newPositionData = AddPositionData(player, position, depth > -1 ? depth : 0);

                PositionsData.Add(newPositionData);

                return newPositionData;
            }

            if (depth <= -1)
            {
                //Add player to the end of the chart
                AddPlayerInPosition(positionData, player, GetMaxDepthPositionInChart(position) + 1);
            }
            else
            {
                var existingPlayer = positionData.Players.FirstOrDefault(x => x.Id == player.Id);
                if (existingPlayer != null)
                {
                    existingPlayer.PositionDepth = depth;
                }
                else
                {
                    AddPlayerInPosition(positionData, player, depth);
                }

                //Push down the overlapping position of all players under the newly added/modified player
                var lstPlayersAboveInputDepth = positionData.Players.OrderBy(x => x.PositionDepth).Where(x => x.PositionDepth >= depth && x.Id != player.Id).ToList();
                var currentDepth = depth;
                foreach (var playerAbove in lstPlayersAboveInputDepth.Where(playerAbove => playerAbove.PositionDepth - currentDepth == 0))
                {
                    playerAbove.PositionDepth += 1;
                    currentDepth = playerAbove.PositionDepth;
                }
            }

            return positionData;
        }

        public virtual PositionData RemovePlayerFromDepthChart(Player player, string position)
        {
            if (player == null || string.IsNullOrEmpty(position)) return null;

            var positionData = PositionsData?.FirstOrDefault(x => x.PositionName.ToLower().Equals(position.ToLower()));
            if (positionData != null)
            {
                var playerIndex = positionData.Players.ToList().FindIndex(x => x.Id == player.Id);
                positionData.Players.RemoveAt(playerIndex);
            }

            return positionData;
        }

        private PositionData AddPositionData(Player player, string position, int depth)
        {
            return new PositionData
            {
                PositionName = position,
                Players = new List<Player>()
                {
                    new Player
                    {
                        Id = player.Id,
                        Name = player.Name,
                        PositionDepth = depth
                    }
                }
            };
        }

        private void AddPlayerInPosition(PositionData positionData, Player player, int depth)
        {
            positionData.Players.Add(
                new Player
                {
                    Id = player.Id,
                    Name = player.Name,
                    PositionDepth = depth
                });
        }

        private int GetMaxDepthPositionInChart(string position)
        {
            var positionData = PositionsData?.FirstOrDefault(x => x.PositionName.ToLower().Equals(position.ToLower()));
            var lastDepthPlayer = positionData?.Players.OrderByDescending(x => x.PositionDepth).FirstOrDefault();
            return lastDepthPlayer?.PositionDepth ?? 0;
        }
    }
}
