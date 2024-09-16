using UnityEngine;

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
                    gridMaxtrix[i,j].color = Color.white;
                }
            }
        }

        public void SetColor(Color color)
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
            public Color color;

            public GridCell(int value, Color color)
            {
                this.value = value;
                this.color = color;
            }
        }
    }
}
