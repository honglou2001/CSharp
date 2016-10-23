using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trainning.Algorithm
{
    public class HeapSort
    {
        public static List<int> arrToSort = new List<int>() { 1, 6, 5, 9, 7, 5, 11, 22, 42, 10, 22 };

        public static void CreateMaxHeap(List<int> listInt, int low, int high)
        {
            if(low>=high || high>listInt.Count-1)
            {
                return;
            }

            int j = 0, k = 0, t = 0;

            //根据完全二叉树特性，前一半元素都是父节点，所以只需要遍历前一半即可
            for (int i = high / 2; i >= low; --i)
            {
                k = i;
                t = listInt[i];//暂存当前节点值
                j = 2 * i + 1;//计算左节点下标(注意：数组下标是从0开始的，而完全二叉树的序号是从1开始的，所以这里的2*i+1是左子节点，而非右子节点！)  
                
                 while (j <= high) //如果左节点存在
                 {
                     //如果右节点也存在，且右节点更大
                     if ((j < high) && (j + 1 <= high) && (listInt[j] < listInt[j + 1]))
                     {
                         ++j;//将j值调整到右节点的序号，即经过该if判断后，j对应的元素就是i元素的左、右子节点中值最大的
                     }

                     //如果本身节点比子节点小
                     if (t < listInt[j])
                     {
                         listInt[k] = listInt[j];//将自己节点的值，更新为左右子节点中最大的值

                         //然后保存左右子节点中最大元素的下标(因为实际上要做的是将最大子节点与自身进行值交换，
                         //上一步只完成了交换值的一部分，后面还会继续处理才能完成整个交换)
                         k = j;
                         j = 2 * k + 1;//交换后，j元素就是父节点了，然后重新以j元素为父节点，继续考量其"左子节点"，准备进入新一轮while循环
                     }
                     else //如果本身已经是最大值了，则说明元素i所对应的子树，已经是最大堆，则直接跳出循环
                     {
                         break;
                     }
                 }

                 //接上面的交换值操作，将最大子节点的元素值替换为t(因为最近的一次if语句中，k=j 了，
                 //所以这里的listInt[k]其实就是listInt[j]=t，即完成了值交换的最后一步，
                 //当然如果最近一次的if语句为false,根本没进入，则这时的k仍然是i，维持原值而已)
                 listInt[k] = t;
            }
        }

        public static void HeapSortArr(List<int> listInt)
        {
            Console.WriteLine(ListToString(listInt));

            int tmp = 0;
            //初始时，将整个数组排列成"初始最大堆"
            CreateMaxHeap(listInt, 0, listInt.Count - 1);
            Console.WriteLine(ListToString(listInt));

            for (int i = listInt.Count - 1; i > 0; --i)
            {
                //将根节点与最末结点对换
                tmp = listInt[0];
                listInt[0] = listInt[i];
                listInt[i] = tmp;
                //去掉沉底的元素，剩下的元素重新排列成“最大堆”
                CreateMaxHeap(listInt, 0, i - 1);
                Console.WriteLine(ListToString(listInt));
            }
        }

        private static string ListToString(List<int> listInt)
        {
            string str = string.Join(",", listInt.ToArray());
            return str;
        }
    }
}
