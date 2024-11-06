using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Entities
{
    public class Vertex
    {
        public string Id { get; set; }
        public List<Edge> Edges { get; set; }

        public Vertex(string id)
        {
            Id = id;
            Edges = new List<Edge>();
        }

        public bool IsAdjacentTo(Vertex other)
        {
            foreach (var edge in Edges)
            {
                if (edge.Start == other || edge.End == other)
                    return true;
            }
            return false;
        }
    }

}
