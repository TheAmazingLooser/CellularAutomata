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
    public class CellularAutomata
    {
        // Suppress all updates (cap updates at 1 update every 100 ms)
        
        private byte States = 5;
        private int NeighbourDistance = 5;
        private int StateThreshold = 29;
        
        //private int ITERATIONS = 600;
        
        // Keyboard-Control
        private KeyboardState KeyboardState = new KeyboardState();
        
        // 2d-Arrays are slower (index-wise) than jagged arrays
        private byte[][] Cells;
        private int Width;
        private int Height;

        // Needed Variables for easy drawing
        private Texture2D CellGridTexture;
        private Color[] CellGridColors;
        
        // Spritebatch for Grid-Initialisations
        private SpriteBatch SpriteBatch;

        private ColorPalette UsedPalette;
        
        #region Performance needed variabled
        private byte[][] TempCells; // Temporary jagged array. Removes the need of copying the Cells every update.
        private int[] TempXArray;
        private int[] TempYArray;
        #endregion
        
        public CellularAutomata(int w, int h, SpriteBatch sb)
        {
            // Set height and width.
            Width = w;
            Height = h;
            SpriteBatch = sb;
            InitializeRandomGrid();
        }

        private int GetCellData(int x, int y)
        {
            return Cells[x][y];
        }

        private void InitializeRandomGrid()
        {
            UsedPalette = ColorPalettes.GetRandomPalette();
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
            TempCells = Cells.Select(s => s.ToArray()).ToArray();
        }

        private void InitializePerformanceVariables()
        {
            TempXArray = new int[NeighbourDistance * 2];
            TempYArray = new int[NeighbourDistance * 2];
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

        private int GetNeighboursMoore(int cX, int cY, int distance, int cellState)
        {
            int neighbours = 0;

            int maxX = cX + distance;
            int maxY = cY + distance;

            int nextState = (cellState + 1) % States;

            int xLen = distance * 2;
            int yLen = xLen;
            
            // To improve performance massively we need to fix the indexes before the loop itself.
            // This reduces the need of over-calculating x and y-indexes (reducing the complexity from O(n²) to O(2n))
            FixIndexArray(TempXArray, cX - distance, maxX, Width);
            FixIndexArray(TempYArray, cY - distance, maxY, Width);

            for (int xIndex = 0; xIndex < xLen; xIndex++)
            {
                int xI = TempXArray[xIndex];
                for (int yIndex = 0; yIndex < yLen; yIndex++)
                {
                    neighbours += GetCellData(xI,TempYArray[yIndex]) == nextState ? 1 : 0;
                }
            }

            return neighbours;
        }

        private void HandleKeyboardInput()
        {
            // Handle Keyboard-Inputs.
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if (KeyboardState.IsKeyDown(Keys.OemPlus) && CurrentKeyboardState.IsKeyUp(Keys.OemPlus))
            {
                // "+" was pressed. Handle it as a parameter-change
                if (CurrentKeyboardState.IsKeyDown(Keys.LeftControl) ||
                    CurrentKeyboardState.IsKeyDown(Keys.RightControl))
                {
                    // Add + 1 to threshold.
                    StateThreshold++;
                    Console.WriteLine($"Increase threshold to {StateThreshold}");
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                         CurrentKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    // Increase neighbour-distance by 1
                    NeighbourDistance++;
                    Console.WriteLine($"Increase neighbour distance to {NeighbourDistance}");
                    InitializePerformanceVariables();
                }
                else
                {
                    // Add a new State
                    States++;
                    Console.WriteLine($"Increase amount of states to {States}");
                }
            } else if (KeyboardState.IsKeyDown(Keys.OemMinus) && CurrentKeyboardState.IsKeyUp(Keys.OemMinus))
            {
                // "-" was pressed. Handle it as a parameter-change
                if (CurrentKeyboardState.IsKeyDown(Keys.LeftControl) ||
                    CurrentKeyboardState.IsKeyDown(Keys.RightControl))
                {
                    // Subtract 1 from threshold.
                    StateThreshold--;
                    Console.WriteLine($"Decreased threshold to {StateThreshold}");
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift) ||
                         CurrentKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    // Subtract 1 from the distance of neighbours.
                    NeighbourDistance--;
                    Console.WriteLine($"Decreased neighbour distance to {NeighbourDistance}");
                    InitializePerformanceVariables();
                }
                else
                {
                    // Remove a State (cannot get smaller then 2)
                    States--;
                    if (States < 2) States = 2;
                    Console.WriteLine($"Decreased amount of states to {States}");
                }
            } else if (KeyboardState.IsKeyDown(Keys.Tab) && CurrentKeyboardState.IsKeyUp(Keys.Tab))
            {
                InitializeRandomGrid();
            }
            KeyboardState = CurrentKeyboardState;
        }

        private void UpdateCells()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int value = Cells[x][y];
                    byte nextValue = (byte)((value + 1) % States);
                    int neighbours = GetNeighboursMoore(x, y, NeighbourDistance, value);
                    if (neighbours > StateThreshold)
                    {
                        TempCells[x][y] = nextValue;
                        value = nextValue;
                    }

                    // If there are more or less than 5 states, we need to customize the coloring of all pixels since there are no hand-picked palettes.
                    if (States != 5)
                    {
                        Color c = ColorExtensions.ColorFromHSV(value * (180/(double)States) + 60, 1, 1);
                        CellGridColors[x + y * Width] = c;
                    }
                    else
                    {
                        // Update the current color of the pixel with the used palette.
                        CellGridColors[x + y * Width] = UsedPalette.GetColor(value);
                    }
                }
            }
            
            CellGridTexture.SetData(CellGridColors);
            Cells = TempCells.Select(s => s.ToArray()).ToArray();
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