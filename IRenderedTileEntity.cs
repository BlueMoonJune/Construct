using System;
using Terraria.DataStructures;

namespace Construct
{
    public interface IRenderedTileEntity
    {
        public TileEntity TE { get { if (this is TileEntity te) return te; throw new InvalidCastException("A class implementing IRenderedTileEntity is not a TileEntity. Only TileEntity subclasses should implement IRenderedTileEntity."); } }
        public void PostDraw() { }
        public void PreDraw() { }
    }
}
