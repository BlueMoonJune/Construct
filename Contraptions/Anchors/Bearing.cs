using Construct.Power.Components;
using Construct.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Construct.Contraptions.Anchors
{
    public class BearingTE : ContraptionAnchor
    {

        public float rotation = 0;
        public Direction direction = 0;
        public Pulley drivePulley;

        public override Point Target => Position.ToPoint() + direction;

        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.active() && tile.type == ModContent.TileType<BearingTile>();
        }

        public override bool Assemble()
        {
            if (Assembled) return false;
            contraption = new(Target, Position.ToPoint());
            return true;
        }

        public override bool Disassemble()
        {
            if (!Assembled) return false;
            contraption.Disassemble();
            contraption = null;
            rotation = direction * MathHelper.PiOver2;
            return true;
        }

        public override void Update()
        {
            base.Update();
            if (contraption is null) return;
            rotation += Util.MathUtil.RPMToRad(drivePulley.RPM);
			drivePulley.Force -= drivePulley.RPM * 0.2f;
			rotation = rotation % MathHelper.TwoPi + MathHelper.TwoPi % MathHelper.TwoPi;
            contraption.position = Position.ToVector2() * 16 + Vector2.One * 8;
            contraption.rotation.Z = rotation - direction * MathHelper.PiOver2;
        }

        public override void OnPlace()
        {
            drivePulley = new Pulley(1, 0);
			AddComponent(drivePulley, new(0, 0));
        }

        public override void PostDraw()
        {
            base.PostDraw();
            Texture2D mountTexture = ModContent.Request<Texture2D>("Construct/Contraptions/Anchors/Bearing_Mount").Value;

            int frameX = (int)(rotation / MathHelper.PiOver2) % 4 * 32;
            Rectangle sourceRect = new Rectangle(frameX, 0, 32, 32);
            float stepProgress = rotation / MathHelper.PiOver2 % 1;

            Matrix offset = Matrix.Identity;
            offset.Translation = new Vector3(Position.X * 16 + 8 - Main.screenPosition.X, Position.Y * 16 + 8 - Main.screenPosition.Y, 0);
            Matrix transform = Main.Transform;
            transform = offset * transform;
            transform = Matrix.CreateRotationZ(rotation) * transform;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                transform
            );

            Color light = Lighting.GetColor(Position.X, Position.Y);

            Main.spriteBatch.Draw(mountTexture, new Vector2(-16, -16), sourceRect, Color.Black);

            Color col = new(1 - stepProgress, 1 - stepProgress, 1 - stepProgress, 0.0f);
            Main.spriteBatch.Draw(mountTexture, new Vector2(-16, -16), sourceRect, col.MultiplyRGBA(light));
            sourceRect.X += 32;
            sourceRect.X %= 32 * 4;
            col = new(stepProgress, stepProgress, stepProgress, 0.0f);
            Main.spriteBatch.Draw(mountTexture, new Vector2(-16, -16), sourceRect, col.MultiplyRGBA(light));

            Main.spriteBatch.End();


        }
    }

    public class BearingTile : EntityTile<BearingTE>
    {
        public override string Texture => "Construct/Contraptions/Anchors/Bearing";

        public override void PreStaticDefaults()
        {

            width = 1;
            height = 1;

            Main.tileSolid[Type] = true;
        }

        public override bool RightClick(int i, int j)
        {
            BearingTE te = GetTileEntity(i, j);
            if (te.contraption == null)
            {
                Main.NewText("Assemble");
                return te.Assemble();
            }
            else
            {
                return te.Disassemble();
            }
        }

        public override bool Slope(int i, int j)
        {
            BearingTE te = GetTileEntity(i, j);
            if (te.Assembled)
                return false;
            te.direction += 1;
            te.rotation = te.direction * MathHelper.PiOver2;
            Main.NewText(te.direction.num);
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || effectOnly) return;
            ModContent.GetInstance<BearingTE>().Kill(i, j);
        }
    }

    public class BearingItem : TileItem<BearingTile>
    {
        public override string Texture => "Construct/Contraptions/Anchors/Bearing";
    }
}
