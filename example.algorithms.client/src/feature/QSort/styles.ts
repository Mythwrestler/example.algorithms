import { makeStyles } from "@material-ui/core/styles";

export const useStyles = makeStyles({
  inReview: {
    backgroundColor: "#68a84d",
    color: "white",
  },
  isBaseCase: {
    backgroundColor: "#C3E8C3",
  },
  summary: {
    marginBottom: "5px",
  },
  pivot: {
    backgroundColor: "rgb(130, 122, 105)",
    color: "rgb(0, 0, 0)",
  },
  swappedItem: {
    fontWeight: "bolder",
    fontSize:"1.25em"
  },
  belowEqualPivot: {
    backgroundColor: "rgb(198, 245, 198)",
    color: "rgb(0, 0, 0)",
  },
  nextForSwap: {
    backgroundColor: "rgb(226, 152, 152)",
    color: "rgb(0, 0, 0)",
  },
  leftSplit: {
    backgroundColor: "rgb(28, 22, 70)",
    color: "rgb(255, 255, 255)",
  },
  rightSplit: {
    backgroundColor: "rgb(118, 124, 0)",
    color: "rgb(255, 255, 255)",
  },
  pivotSplit: {
    backgroundColor: "rgb(94, 79, 44)",
    color: "rgb(0, 0, 0)",
    fontSize: "1.2em",
  },
  baseCase: {
    backgroundColor: "rgb(0, 170, 62)",
    color: "rgb(0, 0, 0)",
  },
});
