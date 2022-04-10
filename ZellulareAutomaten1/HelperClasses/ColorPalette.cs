using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZellulareAutomaten1.HelperClasses
{
    /// <summary>
    /// This class provides some Color-Palettes with 5 different colors per palette. Keep in mind that those palettes only work for 5 states.
    /// </summary>
    public class ColorPalette
    {
        private List<Color> Colors = new List<Color>();
        
        public ColorPalette(params Color[] colors)
        {
            Colors = colors.ToList();
        }
        
        public Color GetColor(int State)
        {
            if (State >= Colors.Count)
                throw new IndexOutOfRangeException($"Cannot get color {State} out of palette.");

            return Colors[State];
        }
    }
}