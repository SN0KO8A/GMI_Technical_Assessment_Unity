using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    public class Grid
    {
        private GridCell[,] gridMaxtrix;

        public GridCell[,] Matrix => gridMaxtrix;

        public Grid(int[,] grid)
        {
            gridMaxtrix = new GridCell[grid.GetLength(0), grid.GetLength(1)];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    gridMaxtrix[i,j].value = grid[i,j];
                    gridMaxtrix[i,j].color = ConsoleColor.White;
                }
            }
        }

        public void DisplayMatrix()
        {
            string gridString = string.Empty;

            for (int i = 0; i < gridMaxtrix.GetLength(0); i++)
            {
                for (int j = 0; j < gridMaxtrix.GetLength(1); j++)
                {
                    Console.ForegroundColor = gridMaxtrix[i,j].color;
                    Console.Write(" " + gridMaxtrix[i,j].value);
                    Console.ResetColor();
                }

                Console.Write('\n');
            }
        }

        public void SetColor(ConsoleColor color)
        {
            for (int i = 0; i < gridMaxtrix.GetLength(0); i++)
            {
                for (int j = 0; j < gridMaxtrix.GetLength(1); j++)
                {
                    gridMaxtrix[i,j].color = color;
                }
            }
        }

        public struct GridCell
        {
            public int value;
            public ConsoleColor color;

            public GridCell(int value = 0, ConsoleColor color = ConsoleColor.White)
            {
                this.value = value;
                this.color = color;
            }
        }
    }
}
