using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Temp;
using TravellingSalesmanProblem;

namespace TestWindowsFormsApplication
{
    public partial class TestForm : Form
    {
        private Bitmap m_Bitmap;
        private List<Point2DReal> m_Points = new List<Point2DReal>();

        private int m_Width = 800;
        private int m_Height = 600;

        private TspAlgo[] _algos = new[]
            {
                new TspAlgo { Name = "Ant Colony", Solver = new AntColony(), Color = Color.Green, IsActive = true},
                new TspAlgo { Name = "Little Algo", Solver = new LittleAlgorithm(), Color = Color.Red }
            };

        public TestForm()
        {
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

            //var algorithm = new LittleAlgorithm();
            //var algorithm = new SimpleGreedy();
            var n = m_Points.Count;
            var m = new double[n,n];
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
            }

            foreach (var algo in _algos.Where(a => a.IsActive))
            {
                algo.Path = algo.Solver.GetPath(n, m);
                algo.Length = GetLength(n, m, algo.Path);
            }

            RefreshPanel();
            RefreshDraw();
        }

        private void RefreshPanel()
        {
            solversPanel.Controls.Clear();
            foreach (var algo in _algos)
            {
                var panel = new FlowLayoutPanel();

                var checkbox = new CheckBox { Text = algo.Name, Checked = algo.IsActive };
                checkbox.CheckedChanged += (s, e) =>
                    {
                        algo.IsActive = checkbox.Checked;
                        RefreshDraw();
                    };
                panel.Controls.Add(checkbox);

                var lengthLabel = new Label { Text = String.Format("Length: {0}", algo.Length) };
                panel.Controls.Add(lengthLabel);

                solversPanel.Controls.Add(panel);
            }
        }

        private void RefreshDraw()
        {
            m_Bitmap = new Bitmap(m_Width, m_Height);
            var g = Graphics.FromImage(m_Bitmap);

            var count = 0;
            foreach (var result in _algos.Where(a => a.IsActive))
            {
                var pen = new Pen(result.Color, 2 * count + 1);
                var n = result.Path.Length;
                var path = result.Path;
                for (int i = 0; i < n - 1; i++)
                {
                    DrawLine(pen, m_Points[path[i]], m_Points[path[i + 1]], g);
                }
                DrawLine(pen, m_Points[path[0]], m_Points[path[n - 1]], g);
                foreach (var point in m_Points)
                {
                    DrawPoint(point.X, point.Y, g);
                }
                count++;
            }

            bitmapPanel.Refresh();
        }

        private static double GetLength(int n, double[,] d, IList<int> path)
        {
            var length = d[path[n - 1], path[0]];
            for (int i = 0; i < n - 1; i++)
            {
                length += d[path[i], path[i + 1]];
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
    }
}
