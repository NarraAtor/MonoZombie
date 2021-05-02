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
        public List<MapSegment> MapSegmentList { get; private set; }
        public Tile[,] TileMatrix { get; private set; }
        private Dictionary<MapSegment, List<MapSegment>> adjacencyDictionary;
        public MapGraph(Tile[,] tileMatrix)
        {

            //Create the list of MapSegments
            MapSegmentList = new List<MapSegment>();
            adjacencyDictionary = new Dictionary<MapSegment, List<MapSegment>>(tileMatrix.GetLength(0));
            TileMatrix = tileMatrix;
            for(int x = 0; x < tileMatrix.GetLength(0); x++)
            {
                for(int y = 0; y < tileMatrix.GetLength(1); y++)
                {
                    if(tileMatrix[x, y].IsWalkable)
                    MapSegmentList.Add(new MapSegment(tileMatrix[x, y]));
                }
            }

            //handle adjacency
            for(int x = 0; x < tileMatrix.GetLength(0); x++)
            {
                for(int y = 0; y < tileMatrix.GetLength(1); y++)
                {
                    if(!TileMatrix[x ,y].IsWalkable)
                    {
                        continue;
                    }
                    MapSegment currentVertex = FindMapSegmentFromPosition(x, y);
                    //if this MapSegment is not on the edges of the graph
                    //TODO: check for MapSegments that were next to walls
                    if (x != 0 && x != tileMatrix.GetLength(0) - 1)
                    {
                        if (y != 0 && y != tileMatrix.GetLength(1) - 1)
                        {
                            //Get the verticies in the cardinal directions around this vertex
                            List<MapSegment> currentVertexAdjacencies = new List<MapSegment>();
                            currentVertexAdjacencies.Add(FindMapSegmentFromPosition(x , y - 1));
                            currentVertexAdjacencies.Add(FindMapSegmentFromPosition(x, y + 1));
                            currentVertexAdjacencies.Add(FindMapSegmentFromPosition(x + 1, y));
                            currentVertexAdjacencies.Add(FindMapSegmentFromPosition(x - 1, y));
                            adjacencyDictionary.Add(currentVertex, currentVertexAdjacencies);
                        }
                    }
                }
            }
            //TODO: Make adjacency matrix and list.

            //test if this worked
            //foreach(MapSegment vertex in MapSegmentList)
            //{
            //    Debug.WriteLine($"{vertex.TileAtVertex.X} {vertex.TileAtVertex.Y}");
            //}

            //Test if the adjacencyDictionary is working correctly
            for (int x = 0; x < TileMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < TileMatrix.GetLength(1); y++)
                {
                    if (!TileMatrix[x, y].IsWalkable)
                    {
                        continue;
                    }
                    Console.WriteLine($"MapSegment at {FindMapSegmentFromPosition(x, y).TileAtVertex.X},{FindMapSegmentFromPosition(x, y).TileAtVertex.Y}  ");

                    foreach(MapSegment vertex in adjacencyDictionary[FindMapSegmentFromPosition(x, y)])
                    {
                       // Console.Write($"{vertex.TileAtVertex.X}, {vertex.TileAtVertex.Y} |");
                    }
                    //Console.WriteLine($"{adjacencyDictionary[FindMapSegmentFromPosition(x, y)]}");
                }
            }
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

        /// <summary>
        /// Purpose: Finds the vertex that the selected zombie is in.
        /// Restrictions:
        /// Issue: the values returned from this method change when the player moves even if zombies are stuck against a wall.
        ///        This issue is most likely caused from the camera function we have. 
        ///        I'm not sure if this actually is a problem considering the fact that the tiles may be moving too.
        /// </summary>
        /// <param name="zombie">the zombie being implemented in this function</param>
        /// <returns>the vertex the zombie is in</returns>
        public MapSegment GetZombieVertex(Enemy zombie)
        {
            //loop through the list of verticies and use Rectangle.Intersects with the tiles they contain.
            //Find the tile with the biggest intersection.
            //Return the vertex that contains that tile.
            Rectangle biggestRectangle = new Rectangle(0, 0, 0, 0);
            MapSegment zombieCurrentMapSegment = null;
            foreach (MapSegment vertex in MapSegmentList)
            {
                //Rectangle zombieCameraBasedRectangle = new Rectangle(zombie.CamX, zombie.CamY, zombie.Rect.Width, zombie.Rect.Height);
                int currentRectangleArea = Rectangle.Intersect(zombie.Rect, vertex.TileAtVertex.Rect).Width * Rectangle.Intersect(zombie.Rect, vertex.TileAtVertex.Rect).Height;
                //If the area of of the biggestRectangle is smaller than the intersection between the player and the current tile...
                if ((biggestRectangle.Width * biggestRectangle.Height) < currentRectangleArea)
                {
                    biggestRectangle = Rectangle.Intersect(zombie.Rect, vertex.TileAtVertex.Rect);
                    zombieCurrentMapSegment = vertex;
                }

            }
            return zombieCurrentMapSegment;
        }

        /// <summary>
        /// Purpose: Helper method that returns a MapSegment based on a tile value.
        /// </summary>
        /// <param name="x">x value of tile</param>
        /// <param name="y">y value of tile</param>
        /// <returns>the map segment with tile at location x,y </returns>
        private MapSegment FindMapSegmentFromPosition(int x, int y)
        {
            MapSegment mapSegment = null;
            foreach (MapSegment vertex in MapSegmentList)
            {
                if (vertex.TileAtVertex.Position == TileMatrix[x, y].Position)
                {
                    mapSegment = vertex;
                }
            }

            return mapSegment;
        }
    }
}
