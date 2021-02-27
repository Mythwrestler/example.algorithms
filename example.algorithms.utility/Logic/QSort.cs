using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Algorithms.Utility.Models;

namespace Examples.Algorithms.Utility.Helpers
{
    public static class QSort
    {

        public static QsortArraySet SortIntegerArray(int[] integerArray)
        {
            // Create QSort Set Object and capture initial array
            QsortArraySet qSortSet = new QsortArraySet();
            qSortSet.InitialArray = integerArray.Select(i => i).ToArray();

            // Create copy of array for recursion
            int[] workingArray = integerArray.Select(i => i).ToArray();

            // Begin recursive qsort
            PivotArraySegment(workingArray, 0, workingArray.Length, qSortSet, 0);

            // Capture final version of working array
            qSortSet.EndingArray = workingArray.Select(i => i).ToArray();

            return qSortSet;
        }


        private static void PivotArraySegment(int[] integerArray, int startPosition, int length, QsortArraySet qSortSet, int parentIteration)
        {


            int currentSwap = -1;
            int nextToSwap = 0;
            int arrayCount = 1;


            // Get local copy of array segment
            SortItem[] arrayForPivot = integerArray.SubArray(startPosition, length).Select(i => new SortItem() { Value = i }).ToArray();

            // Set up Pivot Set and set inital values.
            ArrayPivotSet currentPivotSet = new ArrayPivotSet();
            List<ItemArray> arrayListing = new List<ItemArray>();
            currentPivotSet.StartingArray = integerArray.Select(i => i).ToArray();
            currentPivotSet.WorkingArray = integerArray.Select(i => i).ToArray();
            currentPivotSet.WorkingArrayPosition = startPosition;
            currentPivotSet.ArrayLength = length;

            // Capture parent iteration
            currentPivotSet.ParentIteration = parentIteration;

            // Set current iteration
            currentPivotSet.Iteration = qSortSet.Pivots.Select(a => a.Iteration).DefaultIfEmpty().Max() + 1;

            // Set Recursion level
            currentPivotSet.RecursionLevel = qSortSet.Pivots.Where(a => a.Iteration == parentIteration).Select(a => a.RecursionLevel).FirstOrDefault() + 1;

            // Check If array segment is base case.
            currentPivotSet.IsBaseCase = arrayForPivot.Select(i => i.Value).ToArray().IsBaseSet();



            // Set pivot information

            int pivotValue = arrayForPivot[arrayForPivot.Length - 1].Value;

            arrayForPivot[arrayForPivot.Length - 1].Pivot = true;
            currentPivotSet.PivotValue = pivotValue;

            // Identify potential Swaps
            int potentialSwaps = -1;
            foreach (var item in arrayForPivot)
            {
                if (item.Value <= pivotValue)
                {
                    item.BelowOrAtPivot = true;
                    potentialSwaps += 1;
                }
            }

            if (currentPivotSet.IsBaseCase)
            {
                currentPivotSet.EndingArray = currentPivotSet.StartingArray;
                currentPivotSet.EndingArrayPivotIndex = startPosition + Array.IndexOf(arrayForPivot, arrayForPivot.Where(i => i.Pivot).FirstOrDefault(), 0);
                qSortSet.Pivots.Add(currentPivotSet);
                return;
            }

            // Initial Mark For First Swap
            arrayForPivot[nextToSwap].NextSwap = true;
            arrayListing.Add(new ItemArray()
            {
                ArrayCount = arrayCount,
                Array = CloneSortItemArray(arrayForPivot)
            });


            // Pivot Array, saving an image of the array after each pivot.
            for (int i = 0; i < arrayForPivot.Length; i++)
            {
                // Identify if value is less than pivot
                if (arrayForPivot[i].Value <= pivotValue)
                {
                    if(currentSwap >=0) arrayForPivot[currentSwap].Swapped = false;
                    if(i >=0) arrayForPivot[i].Swapped = false;

                    arrayCount += 1;
                    currentSwap += 1;

                    SwapSortItems(ref arrayForPivot[currentSwap], ref arrayForPivot[i]);

                    // Check if there will be another swap after current swap
                    if (potentialSwaps > currentSwap)
                    {
                        nextToSwap += 1;
                        arrayForPivot[nextToSwap].NextSwap = true;
                    }

                    arrayListing.Add(new ItemArray()
                    {
                        ArrayCount = arrayCount,
                        Array = CloneSortItemArray(arrayForPivot)
                    });

                }
            }

            currentPivotSet.WorkingArrays = arrayListing.ToArray();

            // Update the main array to reflect pivots
            for (int i = 0; i < arrayForPivot.Length; i++)
            {
                integerArray[startPosition + i] = arrayForPivot[i].Value;
            }

            currentPivotSet.EndingArray = integerArray.Select(a => a).ToArray();

            currentPivotSet.EndingArrayPivotIndex = startPosition + Array.IndexOf(arrayForPivot, arrayForPivot.Where(i => i.Pivot).FirstOrDefault(), 0);

            // Check if pivoted array is a base case.  If so, return without recursion
            currentPivotSet.IsBaseCase = arrayForPivot.Select(i => i.Value).ToArray().IsBaseSet();
            if (currentPivotSet.IsBaseCase)
            {
                qSortSet.Pivots.Add(currentPivotSet);
                return;
            }

            // Split the Array and Make Recursive Calls
            var pivotIndex = Array.FindIndex(arrayForPivot, i => i.Pivot);

            int leftSplitStart = startPosition;
            int leftSplitLength = pivotIndex;
            int rightSplitStart = startPosition + pivotIndex + 1;
            int rightSplitLength = arrayForPivot.Length - 1 - pivotIndex;

            // left split only needs to happen if more than 1 value is left of pivot.
            if (leftSplitLength >= 1)
            {
                currentPivotSet.LeftSplit = integerArray.SubArray(leftSplitStart, pivotIndex);
                currentPivotSet.LeftSplitStart = leftSplitStart;
                currentPivotSet.LeftSplitLength = leftSplitLength;
            }

            // right split only needs to happen if more than 1 value is right of pivot.
            if (rightSplitLength >= 1)
            {
                currentPivotSet.RightSplit = integerArray.SubArray(rightSplitStart, rightSplitLength);
                currentPivotSet.RightSplitStart = rightSplitStart;
                currentPivotSet.RightSplitLength = rightSplitLength;
            }

            // save pivot set
            qSortSet.Pivots.Add(currentPivotSet);

            // Recursion Call over left split array
            if (currentPivotSet.LeftSplit != null)
            {
                PivotArraySegment(integerArray, leftSplitStart, leftSplitLength, qSortSet, currentPivotSet.Iteration);
            }

            // Recursion Call over right split array
            if (currentPivotSet.RightSplit != null)
            {
                PivotArraySegment(integerArray, rightSplitStart, rightSplitLength, qSortSet, currentPivotSet.Iteration);
            }
            return;
        }


        private static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static bool IsBaseSet<T>(this T[] arrayForCheck)
        {
            if (arrayForCheck.Length <= 1) return true;

            for (int i = 1; i < arrayForCheck.Length; i++)
            {
                if (Convert.ToInt32(arrayForCheck[i - 1]) > Convert.ToInt32(arrayForCheck[i]))
                {
                    return false;
                }
            }

            return true;

        }

        private static SortItem[] CloneSortItemArray(SortItem[] itemArray)
        {
            return itemArray.Select(i => new SortItem()
            {
                Value = i.Value,
                BelowOrAtPivot = i.BelowOrAtPivot,
                Pivot = i.Pivot,
                Swapped = i.Swapped,
                NextSwap = i.NextSwap
            }).ToArray();
        }

        private static void SwapSortItems(ref SortItem position1, ref SortItem position2)
        {
            // Save current "High" value
            SortItem tempItem = position1;

            // Move "Low" value into "High" value position.  Set swapped indicator.
            position1 = position2;
            position1.Swapped = true;

            // Move saved "High" value into "Low" value position. Set Swapped indicator. Turn Off next swap indicator.
            position2 = tempItem;
            position2.Swapped = true;
            position2.NextSwap = false;
        }


    }


}