using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using System.Reflection;
using MonoMod.Core.Platforms;
using MonoMod.Utils;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.Utilities.Terraria.Utilities;
using Terraria.ModLoader;

namespace Construct
{
    public struct TileEntityStaticFields
    {
        public Dictionary<Point16, TileEntity> byPosition;
        public Dictionary<int, TileEntity> byID;
        public TileEntitiesManager manager;
        public int nextID;

        public TileEntityStaticFields()
        {
            byPosition = new();
            byID = new();
            manager = new();
        }

        public static TileEntityStaticFields GetCurrent()
        {
            return new()
            {
                byPosition = TileEntity.ByPosition,
                byID = TileEntity.ByID,
                manager = TileEntity.manager,
                nextID = TileEntity.TileEntitiesNextID
            };
        }

        public void Apply()
        {
            TileEntity.ByPosition = byPosition;
            TileEntity.ByID = byID;
            TileEntity.manager = manager;
            TileEntity.TileEntitiesNextID = nextID;
        }
    }

    public unsafe class Tilebox : IDisposable
    {
        public static void ModifyTileMapSize(ushort width, ushort height)
        {
            Main.maxTilesX = width - 1; Main.maxTilesY = height - 1;
            fixed (Terraria.Tilemap* tilemapPtr_unused = &Main.tile)
            {
                var tilemapPtr = (ushort*)tilemapPtr_unused;

                ref ushort w = ref tilemapPtr[0];
                ref ushort h = ref tilemapPtr[1];

                w = width;
                h = height;
            }
        }

        private static Tilebox _active;

        public static Tilebox activeTilemap
        {
            get { if (_active == null) InitMainTilemap(); return _active; }
            set => _active = value;
        }

        private static Tilebox _main;

        public static Tilebox mainTilemap 
        {
            get { if (_main == null) InitMainTilemap(); return _main; }
            set => _main = value;
        }

        public static bool canSwap = true;
        public static bool IsMainActive => activeTilemap == mainTilemap;
        public bool IsMain => this == mainTilemap;

        public readonly ushort width;

        public readonly ushort height;

        private bool init = false;
        public Dictionary<Type, IntPtr> pointers = new();
        public TileEntityStaticFields tileEntities = new();
        public Chest[] chest = new Chest[8000];


        public static void InitMainTilemap()
        {
            _main = new Tilebox(Main.tile.Width, Main.tile.Height, true);
            _main.tileEntities = TileEntityStaticFields.GetCurrent();
            _main.chest = Main.chest;
            activeTilemap = _main;
        }



        public Tilebox(ushort width, ushort height, bool main = false)
        {
            this.width = width; this.height = height;


            Type tileData = typeof(TileData);
            Action action = (Action)tileData.GetField("OnClearEverything", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            var delegates = action.GetInvocationList();
            List<Type> types = new();
            foreach (var entry in delegates)
            {
                Type tileDataType = entry.Method.DeclaringType;
                if (main)
                {
                    pointers[tileDataType] = new IntPtr(Pointer.Unbox(tileDataType.GetProperty("ptr").GetValue(null)));
                } else
                {
                    IntPtr ptr = Marshal.AllocHGlobal(width * height * tileDataType.GetManagedSize());
                    pointers[tileDataType] = ptr;
                    for (int i = 0; i < width * height * tileDataType.GetManagedSize(); i++)
                    {
                        *(byte*)(ptr + i) = 0;
                    }
                }
            }
        }

        public TagCompound SaveData()
        {
            

            TagCompound teData = new TagCompound();
            foreach (var (id, te) in tileEntities.byID)
            {
                TagCompound tag = new TagCompound();
                te.SaveData(tag);
                teData.Add(id.ToString(), tag);
            }

            TagCompound chestData = new TagCompound();
            for (int i = 0; i < chest.Length; i++)
            {
                Chest c = chest[i];
                if (c == null) continue;
                chestData.Add(i.ToString(), new TagCompound()
                {
                    {"x", c.x},
                    {"y", c.y},
                    {"name", c.name},
                    {"item", c.item}
                });
            }


            TagCompound tileData = new();

            foreach (var (type, ptr) in pointers)
            {

                Type dataType = type.GetGenericArguments()[0];
                int size = dataType.GetManagedSize();
                byte[] data = new byte[size * width * height];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = ((byte*)ptr)[i];
                }

                tileData.Add(dataType.FullName, data);
            }

            return new TagCompound
            {
                { "width", width },
                { "height", height },
                { "tiles", tileData },
                { "tileEntities", teData },
                { "chest", chestData }
            };
        }

        public static Tilebox LoadData(TagCompound tag)
        {
            var ret = new Tilebox(tag.Get<ushort>("width"), tag.Get<ushort>("height"));
            var tileData = tag.Get<TagCompound>("tiles");
            foreach (var (typeName, dataUnk) in tileData)
            {
                byte[] data = (byte[])dataUnk;
                Type type = null;
                Type dataType = null;
                object[] objs = [.. ModLoader.Mods, Main.instance];
                foreach (object obj in objs)
                {
                    var assembly = Assembly.GetAssembly(obj.GetType());
                    if (assembly.GetType(typeName) is Type t)
                    {
                        type = tileDataDef.MakeGenericType(dataType = t);
                        break;
                    }
                }
                if (type is null)
                {
                    continue;
                }
                IntPtr ptr;
                if (!ret.pointers.TryGetValue(type, out ptr))
                {
                    ret.pointers[type] = ptr = Marshal.AllocHGlobal(data.Length);
                }
                ptr = ret.pointers[type];
                for (int i = 0; i < data.Length; i++)
                {
                    ((byte*)ptr)[i] = data[i];
                }
            }

            return ret;
        }

        public BoxedTile this[int i, int j]
        {
            get => new BoxedTile((uint)(i * height + j), this);
        }

        public void CopyTo(Point from, Tilebox target, Point to)
        {
            CopyTo(from.X, from.Y, target, to.X, to.Y);
        }
        
        public void CopyTo(int fromX, int fromY, Tilebox target, int toX, int toY)
        {
            int fid = fromX * height + fromY;
            int tid = toX * target.height + toY;

            foreach (var (type, ptr) in pointers)
            {
                CopyToType(fid, target, tid, type);
            }
        }

        public static Type tileDataDef = typeof(TileData<TileTypeData>).GetGenericTypeDefinition();

        public void CopyToWhitelist(int fromX, int fromY, Tilebox target, int toX, int toY, params Type[] types)
        {
            int fid = fromX * height + fromY;
            int tid = toX * target.height + toY;
            foreach (Type type in types)
            CopyToType(fid, target, tid, tileDataDef.MakeGenericType(type));
        }

        public void CopyToBlacklist(int fromX, int fromY, Tilebox target, int toX, int toY, params Type[] types)
        {
            int fid = fromX * height + fromY;
            int tid = toX * target.height + toY;
            foreach (Type type in types)
                CopyToType(fid, target, tid, tileDataDef.MakeGenericType(type));
        }

        private void CopyToType(int fid, Tilebox target, int tid, Type type)
        {
            IntPtr ptr;
            if (!pointers.TryGetValue(type, out ptr))
                return;
            IntPtr tPtr;
            if (!target.pointers.TryGetValue(type, out tPtr))
                return;

            Type dataType = type.GetGenericArguments()[0];
            int size = dataType.GetManagedSize();

            Main.NewText($"Copying over {type.GetGenericArguments()[0]} ({size} bytes)");
            int sfid = fid * size;
            int stid = tid * size;
            for (int i = 0; i < size; i++)
            {
                byte data = Marshal.ReadByte(ptr + sfid + i);
                Marshal.WriteByte(tPtr + stid + i, data);
            }
        }

        public void SwapTo()
        {
            if (!canSwap && !IsMain) return;

            if (IsMainActive)
            {
                InitMainTilemap();
            }

            activeTilemap.tileEntities = TileEntityStaticFields.GetCurrent();
            activeTilemap.chest = Main.chest;
            tileEntities.Apply();

            foreach (var (type, data) in pointers)
            {
                PropertyInfo dataPointer = type.GetProperty("ptr");
                dataPointer.SetValue(null, data);
            }

            Main.chest = chest;
            activeTilemap = this;
            ModifyTileMapSize(width, height);
        }

        public void Dispose()
        {
            foreach (var (type, pointer) in pointers)
            {
                Marshal.FreeHGlobal(pointer);
            }
            GC.SuppressFinalize(this);
        }
    }
}
