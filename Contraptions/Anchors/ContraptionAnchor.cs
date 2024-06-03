using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Construct.Power;

namespace Construct.Contraptions.Anchors
{
    public abstract class ContraptionAnchor : ComponentTileEntity
    {
        public Contraption contraption = null;
        public virtual Point Target => Position.ToPoint();
        public bool Assembled => contraption != null;

        public abstract bool Assemble();

        public abstract bool Disassemble();

        public virtual void DrawMount() { }

        public override void SaveData(TagCompound tag)
        {
            if (contraption != null)
            {
                tag.Add("box", contraption.box.SaveData());
                tag.Add("pivot", contraption.pivot);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            /*
            contraption = new Contraption()
            {
                pivot = tag.Get<Vector2>("pivot"),
                box = Tilebox.LoadData(tag.Get<TagCompound>("box"))
            };
            */
        }
    }
}
