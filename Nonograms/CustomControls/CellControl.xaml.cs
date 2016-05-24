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

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(CellStates), typeof(CellControl), new PropertyMetadata(default(CellStates)));
        public CellStates State
        {
            get
            {
                return (CellStates)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
                // ИЗМЕНИТЬ по аналогии с TagType
                switch (value)
                {
                    case CellStates.Checked:
                        CellBorder.Background = new SolidColorBrush(Colors.Black);
                        TagGrid.Visibility = Visibility.Collapsed;
                        break;
                    case CellStates.Tagged:
                        CellBorder.Background = new SolidColorBrush(Colors.White);
                        TagGrid.Visibility = Visibility.Visible;
                        break;
                    case CellStates.Empty:
                    default:
                        CellBorder.Background = new SolidColorBrush(Colors.White);
                        TagGrid.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        public static readonly DependencyProperty TagTypeProperty = DependencyProperty.Register("TagType", typeof(TagTypes), typeof(CellControl), new PropertyMetadata(default(TagTypes), OnTagTypePropertyChanged));

        private static void OnTagTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CellControl nc = d as CellControl;
            nc.OnTagTypePropertyChanged(e);
        }

        private void OnTagTypePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            switch ((TagTypes)e.NewValue)
            {
                case TagTypes.X:
                    Dot.Visibility = Visibility.Collapsed;
                    X.Visibility = Visibility.Visible;
                    break;
                case TagTypes.Dot:
                default:
                    Dot.Visibility = Visibility.Visible;
                    X.Visibility = Visibility.Collapsed;
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
