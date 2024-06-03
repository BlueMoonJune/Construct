using Construct.Contraptions.Anchors;
using Construct.Contraptions.Connectors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Construct.Contraptions
{
    internal class ContraptionSystem : ModSystem
    {
        public static int recursionLimiter = 0;

        public static void FixTiles()
        {

            Vector2 zero2 = Vector2.Zero;
            if (Main.drawToScreen)
            {
                zero2 = Vector2.Zero;
            }
            int num12 = (int)((Main.screenPosition.X - zero2.X) / 16f - 1f);
            int num13 = (int)((Main.screenPosition.X + (float)Main.screenWidth + zero2.X) / 16f) + 2;
            int num14 = (int)((Main.screenPosition.Y - zero2.Y) / 16f - 1f);
            int num15 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + zero2.Y) / 16f) + 5;
            if (num12 < 0)
            {
                num12 = 0;
            }
            if (num13 > Main.maxTilesX)
            {
                num13 = Main.maxTilesX;
            }
            if (num14 < 0)
            {
                num14 = 0;
            }
            if (num15 > Main.maxTilesY)
            {
                num15 = Main.maxTilesY;
            }
            Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();

            for (int j = num14 + screenOverdrawOffset.Y; j < num15 - screenOverdrawOffset.Y; j++)
            {
                for (int i = num12 + screenOverdrawOffset.X; i < num13 - screenOverdrawOffset.X; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.frameX == -1 && tile.frameY == -1)
                    {
                        WorldGen.TileFrame(i, j, false, true);
                    }
                }
            }
        }

        public static void ManualDrawWalls()
        {
            Vector2 zero2 = Vector2.Zero;
            if (Main.drawToScreen)
            {
                zero2 = Vector2.Zero;
            }
            int num12 = (int)((Main.screenPosition.X - zero2.X) / 16f - 1f);
            int num13 = (int)((Main.screenPosition.X + (float)Main.screenWidth + zero2.X) / 16f) + 2;
            int num14 = (int)((Main.screenPosition.Y - zero2.Y) / 16f - 1f);
            int num15 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + zero2.Y) / 16f) + 5;
            if (num12 < 0)
            {
                num12 = 0;
            }
            if (num13 > Main.maxTilesX)
            {
                num13 = Main.maxTilesX;
            }
            if (num14 < 0)
            {
                num14 = 0;
            }
            if (num15 > Main.maxTilesY)
            {
                num15 = Main.maxTilesY;
            }
            Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            for (int j = num14 + screenOverdrawOffset.Y; j < num15 - screenOverdrawOffset.Y; j++)
            {
                for (int i = num12 + screenOverdrawOffset.X; i < num13 - screenOverdrawOffset.X; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.wall == 0) continue;
                    Texture2D tex = Main.instance.WallsRenderer.GetTileDrawTexture(tile, i, j);
                    Rectangle framing = new Rectangle(tile.WallFrameX, tile.WallFrameY, 32, 32);
                    Main.spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X - 8, j * 16 - (int)Main.screenPosition.Y - 8), framing, Lighting.GetColor(i, j).MultiplyRGB(Color.Gray));
                }
            }

            Main.spriteBatch.End();
        }

        public static void ManualDraw()
        {


            Vector2 zero2 = Vector2.Zero;
            if (Main.drawToScreen)
            {
                zero2 = Vector2.Zero;
            }
            int num12 = (int)((Main.screenPosition.X - zero2.X) / 16f - 1f);
            int num13 = (int)((Main.screenPosition.X + (float)Main.screenWidth + zero2.X) / 16f) + 2;
            int num14 = (int)((Main.screenPosition.Y - zero2.Y) / 16f - 1f);
            int num15 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + zero2.Y) / 16f) + 5;
            if (num12 < 0)
            {
                num12 = 0;
            }
            if (num13 > Main.maxTilesX)
            {
                num13 = Main.maxTilesX;
            }
            if (num14 < 0)
            {
                num14 = 0;
            }
            if (num15 > Main.maxTilesY)
            {
                num15 = Main.maxTilesY;
            }
            Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            for (int j = num14 + screenOverdrawOffset.Y; j < num15 - screenOverdrawOffset.Y; j++)
            {
                for (int i = num12 + screenOverdrawOffset.X; i < num13 - screenOverdrawOffset.X; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.HasTile) continue;
                    Texture2D tex = TextureAssets.Tile[tile.type].Value;
                    Rectangle framing = new Rectangle(tile.frameX, tile.frameY, 16, 16);
                    Main.spriteBatch.Draw(tex, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y), framing, Lighting.GetColor(i, j).MultiplyRGB(Color.Gray));
                }
            }

            Main.spriteBatch.End();
        }

        public static void PreDrawTiles()
        {

            FixTiles();
            ManualDrawWalls();
            ManualDraw();

            /*Rendering
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.instance.DoDraw_WallsAndBlacks();
            Main.instance.DoDraw_Tiles_NonSolid();
            Main.spriteBatch.End();
            Main.instance.DoDraw_Tiles_Solid();

            Overlays.Scene.Draw(Main.spriteBatch, RenderLayers.TilesAndNPCs);
            /**/
        }

        public override void PostDrawTiles()
        {
            if (recursionLimiter > 256)
            {
                return;
            }
            recursionLimiter++;
            foreach (var (pos, te) in TileEntity.ByID)
            {
                if (te is ContraptionAnchor anch)
                {
                    anch.contraption?.Draw();
                }
                if (te is IRenderedTileEntity ren)
                {
                    ren.PostDraw();
                }
            }
            recursionLimiter--;
            if (recursionLimiter == 0)
            {
                Point mousepos = ((Main.MouseScreen.Transform(Matrix.Invert(Main.Transform)) + Main.screenPosition) / 16).ToPoint();
                if (ModContent.GetModTile(Main.tile[mousepos].type) is ITileConnector || ModContent.GetModWall(Main.tile[mousepos].wall) is ITileConnector)
                {
                    Main.spriteBatch.Begin(
                        SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        Main.DefaultSamplerState,
                        DepthStencilState.None,
                        Main.Rasterizer,
                        null,
                        Main.Transform
                    );

                    HashSet<Point> tiles = [.. Contraption.GetConnectedTiles(mousepos.X, mousepos.Y).Item1];
                    foreach (var p in tiles)
                    {
                        if (!Main.tile[p].HasTile && Main.tile[p].wall == 0)
                            tiles.Remove(p);
                    }

                    foreach (var p in tiles)
                    {
                        Rectangle frame = new(0, 0, 16, 16);
                        if (tiles.Contains(p + new Point(1, 0)))
                        {
                            frame.X += 18;
                        }
                        if (tiles.Contains(p - new Point(1, 0)))
                        {
                            frame.X += 36;
                        }
                        if (tiles.Contains(p + new Point(0, 1)))
                        {
                            frame.Y += 18;
                        }
                        if (tiles.Contains(p - new Point(0, 1)))
                        {
                            frame.Y += 36;
                        }

                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Construct/ConnectionOverlay").Value, p.ToVector2() * 16 - Main.screenPosition, frame, Color.White);
                    }
                    Main.spriteBatch.End();
                }
            }
        }

        public override void PostUpdateEverything()
        {
            foreach (var (pos, te) in TileEntity.ByID)
            {
                if (te is ContraptionAnchor anch)
                {
                    anch.contraption?.Update();
                }
            }
        }
    }
}
