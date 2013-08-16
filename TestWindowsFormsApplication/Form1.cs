using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Temp;
using TravellingSalesmanProblem;

namespace TestWindowsFormsApplication
{
    public partial class TestForm : Form
    {
        private Bitmap m_Bitmap;
        private List<Point2DReal> m_Points = new List<Point2DReal>();

        private int m_Width = 1200;
        private int m_Height = 900;
        private double m_XScale = 1;
        private double m_XShift = 0;
        private double m_YScale = 1;
        private double m_YShift = 0;

        private TspAlgo[] _algos = new[]
            {
                new TspAlgo { Name = "Ant Colony", Solver = new AntColony(), Color = Color.Green},
                new TspAlgo { Name = "Little Algo", Solver = new LittleAlgorithm(), Color = Color.CornflowerBlue },
                new TspAlgo { Name = "Simple Greedy", Solver = new SimpleGreedy(), Color = Color.Orange },
                new TspAlgo { Name = "Greedy 2", Solver = new Greedy2(), Color = Color.DarkGoldenrod },
                new TspAlgo { Name = "Tabu Search", Solver = new TabuSearch(), Color = Color.Orchid },
                new TspAlgo { Name = "2-opt", Solver = new Opt2(), Color = Color.Silver, IsActive = true},
                new TspAlgo { Name = "about 3-opt", Solver = new Opt3(), Color = Color.Wheat, IsActive = true },
                new TspAlgo { Name = "Cormen", Solver = new CormenHeuristic(), Color = Color.Tomato },
            };

        public TestForm()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            InitializeComponent();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            bitmapPanel.Width = m_Width;
            bitmapPanel.Height = m_Height;

            RefreshPanel();
        }

        private void bitmapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            m_Points.Add(new Point2DReal(e.X, e.Y));
            DrawPoint(e.X, e.Y, Graphics.FromImage(m_Bitmap));
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();
        }

        private void DrawPoint(double x, double y, Graphics g)
        {
            int r = 4;
            g.FillEllipse(new SolidBrush(Color.Red), (float)(x - r), (float) (y - r), 2 * r, 2 * r);
        }

        private void bitmapPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_Bitmap, 0, 0);
        }

        private void buttonGetPath_Click(object sender, EventArgs e)
        {
            if (m_Points.Count < 3)
            {
                return;
            }

            foreach (var algo in _algos)
            {
                if (algo.IsActive)
                {
                    ApplyAlgo(algo);
                }
                else
                {
                    algo.Path = null;
                }
            }

            RefreshPanel();
            RefreshDraw();
        }

        private void ApplyAlgo(TspAlgo algo)
        {
            var n = m_Points.Count;
            if (n < 3)
            {
                algo.Path = null;
                return;
            }
            /*var m = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = m_Points[i].Dist(m_Points[j]);
                }
            }

            double avgDist = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    avgDist += m[i, j];
                }
            }
            avgDist /= (n * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] /= avgDist;
                }
            }*/
            //var measure = new MatrixMeasureFactory().CreateMatrixMeasure(m_Points);
            var measure = new PointsMeasure(m_Points);
            algo.Path = algo.Solver.GetPath(n, measure);
            algo.Length = GetLength(n, m_Points, algo.Path);
        }

        private void RefreshPanel()
        {
            solversPanel.Controls.Clear();
            foreach (var algo in _algos)
            {
                var panel = new FlowLayoutPanel {Height = 50};

                var checkbox = new CheckBox { Text = algo.Name, Checked = algo.IsActive };
                checkbox.CheckedChanged += (s, e) =>
                    {
                        algo.IsActive = checkbox.Checked;
                        if (algo.Path == null && algo.IsActive)
                        {
                            ApplyAlgo(algo);
                            RefreshPanel();
                        }
                        RefreshDraw();
                    };
                panel.Controls.Add(checkbox);

                var lengthLabel = new Label { Width = 160, Text = String.Format("Length: {0:0.00}", algo.Length) };
                panel.Controls.Add(lengthLabel);

                solversPanel.Controls.Add(panel);
            }
        }

        private void RefreshDraw()
        {
            m_Bitmap = new Bitmap(m_Width, m_Height);
            var g = Graphics.FromImage(m_Bitmap);

            var activeAlgos = _algos.Where(a => a.IsActive && a.Path != null);
            var count = activeAlgos.Count();
            foreach (var result in activeAlgos)
            {
                var pen = new Pen(result.Color, 3 * count + 1);
                var n = result.Path.Length;
                var path = result.Path;
                for (int i = 0; i < n - 1; i++)
                {
                    DrawLine(pen, m_Points[path[i]], m_Points[path[i + 1]], g);
                }
                DrawLine(pen, m_Points[path[0]], m_Points[path[n - 1]], g);
                
                count--;
            }

            foreach (var point in m_Points)
            {
                DrawPoint(point.X, point.Y, g);
            }

            textBox1.Text = m_Points.Count.ToString();

            bitmapPanel.Refresh();
        }

        private double GetLength(int n, IEnumerable<Point2DReal> points, IList<int> path)
        {
            var p = points.Select(point => new Point2DReal((point.X - m_XShift) * m_XScale, (point.Y - m_YShift) * m_YScale)).ToArray();
            var length = p[path[n - 1]].Dist(p[path[0]]);
            for (int i = 0; i < n - 1; i++)
            {
                length += p[path[i]].Dist(p[path[i + 1]]);
            }
            return length;
        }

        private void DrawLine(Pen pen, Point2DReal a, Point2DReal b, Graphics g)
        {
            g.DrawLine(pen, (float) a.X, (float) a.Y, (float) b.X, (float) b.Y);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            m_Points = new List<Point2DReal>();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();
        }

        private void buttonAddPoints_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            for (int i = 0; i < numericPoints.Value; i++)
            {
                double x = rnd.NextDouble() * (m_Width - 20) + 10;
                double y = rnd.NextDouble() * (m_Height - 20) + 10;

                m_Points.Add(new Point2DReal(x, y));
            }
            var g = Graphics.FromImage(m_Bitmap);
            foreach(var point in m_Points)
            {
                DrawPoint(point.X, point.Y, g);
            }
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.FileName;
                using (var reader = File.OpenText(path))
                {
                    var line = reader.ReadLine();
                    var n = int.Parse(line);
                    var x = new double[n];
                    var minx = 1e30;
                    var maxx = -1e30;
                    var y = new double[n];
                    var miny = 1e30;
                    var maxy = -1e30;
                    for (int i = 0; i < n; i++)
                    {
                        var s = reader.ReadLine().Split();
                        x[i] = double.Parse(s[0]);
                        minx = Math.Min(minx, x[i]);
                        maxx = Math.Max(maxx, x[i]);
                        y[i] = double.Parse(s[1]);
                        miny = Math.Min(miny, y[i]);
                        maxy = Math.Max(maxy, y[i]);
                    }
                    m_Points = new List<Point2DReal>();
                    foreach (var algo in _algos)
                    {
                        algo.Path = null;
                        algo.Length = 0;
                    }
                    for (int i = 0; i < n; i++)
                    {
                        var nx = (x[i] - minx) / (maxx - minx) * (m_Width - 60) + 30;
                        var ny = (y[i] - miny) / (maxy - miny) * (m_Height - 60) + 30;
                        m_Points.Add(new Point2DReal(nx, ny));
                    }
                    m_XScale = (maxx - minx) / (m_Width - 60);
                    m_XShift = 30;
                    m_YScale = (maxy - miny) / (m_Height - 60);
                    m_YShift = 30;
                }

                RefreshDraw();
            }
        }
    }
}
