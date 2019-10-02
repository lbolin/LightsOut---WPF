using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_LightsOut
{
    class LightsOutGame
    {
        private int numCells = 3;               // Number of cells in grid
        public const int maxGridSize = 3;
        public const int minGridSize = 7;

        private bool[,] grid;                   // Stores on/off state of cells in grid
        private Random rand;		            // Used to generate random numbers

        // Returns the number of horizontal/vertical cells in the grid
        public int NumCells
        {
            get
            {
                return numCells;
            }
            set
            {
                if (value >= maxGridSize && value <= minGridSize)
                {
                    numCells = value;
                    NewGame();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Value must be between " + maxGridSize 
                        + " and " + minGridSize);
                }
            }
        }

        public LightsOutGame()
        {
            rand = new Random();    // Initialize random number generator

            grid = new bool[numCells, numCells];

            // Turn entire grid on
            for (int r = 0; r < numCells; r++)
            {
                for (int c = 0; c < numCells; c++)
                {
                    grid[r, c] = true;
                }
            }
        }

        // Returns the grid value at the given row and column
        public bool GetGridValue(int row, int col)
        {
            return grid[row, col];
        }

        // Randomizes the grid
        public void NewGame()
        {
            grid = new bool[numCells, numCells];

            // Fill grid with either white or black
            for (int r = 0; r < numCells; r++)
            {
                for (int c = 0; c < numCells; c++)
                {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
        }

        // Inverts the selected box and all surrounding boxes
        public void MakeMove(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < numCells && j >= 0 && j < numCells)
                    {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }
        }

        // Returns true if all cells are off
        public bool PlayerWon()
        {
            for (int r = 0; r < numCells; r++)
            {
                for (int c = 0; c < numCells; c++)
                {
                    if (grid[r, c])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
