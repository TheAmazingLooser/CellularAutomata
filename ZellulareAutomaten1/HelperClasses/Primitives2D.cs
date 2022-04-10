using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZellulareAutomaten1.HelperClasses
{
    public static class Primitives2D
    {
        private static Texture2D pixel;

        private static void CreatePixel(SpriteBatch sb)
        {
            pixel = new Texture2D(sb.GraphicsDevice, 1,1,false, SurfaceFormat.Color);
            pixel.SetData<Color>(new Color[] { Color.White });
        }

        public static void PutPixel(this SpriteBatch sb, float x, float y, Color color)
        {
            PutPixel(sb, new Vector2(x,y), color);
        }

        public static void PutPixel(this SpriteBatch sb, Vector2 pos, Color color)
        {
            if (pixel == null)
                CreatePixel(sb);
            
            sb.Draw(pixel, pos,color);
        }
    }
}