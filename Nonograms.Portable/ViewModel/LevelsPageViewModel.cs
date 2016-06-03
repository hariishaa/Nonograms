using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Nonograms.Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public class LevelsPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;

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

        RelayCommand<NonogramInfo> _navigateCommand;
        public RelayCommand<NonogramInfo> NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand<NonogramInfo>(
                    p => _navigationService.NavigateTo(typeof(NonogramPageViewModel).FullName, p)
                    //,
                   // p => !string.IsNullOrEmpty(p)
                   ));

        // как это работает?
        public LevelsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void LoadNonograms()
        {
            NonogramsRepository repo = new NonogramsRepository();
            AllNonograms = new ObservableCollection<NonogramInfo>(repo.GetAllNonogramsInfo());
        }
    }
}
