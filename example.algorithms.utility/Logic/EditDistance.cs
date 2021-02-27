using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Algorithms.Utility.Models;

namespace Examples.Algorithms.Utility.Helpers
{
    public static class EditDistance
    {

        ///<summary> Optimal Edit Distance Algorithm (No substring edited more than once)</summary>
        ///<param name="yString">y-Axis string in comparison</param>
        ///<param name="xString">x-Axis string in comparison</param>
        ///<param name="caseInsensitive">do we care about case?</param>
        public static EditDistanceSet OptimalDistanceFromString01ToString02(string yString, string xString, bool caseInsensitive = false)
        {
            EditDistanceSet comparisonForReturn = new EditDistanceSet();
            comparisonForReturn.String01 = yString;
            comparisonForReturn.String02 = xString;
            comparisonForReturn.IsCaseInsensitive = caseInsensitive;

            char[] yStringCharacterArray, xStringCharacterArray;
            if (caseInsensitive)
            {
                yStringCharacterArray = yString.ToUpperInvariant().ToCharArray();
                xStringCharacterArray = xString.ToUpperInvariant().ToCharArray();
            }
            else
            {
                yStringCharacterArray = yString.ToCharArray();
                xStringCharacterArray = xString.ToCharArray();
            }


            int[][] editDistanceMatrix = BuildOptimalEditDistanceMatrix(yString, xString);

            // Iterate Grid Columns
            for (int column = 1; column <= editDistanceMatrix.GetUpperBound(0); column++)
            {

                // Iterate through Rows on each Column.
                for (int row = 1; row <= editDistanceMatrix[0].GetUpperBound(0); row++)
                {

                    List<int> editValues = new List<int>(){
                        editDistanceMatrix[column - 1][row] + 1, // Deletion
                        editDistanceMatrix[column][row - 1] + 1  // Addition
                    };

                    int cost = yStringCharacterArray[column - 1] == xStringCharacterArray[row - 1] ? 0 : 1;

                    editValues.Add(editDistanceMatrix[column - 1][ row - 1] + cost); // Substitution

                    // Check if transposition is possible
                    if (column > 1 && row > 1
                        && yStringCharacterArray[column - 1] == xStringCharacterArray[row - 1 - 1]
                        && yStringCharacterArray[column - 1 - 1] == xStringCharacterArray[row - 1])
                    {
                        editValues.Add(editDistanceMatrix[column - 2][row - 2] + cost); // transposition
                    }

                    editDistanceMatrix[column][row] = editValues.ToArray().Min();

                }
            }

            comparisonForReturn.EditDistanceMatrix = editDistanceMatrix;
            comparisonForReturn.EditDistance = editDistanceMatrix[yString.Length][xString.Length];

            return comparisonForReturn;
        }


        private static int[][] BuildOptimalEditDistanceMatrix(string yString, string xString)
        {

            int[][] editDistanceMatrix = new int[yString.Length + 1][];
            for (int i = 0; i < yString.Length + 1; i++)
            {
                editDistanceMatrix[i] = new int[xString.Length + 1];
            }

            for (int i = 0; i <= editDistanceMatrix.GetUpperBound(0); i++)
            {
                editDistanceMatrix[i][0] = i;
            }
            for (int j = 0; j <= editDistanceMatrix[0].GetUpperBound(0); j++)
            {
                editDistanceMatrix[0][j] = j;
            }

            return editDistanceMatrix;
        }





        ///<summary> True Edit Distance Algorithm (substrings can be edited more than once)</summary>
        ///<param name="yString">y-Axis string in comparison</param>
        ///<param name="xString">x-Axis string in comparison</param>
        ///<param name="caseInsensitive">do we care about case?</param>

        public static EditDistanceSet TrueDamerauLevenshteinDistance(string yString, string xString, bool caseInsensitive)
        {

            EditDistanceSet comparisonForReturn = new EditDistanceSet();
            comparisonForReturn.String01 = yString;
            comparisonForReturn.String02 = xString;
            comparisonForReturn.IsCaseInsensitive = caseInsensitive;

            char[] yStringCharacterArray, xStringCharacterArray;
            if (caseInsensitive)
            {
                yStringCharacterArray = yString.ToUpperInvariant().ToCharArray();
                xStringCharacterArray = xString.ToUpperInvariant().ToCharArray();
            }
            else
            {
                yStringCharacterArray = yString.ToCharArray();
                xStringCharacterArray = xString.ToCharArray();
            }

            // Build and intialize distance matrix
            int[][] editDistanceMatrix = BuildTrueEditDistanceMatrix(yString, xString);

            // Build and initialize alphabet listing
            SortedDictionary<char, int> alphabet = GetAlphabetDictionary(yStringCharacterArray, xStringCharacterArray);

            for (int i = 1; i <= yString.Length; i++)
            {
                int db = 0;
                for (int j = 1; j <= xString.Length; j++)
                {
                    int i1 = alphabet[xStringCharacterArray[j - 1]];
                    int j1 = db;
                    if (yStringCharacterArray[i - 1] == xStringCharacterArray[j - 1])
                    {
                        editDistanceMatrix[i + 1][j + 1] = editDistanceMatrix[i][j];
                        db = j;
                    }
                    else
                    {
                        int[] values = new int[] {
                            editDistanceMatrix[i][j] + 1,
                            editDistanceMatrix[i+1][j] + 1,
                            editDistanceMatrix[i][j+1] + 1,
                            editDistanceMatrix[i1][j1] + (i-i1-1) + 1 + (j-j1-1)
                        };
                        editDistanceMatrix[i + 1][j + 1] = values.Min();
                    }
                    alphabet[yStringCharacterArray[i - 1]] = i;
                }
            }

            comparisonForReturn.EditDistanceMatrix = editDistanceMatrix;
            comparisonForReturn.EditDistance = editDistanceMatrix[yString.Length + 1][xString.Length + 1];

            
            return comparisonForReturn;
        }


        private static int[][] BuildTrueEditDistanceMatrix(string yString, string xString)
        {

            int[][] editDistanceMatrix = new int[yString.Length + 2][];
            for (int i = 0; i < yString.Length + 2; i++)
            {
                editDistanceMatrix[i] = new int[xString.Length + 2];
            }

            var inf = xString.Length + yString.Length;
            editDistanceMatrix[0][0] = inf;
            for (int i = 0; i <= yString.Length; i++)
            {
                editDistanceMatrix[i + 1][0] = inf;
                editDistanceMatrix[i + 1][1] = i;
            }
            for (int j = 0; j <= xString.Length; j++)
            {
                editDistanceMatrix[0][j + 1] = inf;
                editDistanceMatrix[1][j + 1] = j;
            }

            return editDistanceMatrix;
        }

        private static SortedDictionary<char, int> GetAlphabetDictionary(char[] yString, char[] xString)
        {
            List<char> characterList = yString.ToList();
            characterList.AddRange(xString.ToList());
            var letterDictionary = characterList.Distinct().ToDictionary(key => key, value => 0);
            return new SortedDictionary<char, int>(letterDictionary);
        }

    }
}