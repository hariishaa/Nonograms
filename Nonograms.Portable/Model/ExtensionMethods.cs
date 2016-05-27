using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.Model
{
    public static class ExtensionMethods
    {
        public static bool AreValuesEqual(this int[,] originalArray, int[,] array)
        {
            if (originalArray.GetLength(0) != array.GetLength(0) && originalArray.GetLength(1) != array.GetLength(1))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < originalArray.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (originalArray[i, j] != array[i, j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
