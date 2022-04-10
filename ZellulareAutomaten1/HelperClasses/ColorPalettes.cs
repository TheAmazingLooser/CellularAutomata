using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace ZellulareAutomaten1.HelperClasses
{
    public static class ColorPalettes
    {
        public static ColorPalette GiantGoldfish = new ColorPalette(
            new Color(105, 210, 231), 
            new Color(167,219,216), 
            new Color(224,228,204),
            new Color(243,134,48),
            new Color(250,105,0));
        
        public static ColorPalette ThoughtProvoking = new ColorPalette(
            new Color(236,208,120), 
            new Color(217,91,67), 
            new Color(192,41,66),
            new Color(84,36,55),
            new Color(83,119,122));
        
        public static ColorPalette OceanFive = new ColorPalette(
            new Color(0,160,176), 
            new Color(106,74,60), 
            new Color(204,51,63),
            new Color(235,104,65),
            new Color(237,201,81));

        /// <summary>
        /// Returns a random ColorPalette from this static class (with the help of reflection)
        /// </summary>
        public static ColorPalette GetRandomPalette()
        {
            Random r = new Random();
            var Palettes = typeof(ColorPalettes).GetFields().Where(p => p.FieldType == typeof(ColorPalette)).ToArray();
            return (ColorPalette)Palettes[r.Next(0, Palettes.Length)].GetValue(null);
        }
    }
}