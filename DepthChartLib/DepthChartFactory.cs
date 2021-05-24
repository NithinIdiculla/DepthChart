using DepthChart.Common.Models;
using System.Linq;
using DepthChartLib.Interface;

namespace DepthChartLib
{
    public class DepthChartFactory : IDepthChartFactory
    {
        private readonly ConfigDepthChartData _depthChartData;

        public DepthChartFactory(ConfigDepthChartData depthChartData) => _depthChartData = depthChartData;

        public IDepthChartService GetDepthChartServiceForSport(string sportName)
        {
            if (string.IsNullOrEmpty(sportName) || _depthChartData == null)
                return null;

            var depthChartData = _depthChartData.Data
                .FirstOrDefault(x => x.Sport.Trim().ToLower().Equals(sportName.Trim().ToLower()));

            if (depthChartData == null) return null;
            return (sportName.ToUpper()) switch
            {
                "NFL" => new DepthChartService(depthChartData, new NflDepthChartManager("NFL")),
                "MLB" => new DepthChartService(depthChartData, new MlbDepthChartManager("MLB")),
                _ => null,
            };
        }
    }
}
