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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Nonograms.CustomControls
{
    public sealed partial class NonogramControl : UserControl
    {
        bool _pointerCaptured;
        Rectangle _outlineRectangle;
        Point _beginPoint, _endPoint;
        CheckModes _checkMode;
        #region CheckModesProperty
        public static readonly DependencyProperty CheckModesProperty = DependencyProperty.Register("CheckMode", typeof(CheckModes), typeof(NonogramControl), new PropertyMetadata(default(CheckModes)));
        public CheckModes CheckMode
        {
            get
            {
                return (CheckModes)GetValue(CheckModesProperty);
            }
            set
            {
                SetValue(CheckModesProperty, value);
            }
        }
        #endregion
        #region TagTypeProperty
        public static readonly DependencyProperty TagTypeProperty = DependencyProperty.Register("TagType", typeof(TagTypes), typeof(NonogramControl), new PropertyMetadata(default(TagTypes)));
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
        #endregion
        #region LeftSideValuesProperty
        public static readonly DependencyProperty LeftSideValuesProperty = DependencyProperty.Register("LeftSideValues", typeof(int[][]), typeof(NonogramControl), new PropertyMetadata(default(int[][]), OnLeftSideValuesPropertyChanged));
        private static void OnLeftSideValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NonogramControl nc = d as NonogramControl;
            nc.OnLeftSideValuesPropertyChanged(e);
        }
        private void OnLeftSideValuesPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            BuildLeftSide((int[][])e.NewValue);
        }
        public int[][] LeftSideValues
        {
            get
            {
                return (int[][])GetValue(LeftSideValuesProperty);
            }
            set
            {
                SetValue(LeftSideValuesProperty, value);
            }
        }
        #endregion
        #region TopSideValuesProperty
        public static readonly DependencyProperty TopSideValuesProperty = DependencyProperty.Register("TopSideValues", typeof(int[][]), typeof(NonogramControl), new PropertyMetadata(default(int[][]), OnTopSideValuesPropertyChanged));
        private static void OnTopSideValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NonogramControl nc = d as NonogramControl;
            nc.OnTopSideValuesPropertyChanged(e);
        }
        private void OnTopSideValuesPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            BuildTopSide((int[][])e.NewValue);
        }
        public int[][] TopSideValues
        {
            get
            {
                return (int[][])GetValue(TopSideValuesProperty);
            }
            set
            {
                SetValue(TopSideValuesProperty, value);
            }
        }
        #endregion
        #region FieldProperty
        public static readonly DependencyProperty FieldProperty = DependencyProperty.Register("Field", typeof(int[,]), typeof(NonogramControl), new PropertyMetadata(default(int[,]), OnFieldPropertyChanged));
        private static void OnFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NonogramControl nc = d as NonogramControl;
            nc.OnFieldPropertyChanged(e);
        }
        private void OnFieldPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            //BindableField = ConvertFieldToBindable(Field);

            // фигня?
            //int[,] oldField = ((int[,])e.OldValue);
            //int[,] newField = ((int[,])e.NewValue);
            //// чтобы построить FieldGrid один раз - при первом связывании свойства Field или при изменении его размеров
            //if (oldField == null || oldField.Length != newField.Length)
            //{
            //    BuildFieldGrid(newField.GetLength(0), newField.GetLength(1));
            //}

            BuildFieldGrid(((int[,])e.NewValue).GetLength(0), ((int[,])e.NewValue).GetLength(1));
        }
        public int[,] Field
        {
            get
            {
                return (int[,])GetValue(FieldProperty);
            }
            set
            {
                SetValue(FieldProperty, value);
                //ConvertFieldToBindable(Field);
            }
        }
        //private static readonly DependencyProperty BindableFieldProperty = DependencyProperty.Register("BindableField", typeof(int[][]), typeof(NonogramControl), new PropertyMetadata(default(int[][])));
        //private int[][] BindableField
        //{
        //    get
        //    {
        //        return (int[][])GetValue(BindableFieldProperty);
        //    }
        //    set
        //    {
        //        SetValue(BindableFieldProperty, value);
        //    }
        //}
        //private int[][] ConvertFieldToBindable(int[,] field)
        //{
        //    int rows = field.GetLength(0), columns = field.GetLength(1);
        //    int[][] bindableField = new int[rows][];
        //    for (int i = 0; i < rows; i++)
        //    {
        //        bindableField[i] = new int[columns];
        //        for (int j = 0; j < columns; j++)
        //        {
        //            bindableField[i][j] = field[i, j];
        //        }
        //    }
        //    return bindableField;
        //}

        #endregion
        #region IsSolvedProperty
        public static readonly DependencyProperty IsSolvedProperty = DependencyProperty.Register("IsSolved", typeof(Boolean), typeof(NonogramControl), new PropertyMetadata(default(Boolean)));
        public bool IsSolved
        {
            get
            {
                return (bool)GetValue(IsSolvedProperty);
            }
            set
            {
                SetValue(IsSolvedProperty, value);
            }
        }
        #endregion

        public NonogramControl()
        {
            this.InitializeComponent();
        }

        #region BuildMethods
        private void BuildLeftSide(int[][] leftSideValues)
        {
            // находим максимальное количество чисел в каждой из строк
            int maxLength = 0;
            foreach (var arr in leftSideValues)
            {
                if (arr.Length > maxLength)
                {
                    maxLength = arr.Length;
                }
            }
            // на основе найденного значения устанавливаем количество колонок в гриде
            for (int i = 0; i < maxLength; i++)
            {
                LeftSideGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            // устанавливаем количество строк в гриде
            for (int i = 0; i < leftSideValues.Length; i++)
            {
                LeftSideGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            // заполняем грид значениями
            for (int i = 0; i < leftSideValues.Length; i++)
            {
                var arr = leftSideValues[i];
                for (int j = 0; j < arr.Length; j++)
                {
                    TextBlock tb = new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Text = arr[j].ToString() };
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                    LeftSideGrid.Children.Add(tb);
                }
            }
        }
        private void BuildTopSide(int[][] topSideValues)
        {
            // находим максимальное количество чисел в каждой из колонок
            int maxHeight = 0;
            foreach (var arr in topSideValues)
            {
                if (arr.Length > maxHeight)
                {
                    maxHeight = arr.Length;
                }
            }
            // на основе найденного значения устанавливаем количество строк в гриде
            for (int i = 0; i < maxHeight; i++)
            {
                TopSideGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            // устанавливаем количество колонок в гриде
            for (int i = 0; i < topSideValues.Length; i++)
            {
                TopSideGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            // заполняем грид значениями
            for (int i = 0; i < topSideValues.Length; i++)
            {
                var arr = topSideValues[i];
                for (int j = 0; j < arr.Length; j++)
                {
                    TextBlock tb = new TextBlock { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Text = arr[j].ToString() };
                    Grid.SetRow(tb, maxHeight - j - 1);
                    Grid.SetColumn(tb, i);
                    TopSideGrid.Children.Add(tb);
                }
            }
        }
        private void BuildFieldGrid(int rows, int columns)
        {
            // удаляем старое поле
            FieldGrid.Children.Clear();
            FieldGrid.RowDefinitions.Clear();
            FieldGrid.ColumnDefinitions.Clear();
            // строим новое поле
            for (int i = 0; i < rows; i++)
            {
                FieldGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int j = 0; j < columns; j++)
            {
                FieldGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //Style = (Style)Resources["Cell"]
                    CellControl cell = new CellControl();
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    cell.PointerPressed += CellControl_PointerPressed;
                    cell.PointerEntered += CellControl_PointerEntered;
                    cell.Tag = string.Format("[{0},{1}]", i, j);
                    //cell.TagType = TagType;
                    Binding tagTypeBinding = new Binding { Source = this, Path = new PropertyPath("TagType") };
                    cell.SetBinding(CellControl.TagTypeProperty, tagTypeBinding);
                    FieldGrid.Children.Add(cell);
                }
            }
        }
        #endregion
        // удалить
        public int MyProperty { get; set; } = 1;

        #region ControlEvents
        private void CellControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // задаем начальные координаты контура
            _outlineRectangle = new Rectangle { Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 4, Fill = new SolidColorBrush(Colors.BlueViolet), Opacity = 0.5 };
            CellControl currentCell = sender as CellControl;
            _beginPoint = _endPoint = new Point(Grid.GetColumn(currentCell), Grid.GetRow(currentCell));
            int column = (int)_beginPoint.X, row = (int)_beginPoint.Y;
            Grid.SetColumn(_outlineRectangle, column);
            Grid.SetRow(_outlineRectangle, row);
            FieldGrid.Children.Add(_outlineRectangle);

            // закрашиваем или стираем квадраты
            if (Field[row, column] == 0)
            {
                _checkMode = CheckMode;
            }
            else
            {
                _checkMode = 0;
            }
        }

        private void CellControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // рисуем контур
            if (e.Pointer.IsInContact)
            {
                CellControl currentCell = sender as CellControl;
                _endPoint = new Point(Grid.GetColumn(currentCell), Grid.GetRow(currentCell));
                int rowSpan = Grid.GetRowSpan(_outlineRectangle), columnSpan = Grid.GetColumnSpan(_outlineRectangle);
                if (rowSpan == 1 && columnSpan == 1)
                {
                    // чтобы избежать ситуации 2х2
                    if ((int)Math.Abs(_beginPoint.X - _endPoint.X) > 0)
                    {
                        Grid.SetColumn(_outlineRectangle, (int)Math.Min(_beginPoint.X, _endPoint.X));
                        Grid.SetColumnSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.X - _endPoint.X) + 1);
                    }
                    else
                    {
                        Grid.SetRow(_outlineRectangle, (int)Math.Min(_beginPoint.Y, _endPoint.Y));
                        Grid.SetRowSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.Y - _endPoint.Y) + 1);
                    }
                }
                else if (columnSpan > 1)
                {
                    if ((int)Math.Abs(_beginPoint.X - _endPoint.X) >= (int)Math.Abs(_beginPoint.Y - _endPoint.Y))
                    {
                        Grid.SetColumn(_outlineRectangle, (int)Math.Min(_beginPoint.X, _endPoint.X));
                        Grid.SetColumnSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.X - _endPoint.X) + 1);
                    }
                    // чтобы изменить направление с горизонтального на вертикальное
                    else
                    {
                        Grid.SetRow(_outlineRectangle, (int)Math.Min(_beginPoint.Y, _endPoint.Y));
                        Grid.SetRowSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.Y - _endPoint.Y) + 1);
                        Grid.SetColumn(_outlineRectangle, (int)_beginPoint.X);
                        Grid.SetColumnSpan(_outlineRectangle, 1);
                    }
                }
                else if (rowSpan > 1)
                {
                    if ((int)Math.Abs(_beginPoint.Y - _endPoint.Y) >= (int)Math.Abs(_beginPoint.X - _endPoint.X))
                    {
                        Grid.SetRow(_outlineRectangle, (int)Math.Min(_beginPoint.Y, _endPoint.Y));
                        Grid.SetRowSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.Y - _endPoint.Y) + 1);
                    }
                    // чтобы изменить направление с вертикального на горизонтальное
                    else
                    {
                        Grid.SetColumn(_outlineRectangle, (int)Math.Min(_beginPoint.X, _endPoint.X));
                        Grid.SetColumnSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.X - _endPoint.X) + 1);
                        Grid.SetRow(_outlineRectangle, (int)_beginPoint.Y);
                        Grid.SetRowSpan(_outlineRectangle, 1);
                    }
                }
            }
        }

        // удалить
        private void BaseFieldGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var x = FieldGrid.Children;

            //Field = new int[4, 4] { { 1, 0, 0, 0 }, { 1, 0, 0, 0 }, { 1, 0, 0, 0 }, { 1, 0, 0, 0 } };
        }

        private void FieldGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact)
            {
                _pointerCaptured = ((Grid)sender).CapturePointer(e.Pointer);
            }
        }

        private void FieldGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact && _pointerCaptured)
            {
                ((Grid)sender).ReleasePointerCapture(e.Pointer);
                _pointerCaptured = false;
            }
        }

        private void FieldGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // обновляем ui
            UpdateField(_outlineRectangle, _checkMode);
            // стираем контур
            FieldGrid.Children.Remove(_outlineRectangle);
            // проверяем решение
            CheckSolution(Field);
        }
        #endregion

        private void UpdateField(Rectangle outline, CheckModes checkMode)
        {
            int beginX = Grid.GetColumn(outline), beginY = Grid.GetRow(outline);
            int columns = Grid.GetColumnSpan(outline), rows = Grid.GetRowSpan(outline);
            int[,] field = Field; // костыль??? ещё и не работающий
            CellControl cell;
            if (rows > columns)
            {
                for (int i = beginY; i < beginY + rows; i++)
                {
                    field[i, beginX] = (int)_checkMode;
                    cell = FieldGrid.Children.Where(child => child.GetType() == typeof(CellControl) && ((FrameworkElement)child).Tag.ToString() == string.Format("[{0},{1}]", i, beginX)).Select(child => (CellControl)child).First();
                    cell.State = (CellStates)_checkMode;
                }
            }
            else
            {
                for (int j = beginX; j < beginX + columns; j++)
                {
                    field[beginY, j] = (int)_checkMode;
                    cell = FieldGrid.Children.Where(child => child.GetType() == typeof(CellControl) && ((FrameworkElement)child).Tag.ToString() == string.Format("[{0},{1}]", beginY, j)).Select(child => (CellControl)child).First();
                    cell.State = (CellStates)_checkMode;
                }
            }
            Field = field; // не срабатывает set!!!
        }

        #region CheckSolutionMethods
        public void CheckSolution(int[,] field)
        {
            bool top = CheckTopSideSolution(field);
            bool left = CheckLeftSideSolution(field);
        }

        private bool CheckTopSideSolution(int[,] field)
        {
            int[][] topSideSolution = new int[field.GetLength(1)][];
            int tempSum = 0;
            for (int j = 0; j < field.GetLength(1); j++)
            {
                List<int> tempList = new List<int>();
                tempSum = 0;
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if (field[i, j] <= 0)
                    {
                        if (tempSum > 0)
                        {
                            tempList.Add(tempSum);
                            tempSum = 0;
                        }
                    }
                    else
                    {
                        tempSum++;
                    }
                }
                if (tempSum > 0)
                {
                    tempList.Add(tempSum);
                }
                topSideSolution[j] = tempList.ToArray();
            }
            // доделать!!!
            var x = CompareJaggedArrays(topSideSolution, TopSideValues);
            return x;
        }

        private bool CheckLeftSideSolution(int[,] field)
        {
            int[][] leftSideSolution = new int[field.GetLength(0)][];
            int tempSum = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                List<int> tempList = new List<int>();
                tempSum = 0;
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] <= 0)
                    {
                        if (tempSum > 0)
                        {
                            tempList.Add(tempSum);
                            tempSum = 0;
                        }
                    }
                    else
                    {
                        tempSum++;
                    }
                }
                if (tempSum > 0)
                {
                    tempList.Add(tempSum);
                }
                leftSideSolution[i] = tempList.ToArray();
            }
            return CompareJaggedArrays(leftSideSolution, LeftSideValues);
        }

        private bool CompareJaggedArrays(int[][] arr1, int[][] arr2)
        {
            if (arr1.Length == arr2.Length)
            {
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (!arr1[i].SequenceEqual(arr2[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
