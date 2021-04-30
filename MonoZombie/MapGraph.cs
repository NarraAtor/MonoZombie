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
    /// Author: Eric
    /// Purpose: Holds all of the map segements as a graph. Intended to help with pathfinding.
    /// Restrictions:
    /// </summary>
    class MapGraph
    {
        public List<MapSegment> MapSegmentList { get; set; }
        public MapGraph(Tile[,] tileMatrix)
        {
            //Create the list of MapSegments
            MapSegmentList = new List<MapSegment>();
            for(int x = 0; x < tileMatrix.GetLength(0); x++)
            {
                for(int y = 0; y < tileMatrix.GetLength(1); y++)
                {
                    MapSegmentList.Add(new MapSegment(tileMatrix[x, y]));
                }
            }
            
            //test if this worked
            //foreach(MapSegment vertex in MapSegmentList)
            //{
            //    Debug.WriteLine($"{vertex.TileAtVertex.X} {vertex.TileAtVertex.Y}");
            //}
        }

        /// <summary>
        /// Purpose: Finds the vertex of the tile the player is in or mostly in.
        /// Restrictions:
        /// </summary>
        /// <returns> Returns the vertex the player is in or mostly in.</returns>
        public MapSegment GetPlayerVertex()
        {
            //loop through the list of verticies and use Rectangle.Intersects with the tiles they contain.
            //Find the tile with the biggest intersection.
            //Return the vertex that contains that tile.
            Rectangle biggestRectangle = new Rectangle(0, 0, 0, 0);
            MapSegment playerCurrentMapSegment = null;
            foreach(MapSegment vertex in MapSegmentList)
            {
                int currentRectangleArea = Rectangle.Intersect(Main.Player.Rect, vertex.TileAtVertex.Rect).Width * (Rectangle.Intersect(Main.Player.Rect, vertex.TileAtVertex.Rect).Height);
                //If the area of of the biggestRectangle is smaller than the intersection between the player and the current tile...
                if ((biggestRectangle.Width * biggestRectangle.Height) < currentRectangleArea)
                {
                    biggestRectangle = Rectangle.Intersect(Main.Player.Rect, vertex.TileAtVertex.Rect);
                    playerCurrentMapSegment = vertex;
                }

            }
            return playerCurrentMapSegment;
        }
    }
}
