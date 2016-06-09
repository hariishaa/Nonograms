using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Model.Database
{
    public class NonogramInfo
    {
        public int NonogramId { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
