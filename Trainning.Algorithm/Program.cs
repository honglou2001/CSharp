using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trainning.Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = QuicklySort.arrToSort.Count;
            int low = 0;
            int high = count - 1;
            QuicklySort.QuilckSortArr(QuicklySort.arrToSort, low, high);
            Console.Read();
        }
    }
}
