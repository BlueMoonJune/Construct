using Construct.Templates;
using Construct.Power.Components;
using Terraria;
using Terraria.ModLoader;
namespace Construct.Power.Transfer
{
    public class AxleTE : ComponentTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.active() && tile.type == ModContent.TileType<Axle>();
        }

        public override void OnPlace()
        {
			Main.NewText("Axle Placed");
            AddComponent(new Pulley(Main.rand.Next(0,3), Main.rand.Next(0,3)), new(0, 0));
        }

        public override void Update()
        {
            base.Update();
        }
    }

    public class Axle : EntityTile<AxleTE>
    {
        public override void PreStaticDefaults()
        {

            width = 1;
            height = 1;

        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || effectOnly) return;
            ModContent.GetInstance<AxleTE>().Kill(i, j);
        }
    }

    public class AxleItem : TileItem<Axle>
    {

    }
}
