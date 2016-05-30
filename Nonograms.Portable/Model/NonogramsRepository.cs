using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Model
{
    public class NonogramsRepository
    {
        public NonogramInfo GetNonogramInfo()
        {
            return ConvertFromJson();
        }

        // file handling using embedded resource
        private NonogramInfo ConvertFromJson()
        {
            var assembly = typeof(NonogramInfo).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Nonograms.Portable.Data.Nonogram15x15.json");
            using (var sr = new StreamReader(stream))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<NonogramInfo>(json);
            }
        }
    }
}
