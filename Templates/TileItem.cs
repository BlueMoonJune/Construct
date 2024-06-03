using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Construct.Templates
{
    public class TileItem<T> : ModItem where T : ModTile
    {
        public sealed override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<T>();

            Item.width = 16; // The item texture's width
            Item.height = 16; // The item texture's height

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;

            Item.maxStack = 9999;
            Item.consumable = true;

            PostDefaults();
        }

        public virtual void PostDefaults() { }
    }
}
