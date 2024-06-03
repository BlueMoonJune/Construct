using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Construct.Templates
{
    public abstract class EntityTile : ModTile
    {
        public static Dictionary<Type, EntityTile> te2Tile = new();
        public static Dictionary<int, Type> tileType2TE = new();
        public static Dictionary<int, EntityTile> tileType2Tile = new();

        public int width = 1;
        public int height = 1;
        public int oriX = -1;
        public int oriY = -1;

        public Point16 GetTopLeft(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            i -= tile.TileFrameX / 18 % width;
            j -= tile.TileFrameY / 18 % height;
            return new Point16(i, j);
        }
    }

    public abstract class EntityTile<T> : EntityTile where T : ModTileEntity
    {

        public sealed override void SetStaticDefaults()
        {
            Console.WriteLine($"Loading EntityTile<{typeof(T)}>");
            te2Tile.TryAdd(typeof(T), this);
            tileType2TE.Add(Type, typeof(T));
            tileType2Tile.Add(Type, this);

            Main.tileFrameImportant[Type] = true;

            PreStaticDefaults();

            if (width != 1 || height != 1)
            {
                TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
                TileObjectData.newTile.Width = width;
                TileObjectData.newTile.Height = height;
                TileObjectData.newTile.CoordinateHeights = new int[height];
                for (int i = 0; i < height; i++)
                {
                    TileObjectData.newTile.CoordinateHeights[i] = 16;
                }
                if (oriX < 0) oriX = width / 2;
                if (oriY < 0) oriY = height / 2;
                TileObjectData.newTile.Origin = new Point16(oriX, oriY);

                ModifyTileObjectData();
                TileObjectData.addTile(Type);
            }
        }

        public virtual void PreStaticDefaults() { }

        public virtual void ModifyTileObjectData()
        {
            TileObjectData.newTile.LavaDeath = false;
        }

        public T GetTileEntity(int i, int j)
        {
            TileEntity.ByPosition.TryGetValue(GetTopLeft(i, j), out TileEntity te);
            return te as T;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            Main.NewText($"Placed {typeof(T)}");
            Point16 p = GetTopLeft(i, j);
            ModContent.GetInstance<T>().Place(p.X, p.Y);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<T>().Kill(i, j);
        }
    }
}
