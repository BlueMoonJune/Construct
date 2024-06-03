using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Construct.Util;

namespace Construct.Power
{
    public abstract class Component
    {

        public static ListDictionary<Point, Component> byPosition = new();

        public TileEntity owner;

        public Point Position
        {
            get => new(x, y);
            set
            {
				if (byPosition._dict.ContainsKey(Position))
					byPosition.Remove(Position, this);
                (x, y) = (value.X, value.Y);
                byPosition.Add(Position, this);
            }
        }
        public int x;
        public int y;

        public List<Component> connections = new();
		public float advantage = 1;
        public float rotation;
		public float RPM => _network.rpm / advantage;
		public float Force {
			get => network.force * advantage;
			set => network.force = value / advantage;
		}
        public PowerNetwork _network = new();
		public PowerNetwork network {
			get => _network;
			set {
				_network.components.Remove(this);
				if (_network.components.Count == 0) {
					PowerNetwork.networks.Remove(_network);
				}
				_network = value;
				_network.components.Add(this);
			}
		}

        public virtual void DrawPostTiles()
        {

        }

        public virtual void DrawPreTiles()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnCreate()
        {

        }

        public abstract float GetAdvantageFactor(Component other);

        public void Connect(Component target)
        {
            if (target.network != network)
            {
                float advMul;
                PowerNetwork larger;
                PowerNetwork smaller;
                if (target.network.components.Count > network.components.Count)
                {
                    larger = target.network;
                    smaller = network;
                    advMul = GetAdvantageFactor(target) * target.advantage / advantage;
				}
                else
                {
                    larger = network;
                    smaller = target.network;
                    advMul = target.GetAdvantageFactor(this) * advantage / target.advantage;
                }
                while (smaller.components.Count > 0)
				{
					Component comp = smaller.components[0];
					comp.network = larger;
                    comp.advantage *= advMul;
                }

            }
            connections.Add(target);
            target.connections.Add(this);
        }
    }
}
