using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DepthChart.Common.Models.Extensions
{
    public static class ConfigDepthChartExtension
    {
        public static bool IsValidPosition(this ConfigDepthChart depthChart, string position)
        {
            return depthChart.Positions.Any(x => x.Trim().ToLower().Equals(position.Trim().ToLower()));
        }
    }
}
