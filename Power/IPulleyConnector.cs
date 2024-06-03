/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace Construct.Power
{

    public static class PulleyConnectorExtensions {

        public static void PulleyDraw(this IPulleyConnector p)
        {
            IPulleyConnector.DrawPulley(p.TE.Position.X + p.Center.X, p.TE.Position.Y + p.Center.Y, p.Rotation, p.Wheels);
        }
    }

    public interface IPulleyConnector : IRenderedTileEntity
    {
        public string Texture { get; }
        public Point Center { get; }
        public bool[] Wheels { get; }
        public float Rotation { get; }

        public static void DrawPulley(int i, int j, float rotation, bool[] wheels)
        {

            Texture2D wheelTexture = ModContent.Request<Texture2D>("Construct/Power/Pulley").Value;

            float stepProgress = rotation / MathHelper.PiOver2 % 1;
            float rot = rotation % MathHelper.PiOver2;

            Matrix offset = Matrix.Identity;
            offset.Translation = new Vector3(i * 16 + 8 - Main.screenPosition.X, j * 16 + 8 - Main.screenPosition.Y, 0);
            Matrix transform = Main.Transform;
            transform = offset * transform;
            transform = Matrix.CreateRotationZ(rot) * transform;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                transform
            );

            var light = Lighting.GetColor(i, j);

            for (int k = wheels.Length; k-- > 0;)
            {
                if (wheels[k])
                {

                    var rect = wheelRects[k];

                    var off = new Vector2(rect.Width / 2, rect.Height / 2);

                    Main.spriteBatch.Draw(wheelTexture, -off, rect, Color.Black);

                    Color col = new(1 - stepProgress, 1 - stepProgress, 1 - stepProgress, 0.0f);
                    Main.spriteBatch.Draw(wheelTexture, -off, rect, col.MultiplyRGBA(light));
                    col = new(stepProgress, stepProgress, stepProgress, 0.0f);
                    Main.spriteBatch.Draw(wheelTexture, Vector2.Zero, rect, col.MultiplyRGBA(light), -MathHelper.PiOver2, off, 1, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.End();
        }
    }
}
*/