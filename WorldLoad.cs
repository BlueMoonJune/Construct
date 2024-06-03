using Terraria;
using Terraria.ModLoader;

namespace Construct
{
    internal class WorldLoad : ModSystem
    {
        public override void OnWorldLoad()
        {
            Main.NewText("Woah!");
            //Tilebox.ModifyTileMapSize((ushort)(Main.maxTilesX + 1), (ushort)(Main.maxTilesY + 1));
        }
    }
}
