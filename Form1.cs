using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 :Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string[] getRec(string line, char sep)
        {
            string[] flds = { };
            char[] seps;
            if (line != null)
            {
                int lastP = line.IndexOf('#');
                if (lastP >= 0)
                {
                    line = line.Substring(0, lastP);
                    line = line.Trim();
                }
                if (sep == ' ')
                {
                    seps = new char[2];
                    seps[0] = ' ';
                    seps[1] = '\t';
                    flds = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    seps = new char[1];
                    seps[0] = sep;
                    flds = line.Split(seps);
                }

            }
            return flds;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public class points : IComparable<points>
        {
            public double W;
            public int I;
           public points(double w, int i)
            {
                W = w;
                I = i;
            }

            int IComparable<points>.CompareTo(points other)
            {
                double Wother = other.W;
                double Wthis = this.W;
                if (Wother < Wthis)
                    return 1;
                if (Wother > Wthis)
                    return -1;
                else
                    return 1;

            }
        }
        List<points> array = null;
        double h;
        double m;
        int l = 0;
        private void button1_Click_1(object sender, EventArgs e)
        {
            array = new List<points>();
            /* if (array != null)
             {
                 MessageBox.Show("k");
                 comboBox1_SelectedIndexChanged(comboBox1, e);
             }
             array = new List<points>();*/
            textBox2.Text = "default";
            string fname = textBox1.Text;
            StreamReader sr = new StreamReader(fname);
            string s;
            for (; (s = sr.ReadLine()) != null; l++)
            {
            }
            sr = new StreamReader(fname);
            double w;
            for (int i = 0; (s = sr.ReadLine()) != null; i++)
            {
                double.TryParse(s, out w);
                array.Add(new points(w,i));
            }

            int index = richTextBox1.Rtf.LastIndexOf("}");
            string massif = "\n   ";
            for (int i = 0; i <l; i++)
            {
                massif = string.Concat(massif, " | ", array[i].W.ToString());
            }
            richTextBox1.Rtf = richTextBox1.Rtf.Substring(0, index) + massif + "}";
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            points max = array.Max();
            points min = array.Min();
            if (string.Compare(textBox2.Text, "default")!=0)
            {
               // MessageBox.Show("j");
                Double.TryParse(this.textBox2.Text, out m);
            }
            else if (l < 100)
            {
                m = (int)Math.Sqrt(l);
                if (m % 2 == 0)
                {
                    m--;
                }
            }
            else
            {
                m = (int)Math.Pow(l, 1 / 3);
                if (m % 2 == 0)
                {
                    m--;
                }
            }
            h = (max.W - min.W) / m;
            //  string selState = comboBox1.SelectedItem.ToString();
            if (comboBox1.SelectedIndex == 1 )
            {
                this.chart1.Series[0].Points.Clear();
                chart1.ChartAreas[0].AxisX.Maximum = array.Max().W + 1;
                chart1.ChartAreas[0].AxisX.Minimum = array.Min().W - 1;//Задаешь максимальные значения координат
                chart1.ChartAreas[0].AxisY.Maximum = 1;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisX.Interval = Math.Round(h,1);
                Series histogram = chart1.Series[0];
                chart1.Series[0] = CreateHistogram(histogram, array,h,m);   
            }
            else
            {
                this.chart2.Series[0].Points.Clear();
                Series Fdistrib = chart2.Series[0];
                chart1.ChartAreas[0].AxisY.Maximum = 1.2;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                //   chart1.ChartAreas[0].AxisX.Interval = 0,1;
                chart2.Series[0] = Fdistribut(Fdistrib, array, h, m);
             //   Series FDist = chart2.Series[1];
            //    chart2.Series[1] = FDistribut (FDist, array, h, m);
            }

        }
        public Series FDistribut (Series FDist, List<points> array, double h, double m)
        {
            FDist.Color = Color.Chocolate;
            FDist.ChartType = SeriesChartType.StepLine;
            return FDist;
        }
        public Series Fdistribut (Series Fdistrib, List<points> array, double h, double m)
        {
            if (array != null)
            {
                array.Sort();
                Fdistrib.ChartType = SeriesChartType.Point;
                Fdistrib.Color = Color.DarkOrange;
                double k = 0;
                //  int remarkcount;
                int count = array.Count;
                for (int i = 0; i < count; i++)
                {
                    //  var remark = array.FindAll(rem => rem.W < Math.Round((i + 1) * h, 1) & rem.W >= Math.Round(i * h, 1));
                    // remarkcount = i/count;
                    //   MessageBox.Show( count.ToString(),remark.Count.ToString());
                    k = (double)(i+1) / count;
                    Fdistrib.Points.AddXY(array[i].W, k);
                }
            }
            return Fdistrib;
        }
        public Series CreateHistogram(Series histogram, List <points> array,double h, double m)
        {
            if (array != null)
            {
                // MessageBox.Show(m.ToString(), Math.Round((double)h * (2 + 1), 1).ToString());
                histogram.ChartType = SeriesChartType.Column;
                histogram.BorderWidth = 1;
                histogram.Color = Color.Indigo;
                //   points max = array.Max();
                   points min = array.Min();
                int count = array.Count;
                int remarkcount = 0;
              //  h = count / m;
                double k = 0;
                for (double i = 0; i <= m; i++)
                {
                    // MessageBox.Show(" count = " , count.ToString());
                    var remark = array.FindAll(rem => rem.W < (min.W + h*(i + 1)) & rem.W >= (i * h + min.W));
                    remarkcount = remark.Count;
                      // MessageBox.Show( count.ToString(),remark.Count.ToString());
                    k = (double)remarkcount / count + k;
                    histogram.Points.AddXY(min.W + h * (i + 0.5), (double)remarkcount / count);
                 //   i = i + h;
                }
                 MessageBox.Show(k.ToString());
            }
            return histogram;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            array = null;
            richTextBox1.Text = null;
            h = 0;
            m =0;
            l = 0;
            textBox1.Text = null;
            textBox2.Text = null;           
            comboBox1.SelectedIndex = 0;
        }
        PictureBox pBox = new PictureBox();
      
    }
}
