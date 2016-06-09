using Nonograms.Portable.Enums;
using Nonograms.Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        bool[] _topSideSolutions, _leftSideSolutions;

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
            nc.BuildLeftSide((int[][])e.NewValue);
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
            nc.BuildTopSide((int[][])e.NewValue);
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
        #region FieldHistoryProperty
        public static readonly DependencyProperty FieldHistoryProperty = DependencyProperty.Register("FieldHistory", typeof(ObservableCollection<int[,]>), typeof(NonogramControl), new PropertyMetadata(default(ObservableCollection<int[,]>), OnFieldHistoryPropertyChanged));
        private static void OnFieldHistoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var collection = (ObservableCollection<int[,]>)e.NewValue;
                NonogramControl nc = d as NonogramControl;
                nc.FieldHistory.CollectionChanged += nc.FieldHistory_CollectionChanged;
                nc._field = (int[,])collection[0].Clone(); // чтобы у _field и collection[0] были ссылки на разные массивы
                int rows = collection[0].GetLength(0), columns = collection[0].GetLength(1);
                nc.BuildFieldGrid(rows, columns);
                if (collection.Count > 1)
                {
                    nc.UpdateField(collection.Last());
                    nc.IsSolved = nc.CheckSolution(0, columns, 0, rows, nc._field, nc.LeftSideValues, nc._leftSideSolutions, nc.TopSideValues, nc._topSideSolutions);
                }
            }
        }
        private void FieldHistory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                UpdateField(FieldHistory.Last());
                IsSolved = CheckSolution(0, _field.GetLength(1), 0, _field.GetLength
                    (0), _field, LeftSideValues, _leftSideSolutions, TopSideValues, _topSideSolutions);
            }
        }

        public ObservableCollection<int[,]> FieldHistory
        {
            get
            {
                return (ObservableCollection<int[,]>)GetValue(FieldHistoryProperty);
            }
            set
            {
                SetValue(FieldHistoryProperty, value);
            }
        }
        #endregion
        int[,] _field;
        //#region FieldProperty
        //public static readonly DependencyProperty FieldProperty = DependencyProperty.Register("Field", typeof(int[,]), typeof(NonogramControl), new PropertyMetadata(default(int[,]), OnFieldPropertyChanged));
        //private static void OnFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    NonogramControl nc = d as NonogramControl;
        //    int[,] oldField = ((int[,])e.OldValue);
        //    int[,] newField = ((int[,])e.NewValue);
        //    // чтобы построить FieldGrid один раз - при первом связывании свойства Field
        //    if (oldField == null)
        //    {
        //        nc.BuildFieldGrid(newField.GetLength(0), newField.GetLength(1));
        //    }
        //    // ТОРМОЗИТ ВСЁ
        //    //else if (!newField.AreValuesEqual(oldField))
        //    //{
        //    //    nc.UpdateField(newField);
        //    //}
        //    // чтобы можно было заново решить кроссворд, например, очистив поле
        //    if (nc.IsSolved)
        //    {
        //        nc.IsSolved = false;
        //    }
        //}
        //public int[,] Field
        //{
        //    get
        //    {
        //        return (int[,])GetValue(FieldProperty);
        //    }
        //    set
        //    {
        //        SetValue(FieldProperty, value);
        //    }
        //}
        //#endregion
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
            //FieldHistory.CollectionChanged += FieldHistory_CollectionChanged1;
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
                    TextBlock tb = new TextBlock { Style = (Style)Resources["NumberTextBlock"], Text = arr[j].ToString() };
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, maxLength - arr.Length + j);
                    LeftSideGrid.Children.Add(tb);
                }
            }
            _leftSideSolutions = new bool[leftSideValues.Length];
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
            for (int j = 0; j < topSideValues.Length; j++)
            {
                var arr = topSideValues[j];
                for (int i = 0; i < arr.Length ; i++)
                {
                    TextBlock tb = new TextBlock { Style = (Style)Resources["NumberTextBlock"], Text = arr[i].ToString() };
                    Grid.SetRow(tb, maxHeight - arr.Length + i);
                    Grid.SetColumn(tb, j);
                    TopSideGrid.Children.Add(tb);
                }
            }
            _topSideSolutions = new bool[topSideValues.Length];
        }
        private void BuildFieldGrid(int rows, int columns)
        {
            // строим новое поле
            for (int i = 0; i < rows; i++)
            {
                FieldGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            Grid.SetRowSpan(LeftSideGrid, rows);
            for (int j = 0; j < columns; j++)
            {
                FieldGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            Grid.SetColumnSpan(TopSideGrid, columns);
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    CellControl cell = new CellControl();
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    cell.PointerPressed += CellControl_PointerPressed;
                    cell.PointerEntered += CellControl_PointerEntered;
                    cell.Tag = string.Format("[{0},{1}]", i - 1, j - 1);
                    Binding tagTypeBinding = new Binding { Source = this, Path = new PropertyPath("TagType") };
                    cell.SetBinding(CellControl.TagTypeProperty, tagTypeBinding);
                    FieldGrid.Children.Add(cell);
                }
            }
            // строим разделяющие линии
            for (int i = 1; i <= rows; i++)
            {
                Rectangle horizontalLine = i % 5 == 1 ? new Rectangle { Style = (Style)Resources["BoldHorizontalLine"] } : new Rectangle { Style = (Style)Resources["HorizontalLine"] };
                Grid.SetRow(horizontalLine, i);
                FieldGrid.Children.Add(horizontalLine);
            }
            for (int j = 1; j <= columns; j++)
            {
                Rectangle verticalLine = j % 5 == 1 ? new Rectangle { Style = (Style)Resources["BoldVerticalLine"] } : new Rectangle { Style = (Style)Resources["VerticalLine"] };
                Grid.SetColumn(verticalLine, j);
                FieldGrid.Children.Add(verticalLine);
            }
        }
        #endregion
        #region ControlEvents
        private void CellControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_outlineRectangle == null)
            {
                // задаем начальные координаты контура
                _outlineRectangle = new Rectangle { Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 4, Fill = new SolidColorBrush(Colors.BlueViolet), Opacity = 0.5, IsHitTestVisible = false };
                CellControl currentCell = sender as CellControl;
                int column = Grid.GetColumn(currentCell), row = Grid.GetRow(currentCell);
                _beginPoint = _endPoint = new Point(column, row);
                Grid.SetColumn(_outlineRectangle, column);
                Grid.SetRow(_outlineRectangle, row);
                FieldGrid.Children.Add(_outlineRectangle);

                // инициализация прицела
                Grid.SetRow(HorizontalAimRectangle, row);
                Grid.SetColumn(VerticalAimRectangle, column);
                HorizontalAimRectangle.Visibility = Visibility.Visible;
                VerticalAimRectangle.Visibility = Visibility.Visible;

                // закрашиваем или стираем квадраты
                if (_field[row - 1, column - 1] == 0)
                {
                    _checkMode = CheckMode;
                }
                else
                {
                    _checkMode = 0;
                }
            }
        }

        private void CellControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // рисуем контур
            if (_outlineRectangle != null && e.Pointer.IsInContact)
            {
                CellControl currentCell = sender as CellControl;
                _endPoint = new Point(Grid.GetColumn(currentCell), Grid.GetRow(currentCell));
                int rowSpan = Grid.GetRowSpan(_outlineRectangle), columnSpan = Grid.GetColumnSpan(_outlineRectangle);

                // изменяем положение прицела
                Grid.SetRow(HorizontalAimRectangle, (int)_endPoint.Y);
                Grid.SetColumn(VerticalAimRectangle, (int)_endPoint.X);

                if (rowSpan == 1 && columnSpan == 1)
                {
                    //чтобы избежать ситуации 2х2
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
                    //чтобы изменить направление с горизонтального на вертикальное
                    if ((int)Math.Abs(_beginPoint.X - _endPoint.X) == 0)
                    {
                        Grid.SetRow(_outlineRectangle, (int)Math.Min(_beginPoint.Y, _endPoint.Y));
                        Grid.SetRowSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.Y - _endPoint.Y) + 1);
                        Grid.SetColumn(_outlineRectangle, (int)_beginPoint.X);
                        Grid.SetColumnSpan(_outlineRectangle, 1);
                    }
                    else
                    {
                        Grid.SetColumn(_outlineRectangle, (int)Math.Min(_beginPoint.X, _endPoint.X));
                        Grid.SetColumnSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.X - _endPoint.X) + 1);
                    }
                }             
                else if (rowSpan > 1)
                {
                    //чтобы изменить направление с вертикального на горизонтальное
                    if ((int)Math.Abs(_beginPoint.Y - _endPoint.Y) == 0)
                    {
                        Grid.SetColumn(_outlineRectangle, (int)Math.Min(_beginPoint.X, _endPoint.X));
                        Grid.SetColumnSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.X - _endPoint.X) + 1);
                        Grid.SetRow(_outlineRectangle, (int)_beginPoint.Y);
                        Grid.SetRowSpan(_outlineRectangle, 1);
                    }
                    else
                    {
                        Grid.SetRow(_outlineRectangle, (int)Math.Min(_beginPoint.Y, _endPoint.Y));
                        Grid.SetRowSpan(_outlineRectangle, (int)Math.Abs(_beginPoint.Y - _endPoint.Y) + 1);
                    }
                }
            }
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
            if (_outlineRectangle != null)
            {
                int beginX = Grid.GetColumn(_outlineRectangle) - 1, beginY = Grid.GetRow(_outlineRectangle) - 1;
                int columns = Grid.GetColumnSpan(_outlineRectangle), rows = Grid.GetRowSpan(_outlineRectangle);
                // обновляем ui
                UpdateField(beginX, columns, beginY, rows, _checkMode);
                // стираем контур
                FieldGrid.Children.Remove(_outlineRectangle);
                _outlineRectangle = null;

                VerticalAimRectangle.Visibility = Visibility.Collapsed;
                HorizontalAimRectangle.Visibility = Visibility.Collapsed;

                // проверяем решение
                IsSolved = CheckSolution(beginX, columns, beginY, rows, _field, LeftSideValues, _leftSideSolutions, TopSideValues, _topSideSolutions);
            }
        }
        #endregion

        private void UpdateField(int beginX, int columns, int beginY, int rows, CheckModes checkMode)
        {
            List<CellControl> allCells = FieldGrid.Children.OfType<CellControl>().ToList();
            int fieldHeight = _field.GetLength(1);
            if (rows > columns)
            {
                for (int i = beginY; i < beginY + rows; i++)
                {
                    _field[i, beginX] = (int)_checkMode;
                    allCells[i * fieldHeight + beginX].State = (CellStates)_checkMode;
                }
            }
            else
            {
                for (int j = beginX; j < beginX + columns; j++)
                {
                    _field[beginY, j] = (int)_checkMode;
                    allCells[beginY * fieldHeight + j].State = (CellStates)_checkMode;
                }
            }
            //Field = field;
            FieldHistory.Add((int[,])_field.Clone()); // чтобы ни один элемент FH не ссылался на _field
        }

        private void UpdateField(int[,] newField)
        {
            List<CellControl> allCells = FieldGrid.Children.OfType<CellControl>().ToList();
            for (int i = 0; i < newField.GetLength(0); i++)
            {
                for (int j = 0; j < newField.GetLength(1); j++)
                {
                    allCells[i * newField.GetLength(1) + j].State = (CellStates)newField[i, j];
                }
            }
            _field = (int[,])newField.Clone(); // чтобы _field не ссылался на newField
        }

        #region CheckSolutionMethods
        bool CheckSolution(int beginX, int columns, int beginY, int rows, int[,] field, int[][] leftSideValues, bool[] leftSideSolutions, int[][] topSideValues, bool[] topSideSolutions)
        {
            bool isRight; //правильность заполнения линии
            int tempSum; //временная переменная для подсчёта подряд идущих закрашенных клеток
            List<int> lineSolution = new List<int>(); //текущие числа в строке/колонке
            SolidColorBrush foreground; //цвет текста, который нужно будет установить
            SolidColorBrush unFilledForeground = new SolidColorBrush(Colors.Black); //цвет текста у незаполненной линии
            SolidColorBrush filledForeground = new SolidColorBrush(Colors.Gray); //цвет текста у заполненной линии
            //для столбцов
            for (int j = beginX; j < beginX + columns; j++)
            {
                //заполнение lineSolution
                lineSolution.Clear();
                tempSum = 0;
                isRight = true;
                for (int i = 0; i < leftSideValues.Length; i++)
                {
                    if (field[i, j] <= 0)
                    {
                        if (tempSum > 0)
                        {
                            lineSolution.Add(tempSum);
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
                    lineSolution.Add(tempSum);
                }
                //сравнение lineSolution со значениями j-го столбца и выделение чисел
                var textBlocksInColumn = TopSideGrid.Children.Cast<TextBlock>().Where(child => Grid.GetColumn(child) == j);
                if (!lineSolution.SequenceEqual(topSideValues[j]))
                {
                    isRight = false;
                    foreground = unFilledForeground;
                }
                else
                {
                    foreground = filledForeground;
                }
                foreach (TextBlock tb in textBlocksInColumn)
                {
                    tb.Foreground = foreground;
                }
                //пометка о правильности заполнения столбца
                _topSideSolutions[j] = isRight;
            }
            //для строк
            for (int i = beginY; i < beginY + rows; i++)
            {
                //заполнение lineSolution
                lineSolution.Clear();
                tempSum = 0;
                isRight = true;
                for (int j = 0; j < topSideValues.Length; j++)
                {
                    if (field[i, j] <= 0)
                    {
                        if (tempSum > 0)
                        {
                            lineSolution.Add(tempSum);
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
                    lineSolution.Add(tempSum);
                }
                //сравнение lineSolution со значениями i-й строки и выделение чисел
                var textBlocksInRow = LeftSideGrid.Children.Cast<TextBlock>().Where(child => Grid.GetRow(child) == i);
                if (!lineSolution.SequenceEqual(leftSideValues[i]))
                {
                    isRight = false;
                    foreground = unFilledForeground;
                }
                else
                {
                    foreground = filledForeground;
                }
                foreach (TextBlock tb in textBlocksInRow)
                {
                    tb.Foreground = foreground;
                }
                //пометка о правильности заполнения строки
                _leftSideSolutions[i] = isRight;
            }
            foreach (var topSolution in _topSideSolutions)
            {
                if (!topSolution)
                {
                    return false;
                }
            }
            foreach (var leftSolution in _leftSideSolutions)
            {
                if (!leftSolution)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
