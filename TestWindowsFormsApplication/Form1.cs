using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TestForm()
        {
            InitializeComponent();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            bitmapPanel.Width = m_Width;
            bitmapPanel.Height = m_Height;
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

            var algorithm = new LittleAlgorithm();
            var n = m_Points.Count;
            var m = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = m_Points[i].Dist(m_Points[j]);
                }
            }
            var result = algorithm.GetPath(n, m);
            m_Bitmap = new Bitmap(m_Width, m_Height);
            var g = Graphics.FromImage(m_Bitmap);
            for (int i = 0; i < n - 1; i++)
            {
                DrawLine(m_Points[result[i]], m_Points[result[i + 1]], g);
            }
            DrawLine(m_Points[result[0]], m_Points[result[n - 1]], g);
            foreach (var point in m_Points)
            {
                DrawPoint(point.X, point.Y, g);
            }
            bitmapPanel.Refresh();
        }

        private void DrawLine(Point2DReal a, Point2DReal b, Graphics g)
        {
            g.DrawLine(new Pen(Color.Green), (float) a.X, (float) a.Y, (float) b.X, (float) b.Y);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            m_Points = new List<Point2DReal>();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();
        }
    }
}
