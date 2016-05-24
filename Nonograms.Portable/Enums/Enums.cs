using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Enums
{
    public enum CellStates { Empty = 0, Checked = 1, Tagged = -1 }
    public enum CheckModes { Check = 1, Tag = -1 }
    public enum TagTypes { Dot, X }
}
