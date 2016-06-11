using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Model.Database
{
    public class NonogramInfo
    {
        [Key]
        public int NonogramId { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
