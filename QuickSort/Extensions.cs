using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Project2
{
    public static class Extensions
    {

        static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        private static int Partition<T>(this IList<T> list, int left, int right) where T : IComparable
        {
            // ensure our list isn't already sorted
            if (left >= right) return -1;

            // select random pivot
            var rand = new Random();
            var pivotPoint = rand.Next(left, right + 1);
            var pivot = list[pivotPoint];

            // swap pivot with leftmost element
            list.Swap(left, pivotPoint);

            int mark = left;

            // start at leftmost + 1, since the left will always be our pivot
            for (int i = left + 1; i <= right; i++)
            {
                // if the pivot is less than the current val at i we swap
                if (list[i].CompareTo(pivot) < 0)
                {
                    // update the mark value
                    mark++;

                    // swap values
                    list.Swap(mark, i);
                }
            }

            // perform final swap, at this point, our pivot is our left-most element
            list.Swap(mark, left);

            return mark;
        }

        #region Parallel Quicksort
        private static void InnerQuicksortParallel<T>(this IList<T> list, int left, int right) where T : IComparable
        {
            if (left > right || left < 0 || right < 0) return;

            // get a partition index out of our current list
            var partitionIndex = list.Partition(left, right);

            // if we ever recieve a -1 as a return val our list is sorted
            if (partitionIndex != -1)
            {
                // recursively call quicksort on our two sub-lists divided by the partition
                // parallelize each sub-list since the algorithm is a divide-and-conquer
                // one, this should be completely thread-safe as each method will work
                // on a different part of the list
                Parallel.Invoke(
                    () => list.InnerQuicksortParallel(left, partitionIndex - 1),
                    () => list.InnerQuicksortParallel(partitionIndex + 1, right)
                    );
            }
        }

        public static void QuicksortParallel<T>(this IList<T> list) where T : IComparable
        {
            list.InnerQuicksortParallel(0, list.Count - 1);
        }
        #endregion

        #region Sequential Quicksort
        private static void InnerQuicksortSequential<T>(this IList<T> list, int left, int right) where T: IComparable
        {
            if (left > right || left < 0 || right < 0) return;

            // get a partition index out of our current list
            var partitionIndex = list.Partition(left, right);

            // if we ever recieve a -1 as a return val our list is sorted
            if (partitionIndex != -1)
            {
                // recursively call quicksort on our two sub-lists divided by the partition
                list.InnerQuicksortSequential(left, partitionIndex - 1);
                list.InnerQuicksortSequential(partitionIndex + 1, right);
            }
        }

        public static void QuicksortSequential<T>(this IList<T> list) where T: IComparable
        {
            list.InnerQuicksortSequential(0, list.Count - 1);
        }
        #endregion
    }
}
