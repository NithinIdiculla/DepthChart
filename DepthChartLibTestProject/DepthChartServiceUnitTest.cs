using System;
using System.Collections.Generic;
using System.Text;
using DepthChart.Common.Models;
using DepthChartLib;
using DepthChartLib.Interface;
using Moq;
using Xunit;

namespace DepthChartLibTestProject
{
    public class DepthChartServiceUnitTest
    {
        private readonly ConfigDepthChart _configDepthChart;
        private readonly IDepthChartManager _depthChartManager;
        private readonly Mock<IDepthChartManager> _mockDepthChartManager;
        public DepthChartServiceUnitTest()
        {
            _configDepthChart = new ConfigDepthChart
            {
                Sport = "NFL", Positions = new List<string> { "QB", "WR", "RB", "TE", "K", "P", "KR", "PR" },

            };

            _depthChartManager = new NflDepthChartManager("NFL");
            _mockDepthChartManager = new Mock<IDepthChartManager>();
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_AddPlayerToDepthChart_InvalidInput(Player player, string position)
        {
            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            var ex = Assert.Throws<ArgumentException>(() => depthChartService.AddPlayerToDepthChart(player, position)) ;

            Assert.Equal("Input arguments are invalid", ex.Message);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_CallsManager()
        {
            var player = new Player() {Id = 10};
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _mockDepthChartManager.Object);
            depthChartService.AddPlayerToDepthChart(player, position);

            _mockDepthChartManager.Verify(x => x.AddPlayerToDepthChart(player, position, -1), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_RemovePlayerFromDepthChart_InvalidInput(Player player, string position)
        {
            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            var ex = Assert.Throws<ArgumentException>(() => depthChartService.RemovePlayerFromDepthChart(player, position));

            Assert.Equal("Input arguments are invalid", ex.Message);
        }

        [Fact]
        public void Test_RemovePlayerFromDepthChart_CallsManager()
        {
            var player = new Player() { Id = 10 };
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _mockDepthChartManager.Object);
            depthChartService.RemovePlayerFromDepthChart(player, position);

            _mockDepthChartManager.Verify(x => x.RemovePlayerFromDepthChart(player, position), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_GetPlayersUnderPlayerInDepthChart_InvalidInputs(Player player, string position)
        {
            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            var ex = Assert.Throws<ArgumentException>(() => depthChartService.GetPlayersUnderPlayerInDepthChart(player, position));

            Assert.Equal("Input arguments are invalid", ex.Message);
        }

        [Fact]
        public void Test_GetPlayersUnderPlayerInDepthChart_WithNoPlayersAdded()
        {
            var player = new Player() { Id = 10 };
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            var players = depthChartService.GetPlayersUnderPlayerInDepthChart(player, position);

            Assert.Null(players);

        }

        [Fact]
        public void Test_GetPlayersUnderPlayerInDepthChart_WithPlayerNotAdded()
        {
            var player1 = new Player() { Id = 10 };
            var player2 = new Player() { Id = 20 };
            var player3 = new Player() { Id = 30 };
            var player4 = new Player() { Id = 40 };
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            depthChartService.AddPlayerToDepthChart(player1, position);
            depthChartService.AddPlayerToDepthChart(player2, position, 10);
            depthChartService.AddPlayerToDepthChart(player3, position, 20);

            var players = depthChartService.GetPlayersUnderPlayerInDepthChart(player4, position);

            Assert.Null(players);
        }

        [Fact]
        public void Test_GetPlayersUnderPlayerInDepthChart_WithValidData()
        {
            var player1 = new Player() { Id = 10 };
            var player2 = new Player() { Id = 20 };
            var player3 = new Player() { Id = 30 };
            var player4 = new Player() { Id = 40 };
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            depthChartService.AddPlayerToDepthChart(player1, position, 30);
            depthChartService.AddPlayerToDepthChart(player2, position, 10);
            depthChartService.AddPlayerToDepthChart(player3, position, 20);
            depthChartService.AddPlayerToDepthChart(player4, position, 20);

            var players = depthChartService.GetPlayersUnderPlayerInDepthChart(player2, position);

            Assert.NotNull(players);
            Assert.Equal(3, players.Count);
            Assert.Equal(20, players[0].PositionDepth);
            Assert.Equal(21, players[1].PositionDepth);
            Assert.Equal(30, players[2].PositionDepth);
        }

        [Fact]
        public void Test_GetFullDepthChart_WithNoPlayersAdded()
        {
            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            var players = depthChartService.GetFullDepthChart();

            Assert.Null(players);
        }

        [Fact]
        public void Test_GetFullDepthChart_ReturnsTheCorrectSequence()
        {
            var player1 = new Player() { Id = 10 };
            var player2 = new Player() { Id = 20 };
            var player3 = new Player() { Id = 30 };
            var player4 = new Player() { Id = 40 };
            var position = "QB";

            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            depthChartService.AddPlayerToDepthChart(player1, position, 30);
            depthChartService.AddPlayerToDepthChart(player2, position, 10);
            depthChartService.AddPlayerToDepthChart(player3, position, 20);
            depthChartService.AddPlayerToDepthChart(player4, position, 20);

            var positionData = depthChartService.GetFullDepthChart();

            Assert.NotNull(positionData);
            Assert.Equal(1, positionData.Count);
            Assert.Equal(10, positionData[0].Players[0].PositionDepth);
            Assert.Equal(player2.Id, positionData[0].Players[0].Id);
            Assert.Equal(20, positionData[0].Players[1].PositionDepth);
            Assert.Equal(player4.Id, positionData[0].Players[1].Id);
            Assert.Equal(21, positionData[0].Players[2].PositionDepth);
            Assert.Equal(player3.Id, positionData[0].Players[2].Id);
            Assert.Equal(30, positionData[0].Players[3].PositionDepth);
            Assert.Equal(player1.Id, positionData[0].Players[3].Id);
        }

        [Fact]
        public void Test_GetFullDepthChart_MultiplePositions()
        {
            var player1 = new Player() { Id = 10 };
            var player2 = new Player() { Id = 20 };
            var player3 = new Player() { Id = 30 };
            var player4 = new Player() { Id = 40 };
            var position1 = "QB";
            var position2 = "TE";

            var depthChartService = new DepthChartService(_configDepthChart, _depthChartManager);
            depthChartService.AddPlayerToDepthChart(player1, position1, 30);
            depthChartService.AddPlayerToDepthChart(player2, position1, 10);
            depthChartService.AddPlayerToDepthChart(player3, position2, 20);
            depthChartService.AddPlayerToDepthChart(player4, position2, 20);

            var positionData = depthChartService.GetFullDepthChart();

            Assert.NotNull(positionData);
            Assert.Equal(2, positionData.Count);
            Assert.Equal(2, positionData[0].Players.Count);
            Assert.Equal(position1, positionData[0].PositionName);
            Assert.Equal(2, positionData[1].Players.Count);
            Assert.Equal(position2, positionData[1].PositionName);
        }

        public static IEnumerable<object[]> GetInvalidInputData()
        {
            var allData = new List<object[]>
            {
                new object[] {null, null},
                new object[] {null, "TestPosition"},
                new object[] {new Player {Id = 10}, null},
                new object[] {new Player {Id = 10}, "TestPosition"},
            };

            return allData;
        }
    }
}
