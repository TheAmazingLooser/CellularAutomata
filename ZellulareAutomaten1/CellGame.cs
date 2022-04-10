using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZellulareAutomaten1.HelperClasses;

namespace ZellulareAutomaten1
{
    public class CellGame : Game
    {
        private const int HEIGHT = 200;
        private const int WIDTH = 200;
        
        private SpriteBatch sb;
        private CellularAutomata ca;

        public CellGame()
        {
            GraphicsDeviceManager gdm = new GraphicsDeviceManager(this);
            gdm.PreferredBackBufferHeight = HEIGHT;
            gdm.PreferredBackBufferWidth = WIDTH;

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            sb = new SpriteBatch(GraphicsDevice);
            ca = new CellularAutomata(WIDTH,HEIGHT, sb);
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