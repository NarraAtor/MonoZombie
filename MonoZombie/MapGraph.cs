using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
            foreach(MapSegment vertex in MapSegmentList)
            {
                Debug.WriteLine($"{vertex.TileAtVertex.X} {vertex.TileAtVertex.Y}");
            }
        }
    }
}
