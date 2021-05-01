using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    /// <summary>
    /// Author: Eric Fotang
    /// Purpose: A vertex of the MapGraph. Intended to be used in pathfinding.
    /// Restrictions:
    /// </summary>
    class MapSegment
    {
        public bool Permanent { get; set; }
        public int TotalDistance { get; set; }
        public Tile TileAtVertex { get; set; }
        public MapSegment PreviousNode { get; set; }

        public MapSegment(Tile tileAtThisVertex)
        {
            Permanent = false;
            TotalDistance = 0;
            TileAtVertex = tileAtThisVertex;
            PreviousNode = null;
        }
    }
}
