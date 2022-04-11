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
        
        public static ColorPalette Ophelia = new ColorPalette(
            new Color(187,30,0), 
            new Color(221,77,7), 
            new Color(233,155,21),
            new Color(186,157,27),
            new Color(150,71,77));
        
        public static ColorPalette BlueEyesSea = new ColorPalette(
            new Color(0,24,162), 
            new Color(2,89,196), 
            new Color(6,151,230),
            new Color(2,226,255),
            new Color(77,254,245));

        public static ColorPalette ColorfulDay = new ColorPalette(
            new Color(255, 190, 11),
            new Color(251, 86, 7),
            new Color(255, 0, 110),
            new Color(131, 56, 236),
            new Color(58, 134, 255));

        /// <summary>
        /// Returns a random ColorPalette from this static class (with the help of reflection)
        /// </summary>
        public static ColorPalette GetRandomPalette()
        {
            Random r = new Random();
            var Palettes = typeof(ColorPalettes).GetFields().Where(p => p.FieldType == typeof(ColorPalette)).ToArray();
            var Palette = Palettes[r.Next(0, Palettes.Length)];
            Console.WriteLine("Randomed " + Palette.Name + " palette.");
            return (ColorPalette)Palette.GetValue(null);
        }
    }
}