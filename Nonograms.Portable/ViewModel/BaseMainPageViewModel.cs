using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Nonograms.Portable.Model;
using Nonograms.Portable.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public abstract class BaseMainPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;

        bool _isSettingsOpened;
        public bool IsSettingsOpened
        {
            get
            {
                return _isSettingsOpened;
            }
            set
            {
                _isSettingsOpened = value;
                RaisePropertyChanged();
            }
        }

        bool? _isFullScreenModeEnabled;
        public bool? IsFullScreenModeEnabled
        {
            get
            {
                return _isFullScreenModeEnabled;
            }
            set
            {
                if (value == null)
                {
                    value = true;
                }
                _isFullScreenModeEnabled = value;
                RaisePropertyChanged();
            }
        }

        bool? _areTipsEnabled;
        public bool? AreTipsEnabled
        {
            get
            {
                return _areTipsEnabled;
            }
            set
            {
                if (value == null)
                {
                    value = true;
                }
                _areTipsEnabled = value;
                RaisePropertyChanged();
            }
        }

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

        protected RelayCommand _openSettingsCommand;
        public abstract RelayCommand OpenSettingsCommand { get; }

        protected RelayCommand _closeSettingsCommand;
        public abstract RelayCommand CloseSettingsCommand { get; }

        public BaseMainPageViewModel(INavigationService navigationService)
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
