using Nonograms.Portable.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.Storage;
using Microsoft.Practices.ServiceLocation;

namespace Nonograms.ViewModel
{
    public class MainPageViewModel : BaseMainPageViewModel
    {
        ApplicationDataContainer _settings;

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _settings = ApplicationData.Current.RoamingSettings;
            ApplicationData.Current.DataChanged += Current_DataChanged;
        }

        private async void Current_DataChanged(ApplicationData sender, object args)
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
            await dialog.ShowMessage("You won!", "Victory!!!");
        }

        public override RelayCommand OpenSettingsCommand => _openSettingsCommand ?? (_openSettingsCommand =
            new RelayCommand(() =>
            {
                IsFullScreenModeEnabled = (bool?)_settings.Values["IsFullScreenModeEnabled"];
                IsSettingsOpened = true;
            }));

        public override RelayCommand CloseSettingsCommand => _closeSettingsCommand ?? (_closeSettingsCommand =
            new RelayCommand(() =>
            {
                IsSettingsOpened = false;
                _settings.Values["IsFullScreenModeEnabled"] = IsFullScreenModeEnabled;
            }));
    }
}
