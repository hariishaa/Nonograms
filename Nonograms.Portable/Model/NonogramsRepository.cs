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
        public IEnumerable<NonogramInfo> GetAllNonogramsInfo()
        {
            return ReadJson();
        }

        //public NonogramInfo GetNonogramInfo(int id)
        //{
        //    return ReadJson().Where(info => info.Id == id).FirstOrDefault();
        //}

        // file handling using embedded resource
        private IEnumerable<NonogramInfo> ReadJson()
        {
            var assembly = typeof(NonogramInfo).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Nonograms.Portable.Data.AllNonograms.json");
            using (var sr = new StreamReader(stream))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<NonogramInfo>>(json);
            }
        }
    }
}
