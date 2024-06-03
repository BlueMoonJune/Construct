using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace Construct
{
    public struct BoxedTile
    {
        internal readonly uint TileId;
        public Tilebox owner;

        public ref ushort TileType => ref Get<TileTypeData>().Type;

        public ref ushort WallType => ref Get<WallTypeData>().Type;

        public bool HasTile
        {
            get
            {
                return Get<TileWallWireStateData>().HasTile;
            }
            set
            {
                Get<TileWallWireStateData>().HasTile = value;
            }
        }

        public bool IsActuated
        {
            get
            {
                return Get<TileWallWireStateData>().IsActuated;
            }
            set
            {
                Get<TileWallWireStateData>().IsActuated = value;
            }
        }

        public bool HasActuator
        {
            get
            {
                return Get<TileWallWireStateData>().HasActuator;
            }
            set
            {
                Get<TileWallWireStateData>().HasActuator = value;
            }
        }

        public bool HasUnactuatedTile
        {
            get
            {
                if (HasTile)
                {
                    return !IsActuated;
                }
                return false;
            }
        }

        public SlopeType Slope
        {
            get
            {
                return Get<TileWallWireStateData>().Slope;
            }
            set
            {
                Get<TileWallWireStateData>().Slope = value;
            }
        }

        public BlockType BlockType
        {
            get
            {
                return Get<TileWallWireStateData>().BlockType;
            }
            set
            {
                Get<TileWallWireStateData>().BlockType = value;
            }
        }

        public bool IsHalfBlock
        {
            get
            {
                return Get<TileWallWireStateData>().IsHalfBlock;
            }
            set
            {
                Get<TileWallWireStateData>().IsHalfBlock = value;
            }
        }

        public bool TopSlope
        {
            get
            {
                if (Slope != SlopeType.SlopeDownLeft)
                {
                    return Slope == SlopeType.SlopeDownRight;
                }
                return true;
            }
        }

        public bool BottomSlope
        {
            get
            {
                if (Slope != SlopeType.SlopeUpLeft)
                {
                    return Slope == SlopeType.SlopeUpRight;
                }
                return true;
            }
        }

        public bool LeftSlope
        {
            get
            {
                if (Slope != SlopeType.SlopeDownRight)
                {
                    return Slope == SlopeType.SlopeUpRight;
                }
                return true;
            }
        }

        public bool RightSlope
        {
            get
            {
                if (Slope != SlopeType.SlopeDownLeft)
                {
                    return Slope == SlopeType.SlopeUpLeft;
                }
                return true;
            }
        }

        public ref short TileFrameX => ref Get<TileWallWireStateData>().TileFrameX;

        public ref short TileFrameY => ref Get<TileWallWireStateData>().TileFrameY;

        public int WallFrameX
        {
            get
            {
                return Get<TileWallWireStateData>().WallFrameX;
            }
            set
            {
                Get<TileWallWireStateData>().WallFrameX = value;
            }
        }

        public int WallFrameY
        {
            get
            {
                return Get<TileWallWireStateData>().WallFrameY;
            }
            set
            {
                Get<TileWallWireStateData>().WallFrameY = value;
            }
        }

        public int TileFrameNumber
        {
            get
            {
                return Get<TileWallWireStateData>().TileFrameNumber;
            }
            set
            {
                Get<TileWallWireStateData>().TileFrameNumber = value;
            }
        }

        public int WallFrameNumber
        {
            get
            {
                return Get<TileWallWireStateData>().WallFrameNumber;
            }
            set
            {
                Get<TileWallWireStateData>().WallFrameNumber = value;
            }
        }

        public byte TileColor
        {
            get
            {
                return Get<TileWallWireStateData>().TileColor;
            }
            set
            {
                Get<TileWallWireStateData>().TileColor = value;
            }
        }

        public byte WallColor
        {
            get
            {
                return Get<TileWallWireStateData>().WallColor;
            }
            set
            {
                Get<TileWallWireStateData>().WallColor = value;
            }
        }

        public ref byte LiquidAmount => ref Get<LiquidData>().Amount;

        public int LiquidType
        {
            get
            {
                return Get<LiquidData>().LiquidType;
            }
            set
            {
                Get<LiquidData>().LiquidType = value;
            }
        }

        public bool SkipLiquid
        {
            get
            {
                return Get<LiquidData>().SkipLiquid;
            }
            set
            {
                Get<LiquidData>().SkipLiquid = value;
            }
        }

        public bool CheckingLiquid
        {
            get
            {
                return Get<LiquidData>().CheckingLiquid;
            }
            set
            {
                Get<LiquidData>().CheckingLiquid = value;
            }
        }

        public bool RedWire
        {
            get
            {
                return Get<TileWallWireStateData>().RedWire;
            }
            set
            {
                Get<TileWallWireStateData>().RedWire = value;
            }
        }

        public bool GreenWire
        {
            get
            {
                return Get<TileWallWireStateData>().GreenWire;
            }
            set
            {
                Get<TileWallWireStateData>().GreenWire = value;
            }
        }

        public bool BlueWire
        {
            get
            {
                return Get<TileWallWireStateData>().BlueWire;
            }
            set
            {
                Get<TileWallWireStateData>().BlueWire = value;
            }
        }

        public bool YellowWire
        {
            get
            {
                return Get<TileWallWireStateData>().YellowWire;
            }
            set
            {
                Get<TileWallWireStateData>().YellowWire = value;
            }
        }

        public bool IsTileInvisible
        {
            get
            {
                return Get<TileWallBrightnessInvisibilityData>().IsTileInvisible;
            }
            set
            {
                Get<TileWallBrightnessInvisibilityData>().IsTileInvisible = value;
            }
        }

        public bool IsWallInvisible
        {
            get
            {
                return Get<TileWallBrightnessInvisibilityData>().IsWallInvisible;
            }
            set
            {
                Get<TileWallBrightnessInvisibilityData>().IsWallInvisible = value;
            }
        }

        public bool IsTileFullbright
        {
            get
            {
                return Get<TileWallBrightnessInvisibilityData>().IsTileFullbright;
            }
            set
            {
                Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = value;
            }
        }

        public bool IsWallFullbright
        {
            get
            {
                return Get<TileWallBrightnessInvisibilityData>().IsWallFullbright;
            }
            set
            {
                Get<TileWallBrightnessInvisibilityData>().IsWallFullbright = value;
            }
        }

        internal ref ushort type => ref TileType;

        internal ref ushort wall => ref WallType;

        internal ref short frameX => ref TileFrameX;

        internal ref short frameY => ref TileFrameY;

        internal ref byte liquid => ref LiquidAmount;

        public object Clone()
        {
            throw new NotImplementedException();
            return MemberwiseClone();
        }

        public void ClearEverything()
        {
            throw new NotImplementedException();
            TileData.ClearSingle(TileId);
        }

        public void ClearTile()
        {
            throw new NotImplementedException();
            slope(0);
            halfBrick(halfBrick: false);
            active(active: false);
            inActive(inActive: false);
        }

        public void CopyFrom(BoxedTile from)
        {
            throw new NotImplementedException();
            TileData.CopySingle(from.TileId, TileId);
        }

        internal bool isTheSameAs(BoxedTile compTile)
        {
            if (Get<TileWallWireStateData>().NonFrameBits != compTile.Get<TileWallWireStateData>().NonFrameBits)
            {
                return false;
            }
            if (wall != compTile.wall || liquid != compTile.liquid)
            {
                return false;
            }
            if (liquid > 0 && LiquidType != compTile.LiquidType)
            {
                return false;
            }
            if (Get<TileWallBrightnessInvisibilityData>().Data != compTile.Get<TileWallBrightnessInvisibilityData>().Data)
            {
                return false;
            }
            if (HasTile)
            {
                if (type != compTile.type)
                {
                    return false;
                }
                if (Main.tileFrameImportant[type] && (frameX != compTile.frameX || frameY != compTile.frameY))
                {
                    return false;
                }
            }
            return true;
        }

        internal int blockType()
        {
            if (halfBrick())
            {
                return 1;
            }
            int num = slope();
            if (num > 0)
            {
                num++;
            }
            return num;
        }

        public void ResetToType(ushort type)
        {
            ClearMetadata();
            HasTile = true;
            TileType = type;
        }

        internal void ClearMetadata()
        {
            Get<LiquidData>() = default(LiquidData);
            Get<TileWallWireStateData>() = default(TileWallWireStateData);
            Get<TileWallBrightnessInvisibilityData>() = default(TileWallBrightnessInvisibilityData);
        }

        internal Color actColor(Color oldColor)
        {
            //IL_0008: Unknown result type (might be due to invalid IL or missing references)
            //IL_003c: Unknown result type (might be due to invalid IL or missing references)
            if (!inActive())
            {
                return oldColor;
            }
            double num = 0.4;
            return new Color((int)(byte)(num * (double)(int)((Color)(oldColor)).R), (int)(byte)(num * (double)(int)((Color)(oldColor)).G), (int)(byte)(num * (double)(int)((Color)(oldColor)).B), (int)((Color)(oldColor)).A);
        }

        internal void actColor(ref Vector3 oldColor)
        {
            //IL_000a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0014: Unknown result type (might be due to invalid IL or missing references)
            //IL_0019: Unknown result type (might be due to invalid IL or missing references)
            if (inActive())
            {
                oldColor *= 0.4f;
            }
        }

        internal bool topSlope()
        {
            byte b = slope();
            if (b != 1)
            {
                return b == 2;
            }
            return true;
        }

        internal bool bottomSlope()
        {
            byte b = slope();
            if (b != 3)
            {
                return b == 4;
            }
            return true;
        }

        internal bool leftSlope()
        {
            byte b = slope();
            if (b != 2)
            {
                return b == 4;
            }
            return true;
        }

        internal bool rightSlope()
        {
            byte b = slope();
            if (b != 1)
            {
                return b == 3;
            }
            return true;
        }

        public void Clear(TileDataType types)
        {
            if ((types & TileDataType.Tile) != 0)
            {
                type = 0;
                active(active: false);
                frameX = 0;
                frameY = 0;
            }
            if ((types & TileDataType.Wall) != 0)
            {
                wall = 0;
                wallFrameX(0);
                wallFrameY(0);
            }
            if ((types & TileDataType.TilePaint) != 0)
            {
                ClearBlockPaintAndCoating();
            }
            if ((types & TileDataType.WallPaint) != 0)
            {
                ClearWallPaintAndCoating();
            }
            if ((types & TileDataType.Liquid) != 0)
            {
                liquid = 0;
                liquidType(0);
                checkingLiquid(checkingLiquid: false);
            }
            if ((types & TileDataType.Slope) != 0)
            {
                slope(0);
                halfBrick(halfBrick: false);
            }
            if ((types & TileDataType.Wiring) != 0)
            {
                wire(wire: false);
                wire2(wire2: false);
                wire3(wire3: false);
                wire4(wire4: false);
            }
            if ((types & TileDataType.Actuator) != 0)
            {
                actuator(actuator: false);
                inActive(inActive: false);
            }
        }

        public static void SmoothSlope(Tilebox owner, int x, int y, bool applyToNeighbors = true, bool sync = false)
        {
            if (applyToNeighbors)
            {
                SmoothSlope(owner, x + 1, y, applyToNeighbors: false, sync);
                SmoothSlope(owner, x - 1, y, applyToNeighbors: false, sync);
                SmoothSlope(owner, x, y + 1, applyToNeighbors: false, sync);
                SmoothSlope(owner, x, y - 1, applyToNeighbors: false, sync);
            }
            BoxedTile tile = owner[x, y];
            if (!WorldGen.CanPoundTile(x, y) || !WorldGen.SolidOrSlopedTile(x, y))
            {
                return;
            }
            bool flag = !WorldGen.TileEmpty(x, y - 1);
            bool flag2 = !WorldGen.SolidOrSlopedTile(x, y - 1) && flag;
            bool flag3 = WorldGen.SolidOrSlopedTile(x, y + 1);
            bool flag4 = WorldGen.SolidOrSlopedTile(x - 1, y);
            bool flag5 = WorldGen.SolidOrSlopedTile(x + 1, y);
            int num = ((flag ? 1 : 0) << 3) | ((flag3 ? 1 : 0) << 2) | ((flag4 ? 1 : 0) << 1) | (flag5 ? 1 : 0);
            bool flag6 = tile.halfBrick();
            int num2 = tile.slope();
            switch (num)
            {
                case 10:
                    if (!flag2)
                    {
                        tile.halfBrick(halfBrick: false);
                        tile.slope(3);
                    }
                    break;
                case 9:
                    if (!flag2)
                    {
                        tile.halfBrick(halfBrick: false);
                        tile.slope(4);
                    }
                    break;
                case 6:
                    tile.halfBrick(halfBrick: false);
                    tile.slope(1);
                    break;
                case 5:
                    tile.halfBrick(halfBrick: false);
                    tile.slope(2);
                    break;
                case 4:
                    tile.slope(0);
                    tile.halfBrick(halfBrick: true);
                    break;
                default:
                    tile.halfBrick(halfBrick: false);
                    tile.slope(0);
                    break;
            }
            if (sync)
            {
                int num3 = tile.slope();
                bool flag7 = flag6 != tile.halfBrick();
                bool flag8 = num2 != num3;
                if (flag7 && flag8)
                {
                    NetMessage.SendData(17, -1, -1, null, 23, x, y, num3);
                }
                else if (flag7)
                {
                    NetMessage.SendData(17, -1, -1, null, 7, x, y, 1f);
                }
                else if (flag8)
                {
                    NetMessage.SendData(17, -1, -1, null, 14, x, y, num3);
                }
            }
        }

        public void CopyPaintAndCoating(BoxedTile other)
        {
            color(other.color());
            invisibleBlock(other.invisibleBlock());
            fullbrightBlock(other.fullbrightBlock());
        }

        public TileColorCache BlockColorAndCoating()
        {
            TileColorCache result = default(TileColorCache);
            result.Color = color();
            result.FullBright = fullbrightBlock();
            result.Invisible = invisibleBlock();
            return result;
        }

        public TileColorCache WallColorAndCoating()
        {
            TileColorCache result = default(TileColorCache);
            result.Color = wallColor();
            result.FullBright = fullbrightWall();
            result.Invisible = invisibleWall();
            return result;
        }

        public void UseBlockColors(TileColorCache cache)
        {
            throw new NotImplementedException();
            //cache.ApplyToBlock(this);
        }

        public void UseWallColors(TileColorCache cache)
        {
            throw new NotImplementedException();
            //cache.ApplyToWall(this);
        }

        public void ClearBlockPaintAndCoating()
        {
            color(0);
            fullbrightBlock(fullbrightBlock: false);
            invisibleBlock(invisibleBlock: false);
        }

        public void ClearWallPaintAndCoating()
        {
            wallColor(0);
            fullbrightWall(fullbrightWall: false);
            invisibleWall(invisibleWall: false);
        }

        public override string ToString()
        {
            return "BoxedTile Type:" + type + " Active:" + active() + " Wall:" + wall + " Slope:" + slope() + " fX:" + frameX + " fY:" + frameY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal BoxedTile(uint tileId, Tilebox owner)
        {
            TileId = tileId;
            this.owner = owner;
        }

        public static Type tileDataBase = typeof(TileData<TileTypeData>).GetGenericTypeDefinition();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>() where T : unmanaged, ITileData
        {
            return ref ((T*)(owner.pointers[tileDataBase.MakeGenericType([typeof(T)])]))[TileId];
        }

        public override int GetHashCode()
        {
            return (int)TileId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoxedTile tile, BoxedTile tile2)
        {
            return tile.TileId == tile2.TileId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoxedTile tile, BoxedTile tile2)
        {
            return tile.TileId != tile2.TileId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BoxedTile tile, ArgumentException justSoYouCanCompareWithNull)
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BoxedTile tile, ArgumentException justSoYouCanCompareWithNull)
        {
            return true;
        }

        internal bool active()
        {
            return HasTile;
        }

        internal void active(bool active)
        {
            HasTile = active;
        }

        internal bool inActive()
        {
            return IsActuated;
        }

        internal void inActive(bool inActive)
        {
            IsActuated = inActive;
        }

        internal bool actuator()
        {
            return HasActuator;
        }

        internal void actuator(bool actuator)
        {
            HasActuator = actuator;
        }

        internal bool nactive()
        {
            return HasUnactuatedTile;
        }

        internal byte slope()
        {
            return (byte)Slope;
        }

        internal void slope(byte slope)
        {
            Slope = (SlopeType)slope;
        }

        internal bool halfBrick()
        {
            return IsHalfBlock;
        }

        internal void halfBrick(bool halfBrick)
        {
            IsHalfBlock = halfBrick;
        }

        internal bool HasSameSlope(BoxedTile tile)
        {
            return Slope == tile.Slope;
        }

        internal int wallFrameX()
        {
            return WallFrameX;
        }

        internal void wallFrameX(int wallFrameX)
        {
            WallFrameX = wallFrameX;
        }

        internal int wallFrameY()
        {
            return WallFrameY;
        }

        internal void wallFrameY(int wallFrameY)
        {
            WallFrameY = wallFrameY;
        }

        internal byte frameNumber()
        {
            return (byte)TileFrameNumber;
        }

        internal void frameNumber(byte frameNumber)
        {
            TileFrameNumber = frameNumber;
        }

        internal byte wallFrameNumber()
        {
            return (byte)WallFrameNumber;
        }

        internal void wallFrameNumber(byte wallFrameNumber)
        {
            WallFrameNumber = wallFrameNumber;
        }

        internal byte color()
        {
            return TileColor;
        }

        internal void color(byte color)
        {
            TileColor = color;
        }

        internal byte wallColor()
        {
            return WallColor;
        }

        internal void wallColor(byte wallColor)
        {
            WallColor = wallColor;
        }

        internal byte liquidType()
        {
            return (byte)LiquidType;
        }

        internal void liquidType(int liquidType)
        {
            LiquidType = liquidType;
        }

        internal bool lava()
        {
            return LiquidType == 1;
        }

        internal void lava(bool lava)
        {
            SetIsLiquidType(1, lava);
        }

        internal bool honey()
        {
            return LiquidType == 2;
        }

        internal void honey(bool honey)
        {
            SetIsLiquidType(2, honey);
        }

        internal bool shimmer()
        {
            return LiquidType == 3;
        }

        internal void shimmer(bool shimmer)
        {
            SetIsLiquidType(3, shimmer);
        }

        internal bool skipLiquid()
        {
            return SkipLiquid;
        }

        internal void skipLiquid(bool skipLiquid)
        {
            SkipLiquid = skipLiquid;
        }

        internal bool checkingLiquid()
        {
            return CheckingLiquid;
        }

        internal void checkingLiquid(bool checkingLiquid)
        {
            CheckingLiquid = checkingLiquid;
        }

        internal bool wire()
        {
            return RedWire;
        }

        internal void wire(bool wire)
        {
            RedWire = wire;
        }

        internal bool wire2()
        {
            return BlueWire;
        }

        internal void wire2(bool wire2)
        {
            BlueWire = wire2;
        }

        internal bool wire3()
        {
            return GreenWire;
        }

        internal void wire3(bool wire3)
        {
            GreenWire = wire3;
        }

        internal bool wire4()
        {
            return YellowWire;
        }

        internal void wire4(bool wire4)
        {
            YellowWire = wire4;
        }

        internal bool invisibleBlock()
        {
            return IsTileInvisible;
        }

        internal void invisibleBlock(bool invisibleBlock)
        {
            IsTileInvisible = invisibleBlock;
        }

        internal bool invisibleWall()
        {
            return IsWallInvisible;
        }

        internal void invisibleWall(bool invisibleWall)
        {
            IsWallInvisible = invisibleWall;
        }

        internal bool fullbrightBlock()
        {
            return IsTileFullbright;
        }

        internal void fullbrightBlock(bool fullbrightBlock)
        {
            IsTileFullbright = fullbrightBlock;
        }

        internal bool fullbrightWall()
        {
            return IsWallFullbright;
        }

        internal void fullbrightWall(bool fullbrightWall)
        {
            IsWallFullbright = fullbrightWall;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetIsLiquidType(int liquidId, bool value)
        {
            if (value)
            {
                LiquidType = liquidId;
            }
            else if (LiquidType == liquidId)
            {
                LiquidType = 0;
            }
        }
    }
}
