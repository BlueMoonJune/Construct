using Terraria.ModLoader;
using Terraria;
using Terraria.IO;
using Construct.Contraptions;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace Construct
{
    public class Construct : Mod
    {

        public override void Load()
        {
            On_Main.DoDraw_WallsTilesNPCs += On_Main_DoDraw_WallsTilesNPCs;
            On_WorldFile.SaveWorld += On_WorldFile_SaveWorld;
            On_Player.TileInteractionsCheck += On_Player_TileInteractionsCheck;
            On_Lighting.GetColor_int_int += On_Lighting_GetColor_int_int;
            On_Lighting.AddLight_int_int_float_float_float += On_Lighting_AddLight_int_int_float_float_float;

        }

        private void On_Lighting_AddLight_int_int_float_float_float(On_Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float r, float g, float b)
        {
            Point point = new Point(i, j);
            Vector2 center = new Vector2(i * 16 + 8, j * 16 + 8);
            center = center.Transform((Contraption.contraptionTransform));
            point = (center / 16).ToPoint();
            orig(point.X, point.Y, r, g, b);
        }

        private Microsoft.Xna.Framework.Color On_Lighting_GetColor_int_int(On_Lighting.orig_GetColor_int_int orig, int x, int y)
        {
            Vector2 center = new Vector2(x * 16 + 8, y * 16 + 8);
            center = center.Transform(Contraption.contraptionTransform);
            var point = (center / 16).ToPoint();
            return orig(point.X, point.Y);
        }

        private void On_Player_TileInteractionsCheck(On_Player.orig_TileInteractionsCheck orig, Player self, int myX, int myY)
        {
            //orig(self, myX, myY);
            orig(self, myX, myY);
        }

        private void On_WorldFile_SaveWorld(On_WorldFile.orig_SaveWorld orig)
        {

            Tilebox active = Tilebox.activeTilemap;
            Tilebox.canSwap = false;
            Tilebox.mainTilemap.SwapTo();
            orig();
            Tilebox.canSwap = true;
            active?.SwapTo();
        }

        private void On_Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
        {
            Main.spriteBatch.End();
            foreach (var (p, te) in TileEntity.ByPosition)
            {
                if (te is IRenderedTileEntity rte)
                {
                    rte.PreDraw();
                }
            }
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            orig(self);
        }


    }
}
