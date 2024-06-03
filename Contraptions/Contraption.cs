using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria;
using Construct.Configs;
using Terraria.DataStructures;
using Construct.Contraptions.Connectors;
using Terraria.Graphics.Light;

namespace Construct.Contraptions
{

    public class Contraption
    {
        public static Matrix contraptionTransform = Matrix.Identity;

        public Tilebox box;

        public Vector3 rotation;
        public Vector2 pivot;
        public Vector2 position;

        public static readonly Point padding = new Point(10, 10);

        public Matrix transform
        {
            get => Matrix.CreateTranslation(-pivot.Augment())
                * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z)
                * Matrix.CreateTranslation(position.Augment());
        }

        public static (Point[], Point, Point) GetConnectedTiles(int i, int j)
        {
            Point max = new Point(i, j);
            Point min = new Point(i, j);

            HashSet<Point> tiles = new();
            List<Point> found = new() { new Point(i, j) };
            while (found.Count > 0)
            {
                for (int n = 0; n < found.Count;)
                {
                    var point = found.Pop();
                    if (tiles.Contains(point)) continue;
                    tiles.Add(point);

                    if (point.X > max.X) max.X = point.X;
                    if (point.Y > max.Y) max.Y = point.Y;
                    if (point.X < min.X) min.X = point.X;
                    if (point.Y < min.Y) min.Y = point.Y;

                    ModTile mt = ModContent.GetModTile(Main.tile[point].type);
                    if (mt is ITileConnector ct)
                    {
                        foreach (var p in ct.GetConnectedTiles(point.X, point.Y))
                        {
                            found.Add(p);
                        }
                    }
                    ModWall mw = ModContent.GetModWall(Main.tile[point].wall);
                    if (mw is ITileConnector cw)
                    {
                        foreach (var p in cw.GetConnectedTiles(point.X, point.Y))
                        {
                            found.Add(p);
                        }
                    }

                    Point TL = HelperMethods.GetTopLeftTileInMultitile(point.X, point.Y, out int width, out int height);
                    if (width != 1 || height != 1)
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                found.Add(TL + new Point(x, y));
                            }
                        }
                }
            }
            return (tiles.ToArray(), min, max);
        }

        public static void PostDrawTiles_Filtered()
        {
            foreach (var system in SystemLoader.HookPostDrawTiles.Enumerate())
            {
                string name = system.FullName;
                bool allowed = SystemConfig.instance.classList.Contains(name) == SystemConfig.instance.classWhitelist;
                //Main.NewText(name);
                if (allowed)
                {
                    system.PostDrawTiles();
                }
            }
        }

        public Contraption(Point start, Point anchor)
        {
            var (tiles, min, max) = GetConnectedTiles(start.X, start.Y);
            min -= padding;
            pivot = (anchor - min).ToVector2() * 16 + Vector2.One * 8;
            max += padding;
            box = new((ushort)(max.X - min.X), (ushort)(max.Y - min.Y));
            foreach (var tile in tiles)
            {
                if (tile == anchor) continue;
                Point local = tile - min;
                Tilebox.activeTilemap.CopyTo(tile, box, local);
                if (TileEntity.ByPosition.TryGetValue(new(tile.X, tile.Y), out TileEntity te))
                {
                    int id = box.tileEntities.nextID++;
                    Point16 pos = new(local.X, local.Y);

                    TileEntity.ByID.Remove(te.ID);
                    TileEntity.ByPosition.Remove(te.Position);

                    box.tileEntities.byID[id] = te;
                    te.ID = id;
                    box.tileEntities.byPosition[pos] = te;
                    te.Position = pos;
                    Main.NewText($"Copying TE {te.GetType().Name}");
                }

                int chest = Chest.FindChest(tile.X, tile.Y);
                if (chest > 0)
                {
                    for (int i = 0; i < box.chest.Length; i++)
                    {
                        if (box.chest[i] is null)
                        {
                            box.chest[i] = Main.chest[i];
                            Chest.DestroyChestDirect(tile.X, tile.Y, chest);
                            box.chest[i].x = local.X;
                            box.chest[i].y = local.Y;
                            break;
                        }
                    }
                }

            }
            foreach (var tile in tiles)
            {
                if (tile == anchor) continue;
                Tile t = Main.tile[tile];
                t.ClearEverything();
            }
            foreach (var tile in tiles)
            {
                WorldGen.SquareTileFrame(tile.X, tile.Y);
            }

            Tilebox old = Tilebox.activeTilemap;
            box.SwapTo();
            foreach (var tile in tiles)
            {
                WorldGen.SquareTileFrame(tile.X - min.X, tile.Y - min.Y);
            }
            old.SwapTo();
        }
        public Contraption()
        {

        }
        public void Disassemble()
        {
            Point p = ((position - pivot) / 16).ToPoint();

            for (int j = padding.Y; j < box.height - padding.Y + 1; j++)
            {
                for (int i = padding.X; i < box.width - padding.Y + 1; i++)
                {
                    int wx = p.X + i;
                    int wy = p.Y + j;
                    Point local = new Point(i, j);
                    BoxedTile myTile = box[i, j];
                    if (!myTile.HasTile) continue;
                    WorldGen.KillTile(wx, wy);
                    if (myTile.wall != 0) WorldGen.KillWall(wx, wy);
                    box.CopyTo(local, Tilebox.activeTilemap, p + local);
                }
            }
            for (int j = padding.Y; j < box.height - padding.Y + 1; j++)
            {
                for (int i = padding.X; i < box.width - padding.Y + 1; i++)
                {
                    int wx = p.X + i;
                    int wy = p.Y + j;
                    WorldGen.SquareTileFrame(wx, wy);
                }
            }
            box.Dispose();
        }

        public void DrawWall(int i, int j)
        {

            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.wall == 0) return;
            Texture2D tex = Main.instance.WallsRenderer.GetTileDrawTexture(tile, i, j);
            Rectangle framing = new Rectangle(tile.WallFrameX, tile.WallFrameY, 32, 32);
            Main.spriteBatch.Draw(tex, new Vector2(i * 16 - 8, j * 16 - 8), framing, Lighting.GetColor(i, j));
        }

        public void DrawTile(int i, int j)
        {

            Tile tile = Main.tile[i, j];
            if (!tile.HasTile) return;

            if (Main.tileLighted[tile.type])
            {
                new TileLightScanner().GetTileLight(i, j, out Vector3 col);
                Lighting.AddLight(i, j, col.X, col.Y, col.Z);
            }

            if (TileLoader.PreDraw(i, j, tile.type, Main.spriteBatch) && !tile.IsTileInvisible)
            {
                //Main.instance.TilesRenderer.DrawSingleTile(Main.instance.TilesRenderer._currentTileDrawInfo.Value, true, 0, Main.screenPosition, new(Main.offScreenRange, Main.offScreenRange), j, i);

                Color color = Color.White;
                TileLoader.PreDraw(i, j, tile.type, Main.spriteBatch);

                if (tile.IsActuated) color = tile.actColor(color);

                color = color.MultiplyRGB(Lighting.GetColor(i, j));

                Texture2D tex = Main.instance.TilesRenderer.GetTileDrawTexture(tile, i, j);
                Rectangle framing = new Rectangle(tile.frameX, tile.frameY, 16, 16);

                switch (tile.BlockType)
                {
                    case Terraria.ID.BlockType.Solid:
                        Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16), framing, color);
                        break;
                    case Terraria.ID.BlockType.HalfBlock:
                        Tile bTile = Framing.GetTileSafely(i, j + 1);
                        if (!bTile.HasTile || bTile.BlockType == Terraria.ID.BlockType.HalfBlock || !Main.tileSolid[bTile.type])
                        {
                            framing.Height = 4;
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 + 8), framing, color);
                            framing = new Rectangle(9 * 18, 3 * 18 + 12, 16, 4);
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 + 12), framing, color);
                        }
                        else
                        {
                            framing.Height = 8;
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 + 8), framing, color);
                        }
                        break;
                    case Terraria.ID.BlockType.SlopeDownLeft:
                        framing.Width = 2;
                        framing.Height = 14;
                        for (int n = 0; n < 7; n++)
                        {
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16 + 2 * n, j * 16 + 2 * n), framing, color);
                            framing.X += 2;
                            framing.Height -= 2;
                        }
                        framing.X -= 14;
                        framing.Y += 14;
                        framing.Width = 16;
                        framing.Height = 2;
                        Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 + 14), framing, color);
                        break;
                    case Terraria.ID.BlockType.SlopeDownRight:
                        framing.Width = 2;
                        framing.Height = 2;
                        framing.X += 2;
                        for (int n = 0; n < 7; n++)
                        {
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16 + 2 * n + 2, j * 16 + 12 - 2 * n), framing, color);
                            framing.X += 2;
                            framing.Height += 2;
                        }
                        framing.X -= 16;
                        framing.Y += 14;
                        framing.Width = 16;
                        framing.Height = 2;
                        Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16 + 14), framing, color);
                        break;
                    case Terraria.ID.BlockType.SlopeUpLeft:
                        framing.Width = 2;
                        framing.Height = 14;
                        framing.Y += 2;
                        for (int n = 0; n < 7; n++)
                        {
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16 + 2 * n, j * 16 + 2), framing, color);
                            framing.X += 2;
                            framing.Height -= 2;
                            framing.Y += 2;
                        }
                        framing.X -= 14;
                        framing.Y -= 16;
                        framing.Width = 16;
                        framing.Height = 2;
                        Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16), framing, color);
                        break;
                    case Terraria.ID.BlockType.SlopeUpRight:
                        framing.Width = 2;
                        framing.Height = 2;
                        framing.Y += 14;
                        framing.X += 2;
                        for (int n = 0; n < 7; n++)
                        {
                            Main.spriteBatch.Draw(tex, new Vector2(i * 16 + 2 * n + 2, j * 16 + 2), framing, color);
                            framing.X += 2;
                            framing.Height += 2;
                            framing.Y -= 2;
                        }
                        framing.X -= 16;
                        framing.Width = 16;
                        framing.Height = 2;
                        Main.spriteBatch.Draw(tex, new Vector2(i * 16, j * 16), framing, color);
                        break;
                }
                /**/
            }

            TileLoader.PostDraw(i, j, tile.type, Main.spriteBatch);
        }

        public void Draw()
        {

            contraptionTransform *= transform;

            Main.GameViewMatrix._transformationMatrix = transform * Matrix.CreateTranslation(-(Main.screenPosition).Augment()) * Main.Transform;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.Transform
            );

            var oldScreenPos = Main.screenPosition;
            var oldOffscreen = Main.offScreenRange;
            Main.screenPosition = Vector2.Zero;
            Main.offScreenRange = 0;

            Tilebox old = Tilebox.activeTilemap;
            box.SwapTo();

            for (int j = padding.Y; j < box.height - padding.Y + 1; j++)
            {
                for (int i = padding.X; i < box.width - padding.Y + 1; i++)
                {
                    DrawWall(i, j);
                }
            }

            for (int j = padding.Y; j < box.height - padding.Y + 1; j++)
            {
                for (int i = padding.X; i < box.width - padding.Y + 1; i++)
                {
                    DrawTile(i, j);
                }
            }

            Main.spriteBatch.End();

            PostDrawTiles_Filtered();

            old.SwapTo();

            contraptionTransform *= Matrix.Invert(transform);

            Main.screenPosition = oldScreenPos;
            Main.offScreenRange = oldOffscreen;
            Main.GameViewMatrix._transformationMatrix = Matrix.CreateTranslation((Main.screenPosition).Augment()) * Matrix.Invert(transform) * Main.Transform;

        }

        public void Update()
        {
            contraptionTransform *= transform;
            Tilebox old = Tilebox.activeTilemap;
            box.SwapTo();
            foreach (var (id, te) in box.tileEntities.byID) { te.Update(); }
            SystemLoader.PostUpdateWorld();
            old.SwapTo();
            contraptionTransform *= Matrix.Invert(transform);
        }


    }
}
