using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZellulareAutomaten1.HelperClasses;

namespace ZellulareAutomaten1.CellularExtensions
{
    public class CyclicCellularAutomata : CellularAutomata
    {
        private int StateThreshold = 29;
        
        private KeyboardState KeyboardState = new KeyboardState();
        
        private ColorPalette UsedPalette;
        
        public CyclicCellularAutomata(int w, int h, SpriteBatch sb) : base(w, h, sb)
        {
            States = 5;
            NeighbourDistance = 5;
            UsedPalette = ColorPalettes.GetRandomPalette();
            InitializeRandomGrid();
        }

        protected override void UpdateCells()
        {
            byte[][] TempCells = Cells.Select(s => s.ToArray()).ToArray();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int value = Cells[x][y];
                    byte nextValue = (byte)((value + 1) % States);
                    int neighbours = GetNeighboursMoore(x, y, NeighbourDistance, (value + 1)% States);
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

        protected override void HandleKeyboardInput()
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
    }
}