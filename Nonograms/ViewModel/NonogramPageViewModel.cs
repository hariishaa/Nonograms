using Newtonsoft.Json;
using Nonograms.Portable.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Nonograms.ViewModel
{
    public class NonogramPageViewModel : BaseNonogramPageViewModel
    {
        protected override void LoadSettings()
        {
            var settings = ApplicationData.Current.RoamingSettings;
            AreTipsEnabled = (bool?)settings.Values["AreTipsEnabled"];
        }

        protected override List<int[,]> LoadHistory()
        {
            try
            {
                // шляпа
                var filename = _nonogramInfo.Id.ToString() + ".json";
                StorageFile file = ApplicationData.Current.RoamingFolder.GetFileAsync(filename).AsTask().Result;
                string json  = FileIO.ReadTextAsync(file).AsTask().Result;
                return JsonConvert.DeserializeObject<List<int[,]>>(json);
            }
            catch
            {
                return null;
            }
        }

        public override async void SaveHistory()
        {
            var filename = _nonogramInfo.Id.ToString() + ".json";
            string json = JsonConvert.SerializeObject(FieldHistory);
            StorageFile file = await ApplicationData.Current.RoamingFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }
    }
}
