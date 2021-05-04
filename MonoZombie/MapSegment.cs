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
    public class MapSegment
    {
        public Tile TileAtVertex { get; set; }
        public bool Permanent { get; set; }
        public int TotalDistance { get; set; }
        public MapSegment PreviousNode { get; set; }

        //A* stuff
        public int GValue { get; set; } //How far away this node is from the source node
        public int HValue { get; set; } //How far away this node is from the end node
        public int FValue { get { return GValue + HValue; } }

        public MapSegment(Tile tileAtThisVertex)
        {
            Permanent = false;
            TotalDistance = 0;
            TileAtVertex = tileAtThisVertex;
            PreviousNode = null;
            GValue = 0;
            HValue = 0;
        }
    }
}
