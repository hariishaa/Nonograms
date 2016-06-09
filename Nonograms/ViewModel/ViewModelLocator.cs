using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
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
        //public const string NonogramPageKey = "NonogramPage";

        public ViewModelLocator() : base()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            var navigationService = new NavigationService();
            navigationService.Configure(typeof(TutorialPageViewModel).FullName, typeof(TutorialPage));
            navigationService.Configure(typeof(LevelsPageViewModel).FullName, typeof(LevelsPage));
            navigationService.Configure(typeof(NonogramPageViewModel).FullName, typeof(NonogramPage));
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            //SimpleIoc.Default.Register<IDataService, DataService>();
        }

        public override BaseMainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        public override void RegisterViewModels()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
        }
    }
}
