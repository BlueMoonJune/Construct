using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Construct
{
    public class SpatialBubble : ModProjectile
    {
        public Tilebox box;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 8; // The width of projectile hitbox
            Projectile.height = 8; // The height of projectile hitbox
            Projectile.aiStyle = 0; // The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?
            Projectile.penetrate = 5; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 3000; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.alpha = 255; // The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            Projectile.light = 0.5f; // How much light emit around the projectile
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.extraUpdates = 1; // Set to above 0 if you want the projectile to update multiple time in a frame

            AIType = 3; // Act exactly like default Bullet
        }

        public override void OnSpawn(IEntitySource source)
        {
            box = new Tilebox(26, 26);
            Projectile.rotation = 0;
            Tilebox.mainTilemap.SwapTo();
            Point tilePos = (Projectile.Center / 16).ToPoint() - new Point(7, 7);
            for (int j = 6; j < 21; j++)
            {
                for (int i = 6; i < 21; i++)
                {
                    if (new Vector2(i-13, j-13).LengthSquared() <= 7*7)
                    {
                        Point localTile = new(i, j);
                        Point worldTile = new(tilePos.X + i - 6, tilePos.Y + j - 6);
                        if (WorldGen.CanKillTile(worldTile.X, worldTile.Y))
                        {
                            if (Main.tile[worldTile].type == 5)
                            {
                                WorldGen.KillTile(worldTile.X, worldTile.Y);
                                continue;
                            }
                            Tilebox.mainTilemap.CopyTo(worldTile, box, localTile);
                            WorldGen.KillTile(worldTile.X, worldTile.Y, noItem: true);
                        }
                    }
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 p = Projectile.Center;

            Matrix offset = Matrix.Identity;
            offset.Translation = new Vector3(p.X - Main.screenPosition.X, p.Y - Main.screenPosition.Y, 0);
            Matrix transform = Main.Transform;
            transform = Matrix.CreateFromAxisAngle(Vector3.UnitZ, Projectile.rotation) * offset * transform;

            //*
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                transform
            );
            /**/
            box.SwapTo();

            for (int j = 6; j < 21; j++)
            {
                int jr = j - 13;
                for (int i = 6; i < 21; i++)
                {
                    WorldGen.TileFrame(i, j);
                    int ir = i - 13;
                    if (new Vector2(ir, jr).LengthSquared() > 7 * 7)
                        continue;
                    Tile tile = Main.tile[i, j];
                    if (!tile.HasTile) continue;
                    Texture2D tex = TextureAssets.Tile[tile.type].Value;
                    Rectangle framing = new Rectangle(tile.frameX, tile.frameY, 16, 16);
                    Main.spriteBatch.Draw(tex, new Vector2(ir * 16 - 8, jr * 16 - 8), framing, Color.White);

                }
            }
            Tilebox.mainTilemap.SwapTo();

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.Transform
            );

            float rot = Projectile.velocity.ToRotation();
            int count = 60;

            Dust.NewDust(p, 0, 0, DustID.BlueFlare);
            for (int i = 0; i <= count; i++)
            {
                float off = MathHelper.PiOver2 - MathHelper.Pi * i / count;
                float r = rot - off;
                Dust dust = Dust.NewDustDirect(p + new Vector2(MathF.Cos(r), MathF.Sin(r)) * 120, 0, 0, DustID.BlueFlare);
                dust.velocity = Projectile.velocity * MathF.Abs(MathF.Cos(off));
            }
        }

        public override bool PreKill(int timeLeft)
        {

            Item[] preItems = new Item[Main.item.Length];
            Main.item.CopyTo(preItems, 0);

            box.SwapTo();
            for (int j = 5; j < 20; j++)
            {
                for (int i = 5; i < 20; i++)
                {
                    if (new Vector2(i - 12, j - 12).LengthSquared() > 7 * 7)
                        continue;
                    WorldGen.KillTile(i, j);

                }
            }
                Tilebox.mainTilemap.SwapTo();

            Matrix toWorld = 
                Matrix.CreateTranslation(-200, -200, 0)
                *
                Matrix.CreateRotationZ(Projectile.rotation)
                *
                Matrix.CreateTranslation(Projectile.Center.Augment())
                ;

            for (int x = 0; x < Main.item.Length; x++)
            {
                Item item = Main.item[x];
                if (item != preItems[x] && !item.IsAir)
                {
                    item.Center = item.Center.Transform(toWorld);
                }
            }

            return true;
        }

        public override void AI()
        {
            //Projectile.rotation += 0.05f;
            //Projectile.velocity.Y += 0.01f;
        }
    }
}
