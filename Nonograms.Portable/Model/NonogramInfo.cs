using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Model
{
    public class NonogramInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("top")]
        public int[][] TopSideValues { get; set; }
        [JsonProperty("left")]
        public int[][] LeftSideValues { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public int RowsNumber
        {
            get
            {
                return LeftSideValues != null ? LeftSideValues.Length : 0;
            }
        }
        public int ColumnsNumber
        {
            get
            {
                return TopSideValues != null ? TopSideValues.Length : 0;
            }
        }
    }
}
