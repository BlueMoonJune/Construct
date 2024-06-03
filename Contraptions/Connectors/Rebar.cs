using Construct.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace Construct.Contraptions.Connectors
{
    public class RebarTile : ModTile, ITileConnector
    {

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
        }

        public ushort CheckDirection(int i, int j, Direction dir)
        {
            ushort val = CheckDirection(new(i, j), dir);
            return val;

        }
        public ushort CheckDirection(Point p, Direction dir)
        {
            ushort i = 0;
            while (i < 64)
            {
                i++;
                p += dir;
                if (p.X < 0 || p.X > Main.maxTilesX || p.Y < 0 || p.Y > Main.maxTilesY) return 0;

                if (!Main.tile[p].HasTile)
                {
                    return 0;
                }

                if (ModContent.GetModTile(Main.tile[p].type) is RebarTile)
                {
                    return i;
                }
            }
            return 0;
        }

        public Rectangle GetFrame(int i, int j)
        {

            Rectangle frame = new(0, 0, 16, 16);
            if (CheckDirection(i, j, Direction.Right) > 0)
            {
                frame.X += 16;
            }
            if (CheckDirection(i, j, Direction.Left) > 0)
            {
                frame.X += 32;
            }
            if (CheckDirection(i, j, Direction.Down) > 0)
            {
                frame.Y += 16;
            }
            if (CheckDirection(i, j, Direction.Up) > 0)
            {
                frame.Y += 32;
            }
            return frame;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var frame = GetFrame(i, j);
            Vector2 TileOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 pos = new Vector2(i, j) * 16 - Main.screenPosition + TileOffset;
            Texture2D tex = Main.instance.TilesRenderer.GetTileDrawTexture(Main.tile[i, j], i, j);
            Main.spriteBatch.Draw(tex, pos, frame, Lighting.GetColor(i, j));
            return false;
        }

        public Point[] GetConnectedTiles(int i, int j)
        {
            Point o = new Point(i, j);

            var dists = new ushort[4];
            ushort sum = 0;
            for (int k = 0; k < 4; k++)
            {
                ushort v = CheckDirection(i, j, k);
                sum += v;
                dists[k] = v;
            }

            Point[] ret = new Point[sum];

            ushort c = 0;
            for (int k = 0; k < 4; k++)
            {
                var p = o;
                Direction dir = k;
                for (int n = 0; n < dists[k]; n++)
                {
                    ret[c++] = p += dir;
                }
            }

            return ret;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            Item playerItem;
            if (!Main.mouseItem.IsAir)
                playerItem = Main.mouseItem;
            else
                playerItem = player.HeldItem;

            if (playerItem.type == ItemID.StoneBlock)
            {
                player.noThrow = 2;
                player.cursorItemIconText = "Encase";
            }
        }

        public override bool RightClick(int i, int j)
        {

            Item playerItem;
            if (!Main.mouseItem.IsAir)
                playerItem = Main.mouseItem;
            else
                playerItem = Main.player[Main.myPlayer].HeldItem;

            if (playerItem.type == ItemID.StoneBlock)
            {
                Main.tile[i, j].type = (ushort)ModContent.TileType<EncasedRebar>();
                WorldGen.SquareTileFrame(i, j);
                playerItem.stack--;
                if (playerItem.stack <= 0) playerItem.TurnToAir();
                return true;
            }
            return false;
        }
    }

    public class RebarItem : TileItem<RebarTile>;
}
