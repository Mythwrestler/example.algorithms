import React, { ReactElement, useEffect, useState } from "react";
import API, { APIs } from "../../util/api";
import Button from "@material-ui/core/Button";
import {
  ButtonGroup,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Typography,
  Card,
  CardHeader,
  CardContent,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@material-ui/core";
import { QSortSet, ArraySortSet } from "./QSortSet";
import { useStyles } from "./styles";
import DoneOutlineOutlinedIcon from "@material-ui/icons/DoneOutlineOutlined";
import RemoveCircleOutlineOutlinedIcon from "@material-ui/icons/RemoveCircleOutlineOutlined";
import clsx from "clsx";

export interface QSortProps {}

enum SortStatus {
  FETCH_SMALL = "FETCH_SMALL",
  FETCH_MEDIUM = "FETCH_MEDIUM",
  FETCH_BIG = "FETCH_BIG",
  FETCHING = "FETCHING",
  FETCHED = "FETCHED",
  SORTING = "SORTING",
  SORTED = "SORTED",
}

const QSort: React.FunctionComponent<QSortProps> = () => {
  const classes = useStyles();
  const [arrayListStatus, setArrayListStatus] = useState<SortStatus | null>(
    null
  );
  const [initialArray, setInitialArray] = useState<number[]>([]);
  const [qsortSet, setQSortSet] = useState<QSortSet | null>(null);
  const [showLegend, setShowLegend] = useState<boolean>(false);
  const api: APIs = API();

  useEffect(() => {
    const fecthArrayList = async (
      maxNumberValue: number,
      arrayItemCount: number
    ) => {
      const { status, data } = await api.GetSampleIntegerArray(
        maxNumberValue,
        arrayItemCount
      );
      if (status == 200) {
        setInitialArray(data ?? []);
        setArrayListStatus(SortStatus.FETCHED);
      } else {
        setArrayListStatus(null);
      }
    };

    const sortArrayList = async () => {
      if (initialArray.length > 0) {
        const { status, data } = await api.SortIntegerArray(initialArray);
        if (status == 200) {
          setQSortSet(data);
          setArrayListStatus(SortStatus.SORTED);
        } else {
          setArrayListStatus(null);
        }
      } else {
        setArrayListStatus(null);
      }
    };

    if (
      arrayListStatus == SortStatus.FETCH_SMALL ||
      arrayListStatus == SortStatus.FETCH_MEDIUM ||
      arrayListStatus == SortStatus.FETCH_BIG
    ) {
      setInitialArray([]);
      setQSortSet(null);
      switch (arrayListStatus) {
        case SortStatus.FETCH_SMALL:
          fecthArrayList(10, 10);
          break;
        case SortStatus.FETCH_MEDIUM:
          fecthArrayList(100, 100);
          break;
        case SortStatus.FETCH_BIG:
          fecthArrayList(250, 250);
          break;
        default:
          setArrayListStatus(null);
          break;
      }
      setArrayListStatus(SortStatus.FETCHING);
    }

    if (arrayListStatus == SortStatus.FETCHED) {
      setArrayListStatus(SortStatus.SORTING);
      sortArrayList();
    }
  }, [initialArray, arrayListStatus]);

  const isInReview = (
    arrayIndex: number,
    startingPostion: number,
    length: number
  ): boolean => {
    if (
      arrayIndex >= startingPostion &&
      arrayIndex < startingPostion + length
    ) {
      return true;
    }
    return false;
  };

  const hasSplit = (checkArray: number, checkArray2: number): boolean => {
    if (checkArray === 0 || checkArray2 === 0) {
      return false;
    }
    return true;
  };

  interface DisplayArrayProps {
    label?: string;
    array: number[] | ArraySortSet | undefined;
  }
  const DisplayArray = ({
    label = undefined,
    array,
  }: DisplayArrayProps): JSX.Element => {
    if (array == undefined) return <></>;
    return (
      <div>
        {label && <Typography variant="h5">{label}</Typography>}
        <TableContainer style={{ overflowX: "auto" }}>
          <Table>
            <TableBody>
              <TableRow>
                {Array.isArray(array)
                  ? (array as number[]).map((val, index) => (
                      <TableCell key={`${index}-${val}`}>{val}</TableCell>
                    ))
                  : (array as ArraySortSet).startingArray.map((val, index) => {
                      <TableCell
                        key={`${index}-${val}`}
                        className={
                          isInReview(
                            index,
                            array.workingArrayPosition,
                            array.arrayLength
                          )
                            ? classes.inReview
                            : undefined
                        }
                      >
                        {val + "test"}
                      </TableCell>;
                    })}
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
      </div>
    );
  };

  interface WorkingSetProps {
    arraySortSet: ArraySortSet;
  }

  const WorkingSet = ({ arraySortSet }: WorkingSetProps): JSX.Element => {
    return (
      <TableContainer style={{ overflowX: "auto" }}>
        <Table>
          <TableBody>
            <TableRow>
              {arraySortSet.startingArray.map((val, index) => {
                return (
                  <TableCell
                    key={`${index}-${val}`}
                    className={
                      isInReview(
                        index,
                        arraySortSet.workingArrayPosition,
                        arraySortSet.arrayLength
                      )
                        ? classes.inReview
                        : undefined
                    }
                  >
                    {val}
                  </TableCell>
                );
              })}
            </TableRow>
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  interface PivotSetsProp {
    arraySortSet: ArraySortSet;
  }

  const PivotSets = ({ arraySortSet }: PivotSetsProp): JSX.Element => {
    if (!arraySortSet.workingArrays) return <></>;
    return (
      <>
        <TableContainer style={{ overflowX: "auto" }}>
          <Table>
            <TableBody>
              {arraySortSet.workingArrays.map((itemArray, index) => {
                return (
                  <TableRow>
                    {itemArray.array.map((item, index) => {
                      return (
                        <TableCell
                          className={clsx(
                            item.pivot && classes.pivot,
                            item.swapped && classes.swappedItem,
                            item.belowOrAtPivot && classes.belowEqualPivot,
                            item.nextSwap && classes.nextForSwap
                          )}
                        >
                          {item.value}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        <Typography>Array Pivoted on: {arraySortSet.pivotValue}</Typography>
        <TableContainer style={{ overflowX: "auto" }}>
          <Table>
            <TableBody>
              <TableRow>
                {arraySortSet.endingArray.map((val, index) => {
                  return (
                    <TableCell
                      className={clsx(
                        isInReview(
                          index,
                          arraySortSet.leftSplitStart,
                          arraySortSet.leftSplitLength
                        ) && classes.leftSplit,
                        index == arraySortSet.endingArrayPivotIndex &&
                          classes.pivotSplit,
                        isInReview(
                          index,
                          arraySortSet.rightSplitStart,
                          arraySortSet.rightSplitLength
                        ) && classes.rightSplit,
                        arraySortSet.isBaseCase &&
                          isInReview(
                            index,
                            arraySortSet.rightSplitStart,
                            arraySortSet.rightSplitLength
                          ) &&
                          classes.baseCase
                      )}
                    >
                      {val}
                    </TableCell>
                  );
                })}
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
      </>
    );
  };

  const DisplayStatus = (): JSX.Element => {
    switch (arrayListStatus) {
      case SortStatus.FETCHING:
        return <Typography variant="h5">Fetching Array List</Typography>;
      case SortStatus.SORTING:
        return <Typography variant="h5">Sorting Array List</Typography>;
      case SortStatus.SORTING:
        return <Typography variant="h5">Summary of QSort Actions</Typography>;
      default:
        return <></>;
    }
  };

  const SortBreakdown = (): JSX.Element => {
    console.log(JSON.stringify(qsortSet));
    return (
      <>
        <DisplayArray label="Initial Array" array={qsortSet?.initialArray} />
        <DisplayArray label="Sorted Array" array={qsortSet?.endingArray} />
        {qsortSet?.sortSets.map((arraySortSet) => {
          const { isBaseCase, startingArray } = arraySortSet;
          return (
            <Card
              className={clsx(
                arraySortSet.isBaseCase && classes.isBaseCase,
                classes.summary
              )}
              style={
                arraySortSet.isBaseCase ? { backgroundColor: "#C3E8C3" } : {}
              }
            >
              <CardHeader
                title={
                  arraySortSet.isBaseCase
                    ? "Array Segment Sorted (Base Case)"
                    : "More Work Needed"
                }
                titleTypographyProps={{ variant: "h6" }}
                avatar={
                  arraySortSet.isBaseCase ? (
                    <DoneOutlineOutlinedIcon style={{ color: "green" }} />
                  ) : (
                    <RemoveCircleOutlineOutlinedIcon style={{ color: "red" }} />
                  )
                }
              />
              <CardContent>
                <Typography variant="caption">Array To Be Pivoted</Typography>
                <WorkingSet arraySortSet={arraySortSet} />
                <Typography variant="caption">Array Pivots</Typography>
                <PivotSets arraySortSet={arraySortSet} />
              </CardContent>
            </Card>
          );
        })}
      </>
    );
  };

  const LegendDialog = (): ReactElement => {
    return (
      <Dialog open={showLegend} onClose={() => setShowLegend(false)}>
        <DialogTitle>Legend</DialogTitle>
        <DialogContent>
          <TableContainer>
            <Table>
              <TableRow>
                <TableCell className={classes.inReview}>In Review</TableCell>
                <TableCell>
                  <Typography>Array chunk that is being sorted.</Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.nextForSwap}>
                  Next For Swap
                </TableCell>
                <TableCell>
                  <Typography>
                    Number that is being compared against pivot value.
                  </Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.pivotSplit}>
                  Pivot Value
                </TableCell>
                <TableCell>
                  <Typography>
                    Number that all other numbers in array will be compared
                    against. Numbers less than it will end to its right. Larger
                    numbers will end to its left.
                  </Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.pivot}>Pivoted Value</TableCell>
                <TableCell>
                  <Typography>
                    Value was swapped during the last pivot itteration.
                  </Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.pivot}>Pivoted Value</TableCell>
                <TableCell>
                  <Typography>
                    Value was swapped during the last pivot itteration.
                  </Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.pivot}>Pivoted Value</TableCell>
                <TableCell>
                  <Typography>
                    Value was swapped during the last pivot itteration.
                  </Typography>
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell className={classes.pivot}>Pivoted Value</TableCell>
                <TableCell>
                  <Typography>
                    Value was swapped during the last pivot itteration.
                  </Typography>
                </TableCell>
              </TableRow>
            </Table>
          </TableContainer>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowLegend(false)}>Close</Button>
        </DialogActions>
      </Dialog>
    );
  };

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        padding: "15px 5px 5px 5px",
      }}
    >
      <div style={{ marginBottom: "10px" }}>
        <Typography variant="h4" style={{ textAlign: "center" }}>
          This component demonstrates how a Qsort algorithm operates.
        </Typography>
      </div>

      <div
        style={{
          marginBottom: "10px",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <ButtonGroup
          variant="contained"
          color="primary"
          aria-label="contained primary button group"
        >
          <Button
            onClick={() => {
              setArrayListStatus(SortStatus.FETCH_SMALL);
            }}
          >
            Simple
          </Button>
          <Button
            onClick={() => {
              setArrayListStatus(SortStatus.FETCH_MEDIUM);
            }}
          >
            Medium
          </Button>
          <Button
            onClick={() => {
              setArrayListStatus(SortStatus.FETCH_BIG);
            }}
          >
            Complex
          </Button>
        </ButtonGroup>
        <Button
          variant="outlined"
          color="secondary"
          style={{ marginTop: "10px" }}
          onClick={() => setShowLegend(true)}
        >
          Legend
        </Button>
      </div>
      <div
        style={{
          marginBottom: "10px",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <DisplayStatus />
      </div>
      <div>
        {qsortSet && (
          <>
            <SortBreakdown />
          </>
        )}
      </div>

      <LegendDialog />
    </div>
  );
};

export default QSort;
