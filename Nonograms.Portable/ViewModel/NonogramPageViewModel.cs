﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Nonograms.Portable.Enums;
using Nonograms.Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public class NonogramPageViewModel : ViewModelBase
    {
        bool _isSolved;
        public bool IsSolved
        {
            get
            {
                return _isSolved;
            }
            set
            {
                _isSolved = value;
                RaisePropertyChanged();
                ShowDialogCommand.Execute(_isSolved);
            }
        }

        int[,] _field;
        public int[,] Field
        {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
                RaisePropertyChanged();
                _history.Push(_field);
            }
        }

        ObservableCollection<int[,]> _fieldHistory;
        public ObservableCollection<int[,]> FieldHistory
        {
            get
            {
                return _fieldHistory;
            }
            set
            {
                _fieldHistory = value;
            }
        }

        int[][] _leftSideValues;
        public int[][] LeftSideValues
        {
            get
            {
                return _leftSideValues;
            }
            set
            {
                _leftSideValues = value;
                RaisePropertyChanged();
            }
        }

        int[][] _topSideValues;
        public int[][] TopSideValues
        {
            get
            {
                return _topSideValues;
            }
            set
            {
                _topSideValues = value;
                RaisePropertyChanged();
            }
        }

        TagTypes _tagType;
        public TagTypes TagType
        {
            get
            {
                return _tagType;
            }

            set
            {
                _tagType = value;
                RaisePropertyChanged();
            }
        }

        CheckModes _checkMode;
        public CheckModes CheckMode
        {
            get
            {
                return _checkMode;
            }

            set
            {
                _checkMode = value;
                RaisePropertyChanged();
            }
        }

        Stack<int[,]> _history;
        NonogramInfo _nonogramInfo;

        RelayCommand _checkModeCommand;
        public RelayCommand CheckModeCommand
        {
            get
            {
                return _checkModeCommand ?? (_checkModeCommand = new RelayCommand(() =>
                {
                    CheckMode = CheckMode == CheckModes.Check ? CheckModes.Tag : CheckModes.Check;
                }));
            }
        }

        RelayCommand _tagTypeCommand;
        public RelayCommand TagTypeCommand
        {
            get
            {
                return _tagTypeCommand ?? (_tagTypeCommand = new RelayCommand(() =>
                {
                    TagType = TagType == TagTypes.Dot ? TagTypes.X : TagTypes.Dot;
                }));
            }
        }

        RelayCommand _previousStepCommand;
        public RelayCommand PreviousStepCommand
        {
            get
            {
                return _previousStepCommand ?? (_previousStepCommand = new RelayCommand(() =>
                {
                    //Field = (int[,])_history.Pop().Clone();
                    if (FieldHistory.Count > 1)
                    {
                        FieldHistory.Remove(FieldHistory.Last());
                    }
                }));
            }
        }

        RelayCommand _clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new RelayCommand(() =>
                {
                    Field = new int[_nonogramInfo.RowsNumber, _nonogramInfo.ColumnsNumber];
                }));
            }
        }

        RelayCommand<bool> _showDialogCommand;
        public RelayCommand<bool> ShowDialogCommand
        {
            get
            {
                return _showDialogCommand ?? (_showDialogCommand = new RelayCommand<bool>(async (p) =>
                {
                    var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                    //await dialog.ShowMessage("You won!", "Victory!!!", "Got it!", () => ClearCommand.Execute(null));
                    await dialog.ShowMessage("You won!", "Victory!!!");
                }, p => p));
            }
        }

        public NonogramPageViewModel()
        {

        }

        private void InitializeField(int rows, int columns)
        {
            LeftSideValues = _nonogramInfo.LeftSideValues;
            TopSideValues = _nonogramInfo.TopSideValues;
            TagType = TagTypes.Dot;
            CheckMode = CheckModes.Check;
            FieldHistory = new ObservableCollection<int[,]>();
            //FieldHistory.CollectionChanged += FieldHistory_CollectionChanged;
            FieldHistory.Add(new int[rows, columns]);
            //_history = new Stack<int[,]>();
            //if (_history.Count == 0)
            //{
            //    Field = new int[rows, columns];
            //    //Field = new int[4, 4] { { 1, 0, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 0 }, { 0, 0, 0, 0 } };
            //}
            //else
            //{
            //    // add code
            //}
        }

        private void FieldHistory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var x = e.OldItems;
            var y = e.NewItems;
            var z = FieldHistory;
        }

        public void LoadNonogram(NonogramInfo nonogramInfo)
        {
            _nonogramInfo = nonogramInfo;
            InitializeField(_nonogramInfo.RowsNumber, _nonogramInfo.ColumnsNumber);
        }
    }
}
