using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Nonograms.Portable.ViewModel;
using Nonograms.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.ViewModel
{
    public class ViewModelLocator : BaseViewModelLocator
    {
        public const string NonogramPageKey = "NonogramPage";

        public ViewModelLocator() : base()
        {
            //SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
                //SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                var navigationService = new NavigationService();
                navigationService.Configure(NonogramPageKey, typeof(NonogramPage));
                SimpleIoc.Default.Register<INavigationService>(() => navigationService);
                //SimpleIoc.Default.Register<IDataService, DataService>();
            }
        }
    }
}
