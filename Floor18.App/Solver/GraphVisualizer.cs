using Floor18.App.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor18.App.Solver
{

    public class GraphVisualizer
    {
        private VectorGraph vectorGraph;
        private int canvasWidth = 500;
        private int canvasHeight = 500;
        private int nodeRadius = 20;
        private Dictionary<string, PointF> nodePositions = new Dictionary<string, PointF>();
        private Random random = new Random();

        public GraphVisualizer(VectorGraph vectorGraph)
        {
            this.vectorGraph = vectorGraph;
            InitializeNodePositions();
            ApplyForceDirectedLayout();
        }

        public void DrawGraph(string outputPath)
        {
            using (var bitmap = new Bitmap(canvasWidth, canvasHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            using (var pen = new Pen(Color.Black, 2))
            {
                graphics.Clear(Color.White);

                // رسم یال‌ها
                foreach (var edge in vectorGraph.VectorEdges)
                {
                    if (nodePositions.TryGetValue(edge.StartId, out var start) &&
                        nodePositions.TryGetValue(edge.EndId, out var end))
                    {
                        graphics.DrawLine(pen, start, end);
                    }
                }

                // رسم رئوس
                foreach (var node in nodePositions)
                {
                    var position = node.Value;
                    var rect = new RectangleF(position.X - nodeRadius, position.Y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                    graphics.FillEllipse(Brushes.Black, rect);
                    graphics.DrawEllipse(pen, rect);

                    // حذف کلمه "Room" از ابتدای نام
                    var displayName = node.Key.Replace("Room", ""); // یا استفاده از Substring در صورت نیاز
                    var textPosition = new PointF(position.X - nodeRadius / 1.5f, position.Y - nodeRadius / 1.5f);
                    graphics.DrawString(displayName, font, Brushes.White, textPosition);
                }

                bitmap.Save(outputPath);
            }
        }

        private void InitializeNodePositions()
        {
            var uniqueNodes = new HashSet<string>(vectorGraph.VectorEdges.SelectMany(e => new[] { e.StartId, e.EndId }));
            foreach (var nodeId in uniqueNodes)
            {
                float x = random.Next(nodeRadius, canvasWidth - nodeRadius);
                float y = random.Next(nodeRadius, canvasHeight - nodeRadius);
                nodePositions[nodeId] = new PointF(x, y);
            }
        }

        private void ApplyForceDirectedLayout()
        {
            int iterations = 1000;
            float attractionForce = 0.01f;
            float repulsionForce = 1000f;

            for (int i = 0; i < iterations; i++)
            {
                foreach (var nodeA in nodePositions.Keys)
                {
                    foreach (var nodeB in nodePositions.Keys)
                    {
                        if (nodeA == nodeB) continue;

                        var posA = nodePositions[nodeA];
                        var posB = nodePositions[nodeB];

                        float dx = posA.X - posB.X;
                        float dy = posA.Y - posB.Y;
                        float distance = Math.Max((float)Math.Sqrt(dx * dx + dy * dy), 0.1f);

                        float force = repulsionForce / (distance * distance);
                        posA.X += (dx / distance) * force;
                        posA.Y += (dy / distance) * force;

                        nodePositions[nodeA] = posA;
                    }
                }

                foreach (var edge in vectorGraph.VectorEdges)
                {
                    var posA = nodePositions[edge.StartId];
                    var posB = nodePositions[edge.EndId];

                    float dx = posB.X - posA.X;
                    float dy = posB.Y - posA.Y;
                    float distance = Math.Max((float)Math.Sqrt(dx * dx + dy * dy), 0.1f);

                    float force = attractionForce * distance;
                    posA.X += (dx / distance) * force;
                    posA.Y += (dy / distance) * force;
                    posB.X -= (dx / distance) * force;
                    posB.Y -= (dy / distance) * force;

                    nodePositions[edge.StartId] = posA;
                    nodePositions[edge.EndId] = posB;
                }
            }
        }
    }
}