using Nonograms.Portable.Model;
using Nonograms.Portable.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Nonograms.View.Converters
{
    public sealed class ItemClickEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as ItemClickEventArgs;
            if (args == null)
                throw new ArgumentException("Value is not ItemClickEventArgs");
            if (args.ClickedItem is NonogramInfo)
            {
                return args.ClickedItem as NonogramInfo;         
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
