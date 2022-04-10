using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        private const double UPDATE_SUPPRESION = 100;
        
        private int States = 5;
        private int NeighbourDistance = 3;
        private int StateThreshold = 1;
        
        //private int ITERATIONS = 600;
        
        // Keyboard-Control
        private KeyboardState KeyboardState = new KeyboardState();
        
        // 2d-Arrays are slower (index-wise) than jagged arrays
        private int[][] Cells;
        private int Width;
        private int Height;

        // Needed Variables for easy drawing
        private Texture2D CellGridTexture;
        private Color[] CellGridColors;
        
        // Update suppression (pause updates because them being to expensive and stop rendering)
        private double UpdateMilliseconds = 0;
        
        // Spritebatch for Grid-Initialisations
        private SpriteBatch SpriteBatch;

        private ColorPalette UsedPalette;
        
        public CellularAutomata(int w, int h, SpriteBatch sb)
        {
            // Set height and width.
            Width = w;
            Height = h;
            SpriteBatch = sb;
            InitializeRandomGrid();
        }

        private void InitializeRandomGrid()
        {
            UsedPalette = ColorPalettes.GetRandomPalette();
            // Initialize all cells with a random state (from 0 - M-1)
            Random r = new Random();
            Cells = new int[Width][];
            for (int i = 0; i < Width; i++)
            {
                Cells[i] = new int[Height];
            }
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x][y] = r.Next(0, States);
                }
            }

            CellGridTexture = new Texture2D(SpriteBatch.GraphicsDevice, Width, Height);
            CellGridColors = new Color[Width * Height];
        }
        
        /// <summary>
        /// Fixes the Indexes (either them beeing to low (smaller than 0) or to big (bigger then size)
        /// </summary>
        private int FixIndex(int val, int size)
        {
            if (val < 0)
                return (size + val % size) % size;
            if (val >= size)
                return val % size;
            return val;
        }
        
        private int GetCellState(int x, int y)
        {
            // Check for negative indexes (x and y could be negative)
            // Wrap them around the grid if they are.
            x = FixIndex(x, Width);
            y = FixIndex(y, Height);
            
            return Cells[x][y];
        }

        private int GetNeighboursMoore(int cX, int cY, int distance, int cellState)
        {
            int neighbours = 0;

            int maxX = Math.Min(Width, cX + distance);
            int maxY = Math.Min(Height, cY + distance);

            int nextState = (cellState + 1) % States;

            for (int x = cX - distance; x < maxX; x++)
            {
                for (int y = cY - distance; y < maxY; y++)
                {
                    int nState = GetCellState(x, y);
                    
                    if (nextState == nState)
                        neighbours++;
                }
            }
    
            return neighbours;
        }
        
        public void Update(GameTime gameTime)
        {
            #region Keyboard-Input-Handling
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
            #endregion
            
            UpdateMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (UpdateMilliseconds < UPDATE_SUPPRESION) return;

            UpdateMilliseconds = 0;
            
            // LINQ-Copy jagged array.
            int[][] tCells = Cells.Select(s => s.ToArray()).ToArray();
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int Value = (Cells[x][y] + 1) % States;
                    int neighbours = GetNeighboursMoore(x, y, NeighbourDistance, Cells[x][y]);
                    if (neighbours > StateThreshold)
                    {
                        tCells[x][y] = Value;
                    }

                    if (States != 5)
                    {
                        Color c = ColorExtensions.ColorFromHSV(Value * (180/(double)States) + 60, 1, 1);
                        CellGridColors[x + y * Width] = c;
                    }
                    else
                    {
                        CellGridColors[x + y * Width] = UsedPalette.GetColor(Value);
                    }
                }
            }

            CellGridTexture.SetData(CellGridColors);
            Cells = tCells;
        }

        public void Draw(SpriteBatch sb)
        {
            // Only draw the Textures if they're available.
            if (CellGridTexture != null)
                sb.Draw(CellGridTexture, Vector2.One, Color.White);
            
        }
    }
}