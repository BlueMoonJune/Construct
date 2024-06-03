using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Construct.Contraptions.Connectors
{
    public class RivetedWall : ModWall, ITileConnector
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;

            DustType = DustID.Silver;

            AddMapEntry(new Color(150, 150, 150));
        }

        public Point[] GetConnectedTiles(int i, int j)
        {
            i--;
            j--;
            var ret = new Point[9];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    ret[x + y * 3] = new Point(x + i, y + j);
                }
            }
            return ret;
        }
    }

    public class RivetedWallItem : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<RivetedWall>());
        }
    }
}