using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construct.Contraptions.Connectors
{
    public interface ITileConnector
    {
        public Point[] GetConnectedTiles(int i, int j);
    }
}
