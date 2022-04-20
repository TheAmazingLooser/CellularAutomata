using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZellulareAutomaten1.CellularExtensions;
using ZellulareAutomaten1.HelperClasses;

namespace ZellulareAutomaten1
{
    public class CellGame : Game
    {
        private const int HEIGHT = 300;
        private const int WIDTH = 300;
        
        private SpriteBatch sb;
        private CellularAutomata ca;

        public CellGame()
        {
            GraphicsDeviceManager gdm = new GraphicsDeviceManager(this);
            gdm.PreferredBackBufferHeight = HEIGHT;
            gdm.PreferredBackBufferWidth = WIDTH;
            gdm.SynchronizeWithVerticalRetrace = false;

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            sb = new SpriteBatch(GraphicsDevice);
            ca = new CyclicCellularAutomata(WIDTH,HEIGHT, sb);
        }

        protected override void Update(GameTime gameTime)
        {
            ca.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();

            ca.Draw(sb);
            
            sb.End();
        }
    }
}