using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GMI_Technical_Assessment.Code
{
    public abstract class MatchRule
    {
        protected Color requiredColor;
        protected Color matrixColor;

        public Color RequiredColor { get { return requiredColor; } set { requiredColor = value; } }
        public Color MatrixColor { get { return matrixColor; } set { matrixColor = value; } }

        public MatchRule(Color matrixColor)
        {
            this.matrixColor = matrixColor;
        }

        public MatchRule(Color matrixColor, Color requiredColor)
        {
            this.matrixColor = matrixColor;
            this.requiredColor = requiredColor;
        }

        public abstract int FindMatches(Grid grid);
    }

    public class UnmatchedRule : MatchRule
    {
        public UnmatchedRule(Color matrixColor) : base(matrixColor)
        {
        }

        public UnmatchedRule(Color matrixColor, Color requiredColor) : base(matrixColor, requiredColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            int matchesCount = 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    if (grid.Matrix[i, j].value != 1 || grid.Matrix[i, j].color != matrixColor)
                        continue;

                    grid.Matrix[i, j].color = requiredColor;
                    matchesCount++;
                }
            }

            //Console.WriteLine($"Debug -> Unmatched - {matchesCount}");
            return matchesCount;
        }
    }

    public class CrossShape : MatchRule
    {
        public CrossShape(Color matrixColor) : base(matrixColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            bool isGridSidesEven = grid.Matrix.GetLength(0) % 2 == 0 || grid.Matrix.GetLength(1) % 2 == 0;
            bool isGridTooSmall = grid.Matrix.GetLength(0) < 2 || grid.Matrix.GetLength(1) < 2;

            if (isGridSidesEven || isGridTooSmall)
            {
                return 0;
            }

            int iCenter = grid.Matrix.GetLength(0) / 2;
            int jCenter = grid.Matrix.GetLength(1) / 2;

            for (int step = 0; ; step++)
            {
                int checkResult = 1;

                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                bool isHorizontalOutOfBounds = rightSide >= grid.Matrix.GetLength(1) || leftSide < 0;
                bool isVerticalOutOfBounds = topSide >= grid.Matrix.GetLength(0) || bottomSide < 0;

                if (isHorizontalOutOfBounds && isVerticalOutOfBounds)
                {
                    break;
                }

                if (!isHorizontalOutOfBounds)
                {
                    //If value is zero or grid cell is colored
                    checkResult *= grid.Matrix[iCenter, rightSide].value * (grid.Matrix[iCenter, rightSide].color == matrixColor ? 1 : 0);
                    checkResult *= grid.Matrix[iCenter, leftSide].value * (grid.Matrix[iCenter, leftSide].color == matrixColor ? 1 : 0);
                }

                if (!isVerticalOutOfBounds)
                {
                    //If value is zero or grid cell is colored
                    checkResult *= grid.Matrix[topSide, jCenter].value * (grid.Matrix[topSide, jCenter].color == matrixColor ? 1 : 0);
                    checkResult *= grid.Matrix[bottomSide, jCenter].value * (grid.Matrix[bottomSide, jCenter].color == matrixColor ? 1 : 0);
                }

                if (checkResult == 0)
                {
                    return 0;
                }
            }

            FillColorCross(grid, iCenter, jCenter);

            //Console.WriteLine($"Debug -> CrossShape - 1");
            return 1;
        }

        private void FillColorCross(Grid grid, int iCenter, int jCenter)
        {
            grid.Matrix[iCenter, jCenter].color = requiredColor;

            for (int step = 0; ; step++)
            {
                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                bool isHorizontalOutOfBounds = rightSide >= grid.Matrix.GetLength(0) || leftSide < 0;
                bool isVerticalOutOfBounds = topSide >= grid.Matrix.GetLength(0) || bottomSide < 0;

                if (isHorizontalOutOfBounds && isVerticalOutOfBounds)
                {
                    break;
                }

                if (!isHorizontalOutOfBounds)
                {
                    grid.Matrix[iCenter, rightSide].color = requiredColor;
                    grid.Matrix[iCenter, leftSide].color = requiredColor;
                }

                if (!isVerticalOutOfBounds)
                {
                    grid.Matrix[topSide, jCenter].color = requiredColor;
                    grid.Matrix[bottomSide, jCenter].color = requiredColor;
                }
            }
        }
    }

    public class TShape : MatchRule
    {
        public TShape(Color matrixColor) : base(matrixColor)
        {
        }

        public override int FindMatches(Grid grid)
        {
            bool isVerticalSideEven = grid.Matrix.GetLength(0) % 2 == 0;
            bool isHorizontalSideEven = grid.Matrix.GetLength(1) % 2 == 0;
            bool isGridTooSmall = grid.Matrix.GetLength(0) < 2 || grid.Matrix.GetLength(1) < 2;

            if (isVerticalSideEven && isHorizontalSideEven || isGridTooSmall)
            {
                return 0;
            }

            int iCenter = grid.Matrix.GetLength(0) / 2;
            int jCenter = grid.Matrix.GetLength(1) / 2;

            int horizontalResult = 1;
            int verticalResult = 1;

            for (int step = 0; ; step++)
            {
                int rightSide = jCenter + step;
                int leftSide = jCenter - step;
                int topSide = iCenter + step;
                int bottomSide = iCenter - step;

                bool isHorizontalOut = rightSide >= grid.Matrix.GetLength(1) || leftSide < 0;
                bool isVerticalOut = topSide >= grid.Matrix.GetLength(0) || bottomSide < 0;

                if (isHorizontalOut && isVerticalOut)
                {
                    break;
                }

                if (!isHorizontalOut && !isVerticalSideEven)
                {
                    //If value is zero or grid cell is colored
                    horizontalResult *= grid.Matrix[iCenter, rightSide].value * (grid.Matrix[iCenter, rightSide].color == matrixColor ? 1 : 0);
                    horizontalResult *= grid.Matrix[iCenter, leftSide].value * (grid.Matrix[iCenter, leftSide].color == matrixColor ? 1 : 0);
                }

                if (!isVerticalOut && !isHorizontalSideEven)
                {
                    //If value is zero or grid cell is colored
                    verticalResult *= grid.Matrix[topSide, jCenter].value * (grid.Matrix[topSide, jCenter].color == matrixColor ? 1 : 0);
                    verticalResult *= grid.Matrix[bottomSide, jCenter].value * (grid.Matrix[bottomSide, jCenter].color == matrixColor ? 1 : 0);
                }

                if (horizontalResult == 0 && verticalResult == 0)
                {
                    return 0;
                }
            }

            if (verticalResult == 1 && IsHorizontalSidesIsFilled(grid, out bool isBottom) && !isHorizontalSideEven)
            {
                TShapeSide shapeSide = isBottom ? TShapeSide.Buttom : TShapeSide.Top;
                FillTShapeColor(grid, shapeSide);

                //Console.WriteLine($"Debug -> TShape - 1");
                return 1;
            }

            else if (horizontalResult == 1 && IsVerticalSidesIsFilled(grid, out bool isRight) && !isVerticalSideEven)
            {
                TShapeSide shapeSide = isRight ? TShapeSide.Right : TShapeSide.Left;
                FillTShapeColor(grid, shapeSide);

                //Console.WriteLine($"Debug -> TShape - 1");
                return 1;
            }

            return 0;
        }

        private bool IsVerticalSidesIsFilled(Grid grid, out bool isRight)
        {
            int rightSide = 1;
            int leftSide = 1;

            int horizontalMax = grid.Matrix.GetLength(1) - 1;

            for (int verticalIndex = 0; verticalIndex < grid.Matrix.GetLength(0); verticalIndex++)
            {
                //If value is zero or grid cell is colored
                rightSide *= grid.Matrix[verticalIndex, horizontalMax].value * (grid.Matrix[verticalIndex, horizontalMax].color == matrixColor ? 1 : 0);
                leftSide *= grid.Matrix[verticalIndex, 0].value * (grid.Matrix[verticalIndex, 0].color == matrixColor ? 1 : 0);

                if (rightSide == 0 && leftSide == 0)
                {
                    isRight = false;
                    return false;
                }
            }

            isRight = rightSide == 1;
            return true;
        }

        private bool IsHorizontalSidesIsFilled(Grid grid, out bool isDown)
        {
            int topSide = 1;
            int bottomSide = 1;

            int verticalMax = grid.Matrix.GetLength(0) - 1;

            for (int horizontalIndex = 0; horizontalIndex < grid.Matrix.GetLength(1); horizontalIndex++)
            {
                //If value is zero or grid cell is colored
                bottomSide *= grid.Matrix[verticalMax, horizontalIndex].value * (grid.Matrix[verticalMax, horizontalIndex].color == matrixColor ? 1 : 0);
                topSide *= grid.Matrix[0, horizontalIndex].value * (grid.Matrix[0, horizontalIndex].color == matrixColor ? 1 : 0);

                if (topSide == 0 && bottomSide == 0)
                {
                    isDown = false;
                    return false;
                }
            }

            isDown = bottomSide == 1;
            return true;
        }

        private void FillTShapeColor(Grid grid, TShapeSide shapeSide)
        {
            FillSideColor(grid, shapeSide);

            if (shapeSide == TShapeSide.Top || shapeSide == TShapeSide.Buttom)
            {
                FillMiddleVerticalColor(grid);
            }

            else
            {
                FillMiddleHorizontalColor(grid);
            }
        }

        private void FillMiddleVerticalColor(Grid grid)
        {
            int jCenter = grid.Matrix.GetLength(1) / 2;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                grid.Matrix[i, jCenter].color = requiredColor;
            }
        }

        private void FillMiddleHorizontalColor(Grid grid)
        {
            int iCenter = grid.Matrix.GetLength(0) / 2;

            for (int j = 0; j < grid.Matrix.GetLength(1); j++)
            {
                grid.Matrix[iCenter, j].color = requiredColor;
            }
        }

        private void FillSideColor(Grid grid, TShapeSide shapeSide)
        {
            if (shapeSide == TShapeSide.Top || shapeSide == TShapeSide.Buttom)
            {
                int i = shapeSide == TShapeSide.Top ? 0 : grid.Matrix.GetLength(0) - 1;

                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    grid.Matrix[i, j].color = requiredColor;
                }
            }

            else
            {
                int j = shapeSide == TShapeSide.Left ? 0 : grid.Matrix.GetLength(1) - 1;

                for (int i = 0; i < grid.Matrix.GetLength(0); i++)
                {
                    grid.Matrix[i, j].color = requiredColor;
                }
            }
        }

        private enum TShapeSide
        {
            Top,
            Buttom,
            Right,
            Left,
        }
    }

    public class ShapeRule : MatchRule
    {
        private string name = "test";
        private Pattern[] patterns;

        public ShapeRule(Color matrixColor, string name, Pattern[] patterns) : base(matrixColor)
        {
            this.patterns = patterns;
            this.name = name;
        }

        public override int FindMatches(Grid grid)
        {
            int matches = 0;

            matches += FindAllPossibleMatches(grid);

            return matches;
        }



        private int FindAllPossibleMatches(Grid grid)
        {
            int matches = 0;

            for (int i = 0; i < grid.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Matrix.GetLength(1); j++)
                {
                    for(int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
                    {
                        Pattern pattern = patterns[patternIndex];

                        bool isAvaiableForHorizontal = j + pattern.Width() <= grid.Matrix.GetLength(1) &&
                                                       i + pattern.Height() <= grid.Matrix.GetLength(0);

                        if (isAvaiableForHorizontal)
                        {
                            if (IsPatternApplied(grid, pattern, i, j, true))
                            {
                                FillShape(grid, pattern, i, j, true);
                                matches++;

                                j += pattern.Width() - 1;
                            }
                        }

                        bool isAvaiableForVertical = j + pattern.Height() <= grid.Matrix.GetLength(1) &&
                                                     i + pattern.Width() <= grid.Matrix.GetLength(0);

                        if (isAvaiableForVertical)
                        {
                            if (IsPatternApplied(grid, pattern, i, j, false))
                            {
                                FillShape(grid, pattern, i, j, false);
                                matches++;

                                j += pattern.Height() - 1;
                            }
                        }
                    }
                }
            }

            return matches;
        }

        private bool IsPatternApplied(Grid grid, Pattern pattern, int iStart, int jStart, bool isHorizontal)
        {
            int needFlips = pattern.HasZero() ? 4 : 1;

            for (int flipIndex = 0; flipIndex < needFlips; flipIndex++)
            {
                int[,] matrix = pattern.GetFlipedPattern(flipIndex);
                bool isPatternAppliable = CheckPattern(matrix);

                if (isPatternAppliable)
                    return true;
            }

            return false;

            bool CheckPattern(int[,] flippedPattern)
            {
                for (int i = 0; i < flippedPattern.GetLength(0); i++)
                {
                    for (int j = 0; j < flippedPattern.GetLength(1); j++)
                    {
                        int iGrid = isHorizontal ? iStart + i : iStart + j;
                        int jGrid = isHorizontal ? jStart + j : jStart + i;

                        bool isCellSame = grid.Matrix[iGrid, jGrid].value == flippedPattern[i, j];

                        if (!isCellSame || grid.Matrix[iGrid, jGrid].color != matrixColor)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        private void FillShape(Grid grid, Pattern pattern, int iStart, int jStart, bool isHorizontal)
        {
            for (int i = 0; i < pattern.Height(); i++)
            {
                for (int j = 0; j < pattern.Width(); j++)
                {
                    int iGrid = isHorizontal ? iStart + i : iStart + j;
                    int jGrid = isHorizontal ? jStart + j : jStart + i;

                    if (grid.Matrix[iGrid, jGrid].value == 1)
                    {
                        grid.Matrix[iGrid, jGrid].color = requiredColor;
                    }
                }
            }
        }
    }

    public struct Pattern
    {
        public int x;
        public int y;
        public int[,] matrix;

        public Pattern(int[,] pattern)
        {
            x = 0;
            y = 0;

            this.matrix = pattern;
        }

        public int Width()
        {
            return matrix.GetLength(1); 
        }

        public int Height()
        {
            return matrix.GetLength(0);
        }

        public int[,] GetFlipedPattern(int flips)
        {
            int[,] flipedPatern = new int[matrix.GetLength(0), matrix.GetLength(1)];

            int iFactor;
            int jFactor;

            switch (flips)
            {
                case 1:
                    iFactor = -1;
                    jFactor = 1;
                    break;
                case 2:
                    iFactor = -1;
                    jFactor = -1;
                    break;
                case 3:
                    iFactor = 1;
                    jFactor = -1;
                    break;
                default:
                case 0:
                    iFactor = 1;
                    jFactor = 1;
                    break;
            }

            if (iFactor == 1 && jFactor == 1)
                return matrix;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    int iFliped = iFactor == -1 ? matrix.GetLength(0) - i - 1 : i;
                    int jFliped = jFactor == -1 ? matrix.GetLength(1) - j - 1 : j;

                    flipedPatern[i, j] = matrix[iFliped, jFliped];
                }
            }

            return flipedPatern;
        }

        public bool HasZero()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                        return true;
                }
            }

            return false;
        }
    }
}
