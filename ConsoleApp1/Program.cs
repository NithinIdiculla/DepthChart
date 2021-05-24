using DepthChart.Common.Models;
using DepthChartLib;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DepthCharts
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"Data\", "DepthChartData.json");
            var jsonString = File.ReadAllText(path);
            var configDepthChartData = JsonSerializer.Deserialize<ConfigDepthChartData>(jsonString);

            var depthChartFactory = new DepthChartFactory(configDepthChartData);
            var depthChartService = depthChartFactory.GetDepthChartServiceForSport("NFL");
            if (depthChartService != null)
            {
                depthChartService.AddPlayerToDepthChart(new Player{Id = 1, Name = "TestPlayer10"}, "WR", 0);
                depthChartService.AddPlayerToDepthChart(new Player { Id = 2, Name = "TestPlayer20" }, "WR", 0);
                depthChartService.AddPlayerToDepthChart(new Player { Id = 3, Name = "TestPlayer30" }, "WR", 2);

                depthChartService.AddPlayerToDepthChart(new Player { Id = 1, Name = "TestPlayer30" }, "KR");

                Console.WriteLine();
                Console.WriteLine("Displaying getFullDepthChart");
                Console.WriteLine();

                var fullDepthChart = depthChartService.GetFullDepthChart();
                foreach (var depthChart in fullDepthChart)
                {
                    Console.Write($"{depthChart.PositionName} : ");

                    var players = string.Join(",", depthChart.Players.Select(item => item.Id));
                    
                    Console.Write($"[{players}]");
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("Displaying getPlayersUnderPlayerInDepthChart");
                Console.WriteLine();

                var playersUnderDepthChart =
                    depthChartService.GetPlayersUnderPlayerInDepthChart
                    (new Player {Id = 2, Name = "TestPlayer20"}, "wr");

                var playersBelowDepth = string.Join(",", playersUnderDepthChart.Select(item => item.Id));
                Console.Write($"[{playersBelowDepth}]");
                Console.WriteLine();
                Console.WriteLine();

            }

            Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
    }
}
