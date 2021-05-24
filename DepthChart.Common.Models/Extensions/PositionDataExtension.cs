using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthChart.Common.Models.Extensions
{
    public static class PositionDataExtension
    {
        public static void Update(this IList<PositionData> lstPositionData, PositionData positionData)
        {
            var indexToReplace = lstPositionData.ToList()
                .FindIndex(x => x.PositionName.ToLower().Equals(positionData.PositionName.ToLower()));

            if(indexToReplace == -1)
                lstPositionData.Add(positionData);
            else
                lstPositionData[indexToReplace] = positionData;
        }
    }
}
