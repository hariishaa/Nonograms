using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public abstract class BaseViewModelLocator
    {
        // доделать
        public BaseViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //SimpleIoc.Default.Register<TutorialPageViewModel>();
            SimpleIoc.Default.Register<LevelsPageViewModel>();

            RegisterViewModels();
        }

        public abstract BaseMainPageViewModel MainPage { get; }
        //public TutorialPageViewModel TutorialPage => ServiceLocator.Current.GetInstance<TutorialPageViewModel>();
        public LevelsPageViewModel LevelsPage => ServiceLocator.Current.GetInstance<LevelsPageViewModel>();
        public abstract BaseNonogramPageViewModel NonogramPage { get; }

        public abstract void RegisterViewModels();
    }
}
