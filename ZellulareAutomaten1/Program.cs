using System;
using Microsoft.Xna.Framework;

namespace ZellulareAutomaten1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (CellGame g = new CellGame())
            {
                g.Run();
            }
        }
    }
}