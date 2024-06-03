using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Construct.Contraptions.Connectors
{
    public class EncasedRebar : RebarTile
    {
        public override string Texture => "Construct/Contraptions/Connectors/SmoothStone";

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
            Tile tile = Main.tile[i, j];
            tile.type = (ushort)ModContent.TileType<RebarTile>();
            base.PreDraw(i, j, spriteBatch);
            tile.type = (ushort)ModContent.TileType<EncasedRebar>();
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || effectOnly) return;

            fail = true;
            if (!noItem)
                Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemID.StoneBlock);
            Main.tile[i, j].type = (ushort)ModContent.TileType<RebarTile>();
        }
    }
}
