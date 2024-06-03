using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Construct.Power.Components
{
    public class Pulley : Component
    {

        #region static fields

        public static string[] variants =
        {
            "Construct/Power/Components/Pulley_Stone",
            "Construct/Power/Components/Pulley_Wooden",
            "Construct/Power/Components/Pulley_Boreal",
        };

        public static Point[] largePoints =
        {
            new(-1, -1),
            new( 1, -1),
            new(-1,  1),
            new( 1,  1),
            new( 1,  0),
            new( 0,  1),
            new(-1,  0),
            new( 0, -1),
            new(0, 0)
        };

        public static Point[] medPoints =
        {
            new( 1,  0),
            new( 0,  1),
            new(-1,  0),
            new( 0, -1),
            new(0, 0)
        };

        public static Point[] smallPoints =
        {
            new(0, 0)
        };

        public static Point[][] allPoints =
        {
            smallPoints, medPoints, largePoints
        };

        public static Rectangle[] wheelRects =
        {
            new(0, 0, 16, 16),
            new(0, 16, 32, 32),
            new(32, 0, 48, 48),
        };
        public static Rectangle[] conncetorRects =
        {
            new(0, 0, 20, 20),
            new(0, 20, 36, 36),
            new(36, 0, 52, 52),
        };

        #endregion

        public int size;

        public int variant = 0;

        public Texture2D Texture => ModContent.Request<Texture2D>(variants[variant]).Value;

        public Pulley(int size, int variant)
        {
            this.size = size;
            this.variant = variant;
        }

        public override void Update()
        {
        }

        public override void DrawPreTiles()
        {

            Texture2D wheelTexture = ModContent.Request<Texture2D>(variants[variant]).Value;

            float stepProgress = rotation / MathHelper.PiOver2 % 1;
            float rot = rotation % MathHelper.PiOver2;

            Matrix offset = Matrix.Identity;
            offset.Translation = new Vector3(x * 16 + 8 - Main.screenPosition.X, y * 16 + 8 - Main.screenPosition.Y, 0);
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

            var light = Lighting.GetColor(x, y);

            var rect = wheelRects[size];

            var off = new Vector2(rect.Width / 2, rect.Height / 2);

            Main.spriteBatch.Draw(wheelTexture, -off, rect, Color.Black);

            Color col = new(1 - stepProgress, 1 - stepProgress, 1 - stepProgress, 0.0f);
            Main.spriteBatch.Draw(wheelTexture, -off, rect, col.MultiplyRGBA(light));
            col = new(stepProgress, stepProgress, stepProgress, 0.0f);
            Main.spriteBatch.Draw(Texture, Vector2.Zero, rect, col.MultiplyRGBA(light), -MathHelper.PiOver2, off, 1, SpriteEffects.None, 0);

            Main.spriteBatch.End();
        }

        public override void OnCreate()
        {
            foreach (Direction dir in Direction.directions()) {
				Point pos = Position + new Point(dir.point.X * 4, dir.point.Y * 4);
				foreach (Component comp in Component.byPosition.GetOrEmpty(pos)) {
					if (comp is Pulley pulley) {
						Connect(pulley);
						break;
					}
				}
			}
        }

        public override float GetAdvantageFactor(Component other)
        {
            if (other is Pulley p)
            {
                return (float)(size + 1) / (p.size + 1);
            }
            return 1;
        }
    }


}
