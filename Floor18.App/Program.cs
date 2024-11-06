using Floor18.App.Entities;
using Floor18.App.Solver;

string filePath = "input.txt"; 

GraphBuilder builder = new GraphBuilder();
Graph graph = builder.BuildGraphFromText(filePath);
if (!graph.IsPTP())
{
    Console.WriteLine("The input graph cannot be converted to a PTP graph.");
    return;
}


VectorGraph vectorGraph = new VectorGraph(graph);

foreach (var vectorEdge in vectorGraph.VectorEdges)
{
    Console.WriteLine(vectorEdge);
}

FloorPlanDrawer drawer = new FloorPlanDrawer(graph);
drawer.Draw("FloorPlanOutput.png");

Console.WriteLine("Floor plan saved as 'FloorPlanOutput.png'");


var visualizer = new GraphVisualizer(vectorGraph);
visualizer.DrawGraph("output.png");