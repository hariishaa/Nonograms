using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Model
{
    // класс, заменяющий стандартный 2мерный массив, чтобы можно было отслеживать изменения значений и передавать новые значения во вьюмодель
    public class Observable2DArray : INotifyCollectionChanged
    {
        int[,] _array;
        public int Length { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Observable2DArray(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Length = rows * columns;
            _array = new int[rows, columns];
        }
        public int this[int index1, int index2]
        {
            get
            {
                return _array[index1, index2];
            }
            set
            {
                _array[index1, index2] = value;
                OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            }
        }

        #region INotifyCollectionChanged
        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, args);
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion INotifyCollectionChanged        
    }
}
