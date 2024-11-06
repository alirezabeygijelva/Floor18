using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Entities
{
    public class Graph
    {
        public Dictionary<string, Vertex> Vertices { get; set; } = new Dictionary<string, Vertex>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Vertex GetOrCreateVertex(string id)
        {
            if (!Vertices.ContainsKey(id))
            {
                Vertices[id] = new Vertex(id);
            }
            return Vertices[id];
        }

        public void AddEdge(Vertex v1, Vertex v2)
        {
            if (v1 != v2 && !v1.IsAdjacentTo(v2))
            {
                Edge edge = new Edge(v1, v2);
                v1.Edges.Add(edge);
                v2.Edges.Add(edge);
                Edges.Add(edge);
            }
        }
        #region [-Validation-]
        public bool IsPTP()
        {
            if (!HasValidCorners())
            {
                Console.WriteLine("The graph does not have valid corner vertices for RDG conversion.");
                return false;
            }

            foreach (var vertex in Vertices.Values)
            {
                if (vertex.Edges.Count < 2)
                {
                    Console.WriteLine($"Vertex {vertex.Id} does not meet the minimum edge requirement for PTP.");
                    return false;
                }
            }

           
            if (ContainsInvalidCycles())
            {
                Console.WriteLine("The graph contains invalid cycles for PTP.");
                return false;
            }

            return true;
        }
        public bool ContainsInvalidCycles()
        {
            foreach (var vertex in Vertices.Values)
            {
                if (vertex.Edges.Count == 3) 
                {
                    Console.WriteLine($"Invalid cycle found at vertex {vertex.Id}.");
                    return true;
                }
            }
            return false;
        }
        public bool HasValidCorners()
        {
            if (Vertices.Count < 4)
            {
                Console.WriteLine("The graph does not have enough vertices to form four corners.");
                return false;
            }

           
            return true;
        }
        #endregion
    }
}
