using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;

        RelayCommand _navigateCommand;
        public RelayCommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(
            () => _navigationService.NavigateTo(typeof(LevelsPageViewModel).FullName)
                   //p => _navigationService.NavigateTo(typeof(LevelsPageViewModel).FullName)
                   //,
                   // p => !string.IsNullOrEmpty(p)
                   ));

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
