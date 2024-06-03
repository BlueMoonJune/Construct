using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Construct.Templates;
using Terraria.ModLoader;
using Construct.Power.Components;
using Terraria.DataStructures;

namespace Construct.Power.Generators
{
    public class InfinityGeneratorTE : ComponentTileEntity
    {
		public float force = 0;
		public float rpm = 0;

        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.active() && tile.type == ModContent.TileType<InfinityGenerator>();
        }

		public Pulley drivePulley;

        public override void OnPlace()
        {
            drivePulley = new(1, 0);
			AddComponent(drivePulley, new(1, 0));
        }

        public override void Update()
        {
			base.Update();
			
			float rpmDif = rpm - drivePulley.RPM;
			if (Math.Abs(rpmDif) > 1) {
				drivePulley.Force += Math.Sign(rpmDif) * force;
			} else {
				drivePulley.Force += rpmDif * force;
			}
            
        }
    } 
	public class InfinityGenerator : EntityTile<InfinityGeneratorTE> {
        public override void PreStaticDefaults()
        {
            width = 3;
			height = 2;
        }

        public override bool RightClick(int i, int j)
        {
			InfinityGeneratorTE te = GetTileEntity(i, j);
            Point16 subtile = new Point16(i, j) - GetTopLeft(i, j);
			if (subtile.X == 1) return false;
			int change = 0;
			if (subtile.X == 0) {
				change = -1;
			} else if (subtile.X == 2) {
				change = 1;
			}

			if (subtile.Y == 0) {
				te.force += change;
			} else {
				te.rpm += change;
			}
			return true;
        }

        public override void MouseOver(int i, int j)
        {
            
			InfinityGeneratorTE te = GetTileEntity(i, j);
            Point16 subtile = new Point16(i, j) - GetTopLeft(i, j);
			var player = Main.LocalPlayer;
			player.cursorItemIconText = "";
			if (subtile.X == 1) {
				return;
			}
			player.cursorItemIconEnabled = true;

			if (subtile.Y == 0) {
				player.cursorItemIconText += $"Force: {te.force} ";
			} else {
				player.cursorItemIconText += $"RPM: {te.drivePulley.RPM:0.00}/{te.rpm} ";
			}

			if (subtile.X == 0) {
				player.cursorItemIconText += "-1";
			} else if (subtile.X == 2) {
				player.cursorItemIconText += "+1";
			}

        }



    

    
    
	}
	public class InfinityGeneratorItem : TileItem<InfinityGenerator> {

	}
}
