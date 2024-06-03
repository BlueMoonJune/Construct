using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Construct.Power
{
    public abstract class ComponentTileEntity : ModTileEntity, IRenderedTileEntity
    {
        public Dictionary<Point, Component> components = new();
        private bool _callOnPlace = true;

        public virtual void PreDraw()
        {
            foreach (var (pos, comp) in components)
            {
                comp?.DrawPreTiles();
            }
        }

        public virtual void PostDraw()
        {
            foreach (var (pos, comp) in components)
            {
                comp?.DrawPostTiles();
            }
        }

        public override void Update()
        {
            if (_callOnPlace)
            {
                OnPlace();
                _callOnPlace = false;
            }
            foreach (var (pos, comp) in components)
            {
				Point position = Position.ToPoint() + pos;
				if (position != comp.Position) {
					Main.NewText($"Updating position of Component ({pos}) from {comp.Position} to {position}");
					comp.Position = position;
				}
                comp?.Update();
            }
        }

        public virtual void OnPlace()
        {

        }

        public override void OnKill()
        {
            foreach (var (pos, comp) in components)
            {
                Component.byPosition.Remove(comp.Position, comp);
                comp.network.components.Remove(comp);
                if (comp.network.components.Count == 0)
                {
                    PowerNetwork.networks.Remove(comp.network);
                }
            }
        }



        public void AddComponent(Component comp, Point position)
        {
            components.Add(position, comp);
            position = Position.ToPoint() + position;
            Component.byPosition.Add(position, comp);
            comp.network.components.Add(comp);
            comp.owner = this;
            comp.Position = position;
            comp.OnCreate();
        }
    }
}
