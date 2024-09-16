using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    internal static class GridLoader
    {
        public static Grid LoadFromFile(string fileName)
        {
            if(!File.Exists(fileName))
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't exist");
                return null;
            }

            string text = File.ReadAllText(fileName);

            if (text.Length == 0)
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't have content or broken");
                return null;
            }

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

        public static MatchTest[] FillTests(MatchTest[] tests, string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't exist");
                return null;
            }

            string text = File.ReadAllText(fileName);

            if (text.Length == 0)
            {
                Console.WriteLine($"ERROR - File {fileName} doesn't have content or broken");
                return null;
            }

            if (text.Split('-').Length < 2)
            {
                Console.WriteLine($"WARNING - This file doesn't have tests");
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

        public static Grid GetRandomized(int height, int width, int fillPercent = 50)
        {
            int[,] gridMatrix = new int[height, width];
            Random random = new Random();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    gridMatrix[i,j] = random.Next(0, 101) >= 100 - fillPercent ? 1 : 0;
                }
            }

            Grid grid = new Grid(gridMatrix);

            return grid;
        }
    }
}
