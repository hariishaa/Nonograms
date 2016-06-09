using Newtonsoft.Json;
using Nonograms.Portable.Model.DTO;
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
        string _manifestResourcePath = "Nonograms.Portable.Data.AllNonograms.json";
        public async Task<IEnumerable<NonogramInfo>> GetAllNonogramsInfo()
        {
            return await ReadJson(_manifestResourcePath);
        }

        //public NonogramInfo GetNonogramInfo(int id)
        //{
        //    return ReadJson().Where(info => info.Id == id).FirstOrDefault();
        //}

        // file handling using embedded resource
        private async Task<IEnumerable<NonogramInfo>> ReadJson(string manifestResourcePath)
        {
            var assembly = typeof(NonogramInfo).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(manifestResourcePath);
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<IEnumerable<NonogramInfo>>(json);
            }
        }
    }
}
