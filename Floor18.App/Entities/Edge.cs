using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Entities
{
    public class Edge
    {
        public Vertex Start { get; }
        public Vertex End { get; }

        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        public override string ToString() => $"{Start.Id} -- {End.Id}";
    }
}
