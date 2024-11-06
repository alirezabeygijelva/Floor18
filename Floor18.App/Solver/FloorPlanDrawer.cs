using Floor18.App.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;

namespace Floor18.App.Solver
{
    public class FloorPlanDrawer
    {
        private Graph graph;
        private int canvasWidth = 300;
        private int canvasHeight = 400;
        private int padding = 2;  // فاصله بین اتاق‌ها

        public FloorPlanDrawer(Graph graph)
        {
            this.graph = graph;
        }

        public void Draw(string outputPath)
        {
            using (var bitmap = new Bitmap(canvasWidth, canvasHeight))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var font = new System.Drawing.Font("Arial", 10))
            using (var pen = new Pen(Color.Black, 2))
            using (var brush = new SolidBrush(Color.LightBlue))
            {
                graphics.Clear(Color.White);

                // چیدمان اتاق‌ها با توجه به هم‌جواری‌ها و تنظیم ابعاد
                var positions = GenerateRoomPositions();

                foreach (var vertex in graph.Vertices.Values)
                {
                    if (positions.TryGetValue(vertex.Id, out var roomInfo))
                    {
                        var rect = new Rectangle(roomInfo.Position.X, roomInfo.Position.Y, roomInfo.Width, roomInfo.Height);
                        graphics.FillRectangle(brush, rect);
                        graphics.DrawRectangle(pen, rect);

                        // نمایش نام اتاق
                        var textSize = graphics.MeasureString(vertex.Id, font);
                        var textPosition = new PointF(
                            roomInfo.Position.X + (roomInfo.Width - textSize.Width) / 2,
                            roomInfo.Position.Y + (roomInfo.Height - textSize.Height) / 2
                        );
                        graphics.DrawString(vertex.Id, font, Brushes.Black, textPosition);
                    }
                }

                bitmap.Save(outputPath);
            }
        }

        private Dictionary<string, RoomInfo> GenerateRoomPositions()
        {
            var positions = new Dictionary<string, RoomInfo>();

            int maxRows = 3;
            int maxCols = 3;
            int cellWidth = canvasWidth / maxCols;
            int cellHeight = canvasHeight / maxRows;

            // برای شروع چیدمان اتاق‌ها، به ترتیب هم‌جواری حرکت می‌کنیم
            var queue = new Queue<Vertex>();
            var startVertex = graph.Vertices.Values.First();
            queue.Enqueue(startVertex);
            positions[startVertex.Id] = new RoomInfo(new Point(0, 0), cellWidth, cellHeight);

            var visited = new HashSet<string> { startVertex.Id };

            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();
                var currentRoomInfo = positions[currentVertex.Id];

                int neighborIndex = 0;

                foreach (var edge in currentVertex.Edges)
                {
                    var neighbor = edge.End;
                    if (visited.Contains(neighbor.Id)) continue;

                    Point newPosition = currentRoomInfo.Position;
                    int newWidth = cellWidth;
                    int newHeight = cellHeight;

                    // تنظیم موقعیت همسایه‌ها بر اساس جهت و هم‌جواری
                    switch (neighborIndex % 4)
                    {
                        case 0: // راست
                            newPosition.Offset(currentRoomInfo.Width, 0);
                            newWidth = cellWidth;
                            break;
                        case 1: // پایین
                            newPosition.Offset(0, currentRoomInfo.Height);
                            newHeight = cellHeight;
                            break;
                        case 2: // چپ
                            newPosition.Offset(-cellWidth, 0);
                            newWidth = cellWidth;
                            break;
                        case 3: // بالا
                            newPosition.Offset(0, -cellHeight);
                            newHeight = cellHeight;
                            break;
                    }

                    // اضافه کردن اتاق همسایه به لیست موقعیت‌ها
                    positions[neighbor.Id] = new RoomInfo(newPosition, newWidth, newHeight);
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor.Id);

                    neighborIndex++;
                }
            }

            return positions;
        }


    }

    public class RoomInfo
    {
        public Point Position { get; }
        public int Width { get; }
        public int Height { get; }

        public RoomInfo(Point position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }


}

