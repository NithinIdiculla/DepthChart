using System;
using System.Collections.Generic;
using System.Linq;
using DepthChart.Common.Models;
using DepthChartLib;
using Xunit;

namespace DepthChartLibTestProject
{
    public class DepthChartManagerUnitTest
    {
        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_AddPlayerToDepthChart_InvalidInput(Player player, string position)
        {
            var depthChartManager = new MlbDepthChartManager("mlb");
            var positionData = depthChartManager.AddPlayerToDepthChart(player, position);

            Assert.Null(positionData);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_NoDepth()
        {
            var player = new Player {Id = 10};
            var position = "TestPosition";
            var defaultDepth = 0;

            var depthChartManager = new MlbDepthChartManager("mlb");
            var positionData = depthChartManager.AddPlayerToDepthChart(player, position);

            Assert.NotNull(positionData);
            Assert.Equal(position, positionData.PositionName);
            Assert.Equal(1, positionData.Players.Count);
            Assert.Equal(player.Id, positionData.Players[0].Id);
            Assert.Equal(defaultDepth, positionData.Players[0].PositionDepth);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_InvalidDepth()
        {
            var player = new Player { Id = 10 };
            var position = "TestPosition";
            var defaultDepth = 0;

            var depthChartManager = new MlbDepthChartManager("mlb");
            var positionData = depthChartManager.AddPlayerToDepthChart(player, position, -3);

            Assert.NotNull(positionData);
            Assert.Equal(position, positionData.PositionName);
            Assert.Equal(1, positionData.Players.Count);
            Assert.Equal(player.Id, positionData.Players[0].Id);
            Assert.Equal(defaultDepth, positionData.Players[0].PositionDepth);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_SamePlayer_SameDepth()
        {
            var player = new Player { Id = 10 };
            var position = "TestPosition";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player, position, 1);
            depthChartManager.AddPlayerToDepthChart(player, position, 1);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(1, depthChartManager.PositionsDepthChart[0].Players.Count);
            Assert.Equal(player.Id, depthChartManager.PositionsDepthChart[0].Players[0].Id);
            Assert.Equal(1, depthChartManager.PositionsDepthChart[0].Players[0].PositionDepth);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_SamePlayer_DifferentDepth()
        {
            var player = new Player { Id = 10 };
            var position = "TestPosition";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player, position);
            depthChartManager.AddPlayerToDepthChart(player, position, 10);
            depthChartManager.AddPlayerToDepthChart(player, position, 20);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(1, depthChartManager.PositionsDepthChart[0].Players.Count);
            Assert.Equal(player.Id, depthChartManager.PositionsDepthChart[0].Players[0].Id);
            Assert.Equal(20, depthChartManager.PositionsDepthChart[0].Players[0].PositionDepth);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_DifferentPlayer_SameDepth()
        {
            var player1 = new Player { Id = 10 };
            var player2 = new Player { Id = 20 };
            var player3 = new Player { Id = 30 };
            var position = "TestPosition";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position, 1);
            depthChartManager.AddPlayerToDepthChart(player2, position, 1);
            depthChartManager.AddPlayerToDepthChart(player3, position, 1);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(3, depthChartManager.PositionsDepthChart[0].Players.Count);

            var depthPlayer1 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 10);
            Assert.Equal(player1.Id, depthPlayer1.Id);
            Assert.Equal(3, depthPlayer1.PositionDepth);

            var depthPlayer2 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 20);
            Assert.Equal(player2.Id, depthPlayer2.Id);
            Assert.Equal(2, depthPlayer2.PositionDepth);

            var depthPlayer3 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 30);
            Assert.Equal(player3.Id, depthPlayer3.Id);
            Assert.Equal(1, depthPlayer3.PositionDepth);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_DifferentPlayer_MixedDepth()
        {
            var player1 = new Player { Id = 10 };
            var player2 = new Player { Id = 20 };
            var player3 = new Player { Id = 30 };
            var player4 = new Player { Id = 40 };
            var position = "TestPosition";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position, 30);
            depthChartManager.AddPlayerToDepthChart(player2, position, 20);
            depthChartManager.AddPlayerToDepthChart(player3, position, 20);
            depthChartManager.AddPlayerToDepthChart(player4, position);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(4, depthChartManager.PositionsDepthChart[0].Players.Count);

            var depthPlayer1 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 10);
            Assert.Equal(player1.Id, depthPlayer1.Id);
            Assert.Equal(30, depthPlayer1.PositionDepth);

            var depthPlayer2 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 20);
            Assert.Equal(player2.Id, depthPlayer2.Id);
            Assert.Equal(21, depthPlayer2.PositionDepth);

            var depthPlayer3 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 30);
            Assert.Equal(player3.Id, depthPlayer3.Id);
            Assert.Equal(20, depthPlayer3.PositionDepth);

            var depthPlayer4 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 40);
            Assert.Equal(player4.Id, depthPlayer4.Id);
            Assert.Equal(31, depthPlayer4.PositionDepth);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_PlayerAddedToLast_WhenNoDepth()
        {
            var player1 = new Player { Id = 10 };
            var player2 = new Player { Id = 20 };
            var player3 = new Player { Id = 30 };
            var position = "TestPosition";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position, 10);
            depthChartManager.AddPlayerToDepthChart(player2, position, 20);
            depthChartManager.AddPlayerToDepthChart(player3, position);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(3, depthChartManager.PositionsDepthChart[0].Players.Count);

            var depthPlayer3 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 30);
            Assert.Equal(player3.Id, depthPlayer3.Id);
            Assert.Equal(21, depthPlayer3.PositionDepth);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_PlayerAddedToDifferentPositions()
        {
            var player1 = new Player { Id = 10 };
            var position1 = "TestPosition1";
            var player2 = new Player { Id = 20 };
            var position2 = "TestPosition2";
            var player3 = new Player { Id = 30 };
            var position3 = "TestPosition3";
            var player4 = new Player { Id = 40 };

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position1, 10);
            depthChartManager.AddPlayerToDepthChart(player2, position2, 20);
            depthChartManager.AddPlayerToDepthChart(player3, position3);
            depthChartManager.AddPlayerToDepthChart(player4, position3);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(3, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(position1, depthChartManager.PositionsDepthChart[0].PositionName);
            Assert.Equal(1, depthChartManager.PositionsDepthChart[0].Players.Count);
            Assert.Equal(position2, depthChartManager.PositionsDepthChart[1].PositionName);
            Assert.Equal(1, depthChartManager.PositionsDepthChart[1].Players.Count);
            Assert.Equal(position3, depthChartManager.PositionsDepthChart[2].PositionName);
            Assert.Equal(2, depthChartManager.PositionsDepthChart[2].Players.Count);
            var depthPlayer3 = depthChartManager.PositionsDepthChart[2].Players.First(x => x.Id == 30);
            Assert.Equal(0, depthPlayer3.PositionDepth);
            var depthPlayer4 = depthChartManager.PositionsDepthChart[2].Players.First(x => x.Id == 40);
            Assert.Equal(1, depthPlayer4.PositionDepth);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_RemovePlayerFromDepthChart_InvalidInput(Player player, string position)
        {
            var depthChartManager = new MlbDepthChartManager("mlb");
            var positionData = depthChartManager.RemovePlayerFromDepthChart(player, position);

            Assert.Null(positionData);
        }

        [Fact]
        public void Test_RemovePlayerFromDepthChart_EmptyDepthChart()
        {
            var depthChartManager = new MlbDepthChartManager("mlb");
            var positionData = depthChartManager.RemovePlayerFromDepthChart(new Player(){Id = 10}, "position");

            Assert.Null(positionData);
        }

        [Fact]
        public void Test_RemovePlayerFromDepthChart_OnePlayer()
        {
            var player1 = new Player { Id = 10 };
            var position1 = "TestPosition1";

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position1, 10);
            depthChartManager.RemovePlayerFromDepthChart(player1, position1);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(0, depthChartManager.PositionsDepthChart[0].Players.Count);
        }

        [Fact]
        public void Test_RemovePlayerFromDepthChart_DoNotChangeExitingPlayerDepthLevel()
        {
            var player1 = new Player { Id = 10 };
            var position1 = "TestPosition1";
            var player2 = new Player { Id = 20 };
            var player3 = new Player { Id = 30 };
            var player4 = new Player { Id = 40 };

            var depthChartManager = new MlbDepthChartManager("mlb");
            depthChartManager.AddPlayerToDepthChart(player1, position1, 10);
            depthChartManager.AddPlayerToDepthChart(player2, position1, 20);
            depthChartManager.AddPlayerToDepthChart(player3, position1);
            depthChartManager.AddPlayerToDepthChart(player4, position1);
            depthChartManager.RemovePlayerFromDepthChart(player2, position1);

            Assert.NotNull(depthChartManager.PositionsDepthChart);
            Assert.Equal(1, depthChartManager.PositionsDepthChart.Count);
            Assert.Equal(3, depthChartManager.PositionsDepthChart[0].Players.Count);

            var depthPlayer1 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 10);
            Assert.Equal(10, depthPlayer1.PositionDepth);
            var depthPlayer2 = depthChartManager.PositionsDepthChart[0].Players.FirstOrDefault(x => x.Id == 20);
            Assert.Null(depthPlayer2);
            var depthPlayer3 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 30);
            Assert.Equal(21, depthPlayer3.PositionDepth);
            var depthPlayer4 = depthChartManager.PositionsDepthChart[0].Players.First(x => x.Id == 40);
            Assert.Equal(22, depthPlayer4.PositionDepth);
        }

        public static IEnumerable<object[]> GetInvalidInputData()
        {
            var allData = new List<object[]>
            {
                new object[] {null, null},
                new object[] {null, "TestPosition"},
                new object[] {new Player {Id = 10}, null}
            };

            return allData;
        }
    }
}
