using Microsoft.EntityFrameworkCore;
using Nonograms.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Nonograms.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            combo.SelectedIndex = settings.Values["TagType"] != null ? (int)settings.Values["TagType"] : 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var vm = DataContext as MainPageViewModel;
            vm.LoadNonograms();

            // не работает!!!
            //using (var db = new DatabaseContext())
            //{
            //    var x = db.Database.EnsureCreated();

            //    //db.Database.Migrate();
            //    //db.NonogramsInfo.Add(new NonogramInfo { Name = "bob", NonogramId = 1, StateId = 1 });
            //    //db.SaveChanges();
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutGrid.Visibility = AboutGrid.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            settings.Values["TagType"] = combo.SelectedIndex;
        }
    }
}
