using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Solver
{
    public class VectorEdge
    {
        public string StartId { get; }
        public string EndId { get; }

        public VectorEdge(string startId, string endId)
        {
            StartId = startId;
            EndId = endId;
        }

        public override string ToString() => $"{StartId} -> {EndId}";
    }
}
