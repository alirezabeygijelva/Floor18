using Floor18.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Solver
{
    public class GraphBuilder
    {
        public Graph BuildGraphFromText(string filePath)
        {
            var graph = new Graph();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(':');
                var roomName = parts[0].Trim();
                var adjacentRooms = parts[1].Split(',');

                var roomVertex = graph.GetOrCreateVertex(roomName);

                foreach (var adjacentRoom in adjacentRooms)
                {
                    var adjacentVertex = graph.GetOrCreateVertex(adjacentRoom.Trim());
                    graph.AddEdge(roomVertex, adjacentVertex);
                }
            }

            return graph;
        }
    }
}
