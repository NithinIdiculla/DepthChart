using System;
using System.Collections.Generic;
using System.Text;
using DepthChart.Common.Models;

namespace DepthChartLib.Interface
{
    public interface IDepthChartFactory
    {
        IDepthChartService GetDepthChartServiceForSport(string sportName);
    }
}
