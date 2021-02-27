export interface QSortSet {
  initialArray: number[];
  endingArray: number[];
  sortSets: ArraySortSet[];
}

export interface ArraySortSet {
  startingArray: number[];
  isBaseCase: boolean;
  endingArray: number[];
  endingArrayPivotIndex: number;
  leftSplitStart: number;
  leftSplitLength: number;
  leftSplit: number[];
  rightSplitStart: number;
  rightSplitLength: number;
  rightSplit: number[];
  pivotValue: number;
  workingArrays: ItemArray[];
  workingArray: number[] | null;
  workingArrayPosition: number;
  arrayLength: number;
}

interface ItemArray {
  arrayCount: number;
  array: SortItem[];
}

interface SortItem {
  value: number;
  pivot: boolean;
  belowOrAtPivot: boolean;
  nextSwap: boolean;
  swapped: boolean;
}
