using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZellulareAutomaten1.HelperClasses;

namespace ZellulareAutomaten1
{
    /// <summary>
    /// This is the base-class used for all cellular automata.
    /// Just extend this class and override needed function (<see cref="UpdateCells"/>)
    /// </summary>
    public class CellularAutomata
    {
        protected byte States = 2; // 5
        protected int NeighbourDistance = 1; // 1
        private int StateThreshold = 1; // 1
        
        // 2d-Arrays are slower (index-wise) than jagged arrays
        protected byte[][] Cells;
        protected int Width;
        protected int Height;

        // Needed Variables for easy drawing
        protected Texture2D CellGridTexture;
        protected Color[] CellGridColors;
        
        // Spritebatch for Grid-Initialisations
        protected SpriteBatch SpriteBatch;
        
        #region Performance needed variabled
        private int[] TempXArray;
        private int[] TempYArray;
        #endregion
        
        public CellularAutomata(int w, int h, SpriteBatch sb)
        {
            // Set height and width.
            Width = w;
            Height = h;
            SpriteBatch = sb;
        }

        protected int GetCellData(int x, int y)
        {
            return Cells[x][y];
        }

        protected void InitializeRandomGrid()
        {
            // Initialize all cells with a random state (from 0 - M-1)
            Random r = new Random(1);
            Cells = new byte[Width][];
            for (int i = 0; i < Width; i++)
            {
                Cells[i] = new byte[Height];
            }
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x][y] = (byte)r.Next(0, States);
                }
            }
            
            CellGridTexture = new Texture2D(SpriteBatch.GraphicsDevice, Width, Height);
            CellGridColors = new Color[Width * Height];
            
            InitializePerformanceVariables();
        }

        protected void InitializePerformanceVariables()
        {
            int extraOdd = NeighbourDistance % 2;
            TempXArray = new int[NeighbourDistance * 2 + extraOdd];
            TempYArray = new int[NeighbourDistance * 2 + extraOdd];
        }
        
        /// <summary>
        /// Fixes the Indexes (either them being to low (smaller than 0) or to big (bigger then size)
        /// </summary>
        private int FixIndex(int val, int size)
        {
            // Check for negative indexes (x and y could be negative)
            // Wrap them around the size if they are.
            if (val < 0)
                return (size + val % size) % size;
            if (val >= size)
                return val % size;
            return val;
        }

        private void FixIndexArray(int[] arr, int startVal, int endVal, int maxSize)
        {
            int i = 0;
            for (int x = startVal; x < endVal; x++)
            {
                arr[i] = FixIndex(x, maxSize);
                i++;
            }
        }

        protected int GetNeighboursMoore(int cX, int cY, int distance, int state)
        {
            int neighbours = 0;

            int oddOffset = distance % 2;
            int maxX = cX + distance + oddOffset;
            int maxY = cY + distance + oddOffset;


            int xLen = distance * 2 + oddOffset;
            int yLen = xLen;
            
            // To improve performance massively we need to fix the indexes before the loop itself.
            // This reduces the need of over-calculating x and y-indexes (reducing the complexity from O(n²) to O(2n))
            FixIndexArray(TempXArray, cX - distance, maxX, Width);
            FixIndexArray(TempYArray, cY - distance, maxY, Height);

            for (int xIndex = 0; xIndex < xLen; xIndex++)
            {
                int xI = TempXArray[xIndex];
                for (int yIndex = 0; yIndex < yLen; yIndex++)
                {
                    int yI = TempYArray[yIndex];
                    if (xI == cX && yI == cY) continue;
                    neighbours += GetCellData(xI,yI) == state ? 1 : 0;
                }
            }

            return neighbours;
        }

        protected virtual void HandleKeyboardInput()
        {
            
        }

        protected virtual void UpdateCells()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            HandleKeyboardInput();
            UpdateCells();
        }

        public void Draw(SpriteBatch sb)
        {
            // Only draw the Textures if they're available.
            if (CellGridTexture != null)
                sb.Draw(CellGridTexture, Vector2.Zero, Color.White);
        }
    }
}