using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Nonograms.Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;

        // переименовать
        RelayCommand _navigateCommand;
        public RelayCommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(
            () => _navigationService.NavigateTo(typeof(LevelsPageViewModel).FullName)
                   //p => _navigationService.NavigateTo(typeof(LevelsPageViewModel).FullName)
                   //,
                   // p => !string.IsNullOrEmpty(p)
                   ));

        // переименовать
        RelayCommand _navigateCommand2;
        public RelayCommand NavigateCommand2 => _navigateCommand2 ?? (_navigateCommand2 = new RelayCommand(
            () => _navigationService.NavigateTo(typeof(TutorialPageViewModel).FullName)));

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }




        ObservableCollection<NonogramInfo> _allNonograms;
        public ObservableCollection<NonogramInfo> AllNonograms
        {
            get
            {
                return _allNonograms;
            }
            set
            {
                _allNonograms = value;
                RaisePropertyChanged();
            }
        }

        public void LoadNonograms()
        {
            //NonogramsRepository repo = new NonogramsRepository();
            //AllNonograms = new ObservableCollection<NonogramInfo>(repo.GetAllNonogramsInfo());
        }
    }
}
