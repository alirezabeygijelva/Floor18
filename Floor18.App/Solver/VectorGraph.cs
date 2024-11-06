using Floor18.App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Solver
{
    public class VectorGraph
    {
        public List<VectorEdge> VectorEdges { get; set; } = new List<VectorEdge>();

        public VectorGraph(Graph graph)
        {
            foreach (var edge in graph.Edges)
            {
                VectorEdges.Add(new VectorEdge(edge.Start.Id, edge.End.Id));
            }
        }
    }

  
}
