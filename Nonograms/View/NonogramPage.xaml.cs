using Nonograms.Portable;
using Nonograms.Portable.Model;
using Nonograms.Portable.Model.DTO;
using Nonograms.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Nonograms.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NonogramPage : Page
    {
        public NonogramPage()
        {
            this.InitializeComponent();
            this.Loading += NonogramPage_Loading;
            Window.Current.VisibilityChanged += Current_VisibilityChanged;
        }

        // !!!
        private void Current_VisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            //var vm = DataContext as Portable.ViewModel.BaseNonogramPageViewModel;
            //vm.SaveHistory();
        }

        private void NonogramPage_Loading(FrameworkElement sender, object args)
        {
            //var vm = DataContext as Portable.ViewModel.NonogramPageViewModel;
            //vm.LoadNonogram((NonogramInfo)e.Parameter);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // зачем???
            base.OnNavigatedTo(e);

            var vm = DataContext as NonogramPageViewModel;
            vm.LoadNonogram((NonogramInfo)e.Parameter);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            var vm = DataContext as NonogramPageViewModel;
            vm.SaveHistory();
        }

        //удалить
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
            var b = Window.Current.Bounds;
        }

        private void Nonogram_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
        }

        private void Nonogram_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
        }
    }
}
