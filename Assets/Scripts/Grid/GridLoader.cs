using System.Linq;
using UnityEngine;

namespace GMI_Technical_Assessment.Code
{
    public static class GridLoader
    {
        public static Grid LoadFromFile(TextAsset textAsset)
        {
            string text = textAsset.text;
            
            string gridText = text.Split('-')[0];
            string[] rows = gridText.Split(new[] {'\r', '\n'} );
            rows = rows.Where(x=>!string.IsNullOrEmpty(x)).ToArray();

            int rowsLength = rows.Length;
            int columnsLength = rows[0].Split(' ').Length;
            int[,] gridMatrix = new int[rowsLength, columnsLength];


            for (int i = 0; i < rowsLength; i++)
            {
                string[] currentRow = rows[i].Split(' ');

                for (int j = 0; j < columnsLength; j++)
                {
                    if(currentRow[j] == "x")
                    {
                        gridMatrix[i,j] = -1;
                    }
                    else
                    {
                        gridMatrix[i,j] = int.Parse(currentRow[j]);
                    }
                }
            }

            Grid grid = new Grid(gridMatrix);

            return grid;
        }

        public static MatchTest[] FillTests(TextAsset textAsset, MatchTest[] tests)
        {
            string text = textAsset.text;

            if (text.Split('-').Length < 2)
            {
                Debug.LogWarning($"This file {textAsset.name} doesn't have tests");
                return tests;
            }

            string gridText = text.Split('-')[1];
            string[] rowTests = gridText.Split('\n');

            foreach (MatchTest currentTest in tests)
            {
                string testRow = FindRequiredTestRow(currentTest.ID);

                if (string.IsNullOrEmpty(testRow))
                {
                    currentTest.SetRequiredMatches(0);
                }

                else
                {
                    int requiredMatches = int.Parse(testRow.Split('=')[1]);
                    currentTest.SetRequiredMatches(requiredMatches);
                }
            }

            string FindRequiredTestRow(string testID)
            {
                foreach (string currentRow in rowTests)
                {
                    if(currentRow.Contains(testID))
                    {
                        return currentRow;
                    }
                }

                return string.Empty;
            }

            return tests;
        }
    }
}
