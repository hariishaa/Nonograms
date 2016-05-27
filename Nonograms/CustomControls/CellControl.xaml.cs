using Nonograms.Portable.Enums;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Nonograms.CustomControls
{
    public sealed partial class CellControl : UserControl
    {

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(CellStates), typeof(CellControl), new PropertyMetadata(default(CellStates), OnStatePropertyChanged));

        private static void OnStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CellControl nc = d as CellControl;
            switch ((CellStates)e.NewValue)
            {
                case CellStates.Checked:
                    nc.CellBorder.Background = new SolidColorBrush(Colors.Black);
                    nc.TagGrid.Visibility = Visibility.Collapsed;
                    break;
                case CellStates.Tagged:
                    nc.CellBorder.Background = new SolidColorBrush(Colors.White);
                    nc.TagGrid.Visibility = Visibility.Visible;
                    break;
                case CellStates.Empty:
                default:
                    nc.CellBorder.Background = new SolidColorBrush(Colors.White);
                    nc.TagGrid.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public CellStates State
        {
            get
            {
                return (CellStates)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        public static readonly DependencyProperty TagTypeProperty = DependencyProperty.Register("TagType", typeof(TagTypes), typeof(CellControl), new PropertyMetadata(default(TagTypes), OnTagTypePropertyChanged));

        private static void OnTagTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CellControl nc = d as CellControl;
            switch ((TagTypes)e.NewValue)
            {
                case TagTypes.X:
                    nc.Dot.Visibility = Visibility.Collapsed;
                    nc.X.Visibility = Visibility.Visible;
                    break;
                case TagTypes.Dot:
                default:
                    nc.Dot.Visibility = Visibility.Visible;
                    nc.X.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public TagTypes TagType
        {
            get
            {
                return (TagTypes)GetValue(TagTypeProperty);
            }
            set
            {
                SetValue(TagTypeProperty, value);
            }
        }

        public CellControl()
        {
            this.InitializeComponent();
            // установка стандартных значений
            State = CellStates.Empty;
            TagType = TagTypes.Dot;
        }
    }
}
