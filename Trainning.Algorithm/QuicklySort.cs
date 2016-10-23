using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trainning.Algorithm
{
    public class QuicklySort
    {
        public static List<int> arrToSort = new List<int>() { 6, 1, 5, 9, 7, 5,11,22,42,10,22 };
        public static void QuilckSortArr(List<int> arr,int low,int high)
        {
            Console.WriteLine(ListToString(arr));

            if (low >= high) return;
            //把首位作为比较的枢轴值
            int pivot = arr[low];
            int i = low;
            int j = high;

            while (i < j)
            {
                //从右到左，找到第一个比基准值小的数
                while(arr[j]>=pivot && i < j)
                {
                    j--;
                }
                //执行到此,j已指向从右端起首个小于nPivot的元素
                //交换位置,把小于pivot的放在右边
                arr[i] = arr[j];

                //从左到右，找到第一个比基准值大的数
                while (arr[i] <= pivot && i < j)
                {
                    i++;
                }
                //执行到此,i已指向从左端起首个大于nPivot的元素
                //执行替换，把大于pivot的放在左边
                arr[j] = arr[i];
               
            }
            //退出while循环,执行至此,必定是i=j的情况
            //i(或j)指向的即是枢轴的位置,定位该趟排序的枢轴并将该位置返回
            arr[i] = pivot;

            //对枢轴的左端进行排序
            QuilckSortArr(arr, low, i-1);
            //对枢轴的右端进行排序
            QuilckSortArr(arr, i +1, high);
        }

        private static string ListToString(List<int> listInt)
        {
            string str = string.Join(",", listInt.ToArray());
            return str;
        }
    }
}
