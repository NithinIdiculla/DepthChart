using System.Collections.Generic;
using DepthChart.Common.Models;
using DepthChartLib;
using Xunit;

namespace DepthChartLibTestProject
{
    public class DepthChartFactoryTest
    {
        private ConfigDepthChartData _configChartData;
        public DepthChartFactoryTest()
        {
            _configChartData = new ConfigDepthChartData
            {
                Data = new List<ConfigDepthChart>()
                {
                    new ConfigDepthChart()
                    {
                        Sport = "NFL", Positions = new List<string> {"QB", "WR", "RB", "TE", "K", "P", "KR", "PR"}
                    },
                    new ConfigDepthChart()
                    {
                        Sport = "MLB", Positions = new List<string> { "SP", "RP", "C", "1B", "2B", "3B", "SS", "LF", "RF", "CF", "DH" }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputData))]
        public void Test_DepthChartFactory_InvalidData(ConfigDepthChartData chart, string sport)
        {
            var factory = new DepthChartFactory(chart);

            var depthChartService = factory.GetDepthChartServiceForSport(sport);

            Assert.Null(depthChartService);
            
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_InvalidSport()
        {
            var factory = new DepthChartFactory(_configChartData);

            var depthChartService = factory.GetDepthChartServiceForSport("sport");

            Assert.Null(depthChartService);
        }

        [Fact]
        public void Test_AddPlayerToDepthChart_ValidSport()
        {
            var factory = new DepthChartFactory(_configChartData);

            var depthChartService = factory.GetDepthChartServiceForSport("nfl");

            Assert.NotNull(depthChartService);
        }

        public static IEnumerable<object[]> GetInvalidInputData()
        {
            var allData = new List<object[]>
            {
                new object[] {null, null},
                new object[] {null, ""},
                new object[] {null, "NFL"},
                new object[] { new ConfigDepthChartData(), null},
                new object[] { new ConfigDepthChartData(), ""}

            };

            return allData;
        }
    }
}
