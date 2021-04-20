using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2_oiad
{
    public struct MyPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public partial class Form1 : Form
    {
        const int k = 4;
        const int countDot = 430;
        Random rnd = new Random();
        List<MyPoint> pointsAll = new List<MyPoint>();
        Cluster[] clusters = new Cluster[k];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateDots(pointsAll);

            for (int i = 0; i < pointsAll.Count; i++)
                myChart.Series["AllDots"].Points.AddXY(pointsAll[i].X, pointsAll[i].Y);

            InitialCenters(pointsAll, clusters);
        }

        void GenerateDots(List<MyPoint> pList)
        {
            MyPoint tempP = new MyPoint();
            double lower = 1.0;
            double upper = 30.0;
            for (int i = 0; i <= countDot; i++)
            {
                if (i == 70)
                    upper = 20.0;
                else if (i == 180)
                {
                    lower = 5.0;
                    upper = 25.0;
                }
                else if (i == 290)
                    lower = 10.0;
                else if (i == 340)
                {
                    lower = 1.0;
                    upper = 30.0;
                }
                tempP.X = lower + (rnd.NextDouble() * (upper - lower));
                tempP.X = Math.Round(tempP.X, 3);
                tempP.Y = lower + (rnd.NextDouble() * (upper - lower));
                tempP.Y = Math.Round(tempP.Y, 3);
                if (pList.Contains(tempP))
                    i--;
                else
                    pList.Add(tempP);
            }
        }

        void InitialCenters(List<MyPoint> points, Cluster[] clusts)
        {
            int step = countDot / k;
            int steper = 0;

            for (int i = 0; i < k; steper += step, i++)
            {
                clusts[i] = new Cluster(points[steper]);
            }
        }

        void AssignTo(Cluster[] clusts, List<MyPoint> pList)
        {
            for (int i = 0; i < k; i++)
                clusts[i].Clear();
            for (int i = 0; i < countDot; i++)
            {
                double tempMin = clusts[0].GetDistanceTo(pList[i]);
                Cluster tempClust = clusts[0];
                for (int j = 1; j < k; j++)
                    if (tempMin > clusts[j].GetDistanceTo(pList[i]))
                    {
                        tempMin = clusts[j].Distance;
                        tempClust = clusts[j];
                    }
                tempClust.Add(pList[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (; ; )
            {
                AssignTo(clusters, pointsAll);

                for (int i = 0; i < k; i++)
                    clusters[i].SetCenter();

                int count = 0;
                for (int i = 0; i < k; i++)
                    if (clusters[i].currCentr.X == clusters[i].lastCentr.X && clusters[i].currCentr.Y == clusters[i].lastCentr.Y)
                        count++;

                if (count == k) break;
            }

            for (int i = 0; i < k + 1; i++)
                myChart.Series[i].Points.Clear();

            for (int i = 0; i < k; i++)
                for (int j = 0; j < clusters[i].Size(); j++)
                    myChart.Series[i + 1].Points.AddXY(clusters[i].Points[j].X, clusters[i].Points[j].Y);

            using (StreamWriter sw = new StreamWriter("testfile.txt"))
            {
                for (int i = 0; i < k; i++)
                    for (int j = 0; j < clusters[i].Size(); j++)
                        sw.WriteLine($"dot{j} {clusters[i].Points[j].X} {clusters[i].Points[j].Y}");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                for (int i = 0; i < k; i++)
                    myChart.Series[i + 5].Points.AddXY(clusters[i].currCentr.X, clusters[i].currCentr.Y);
            else
                for (int i = 0; i < k; i++)
                     myChart.Series[i + 5].Points.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AssignTo(clusters, pointsAll);
            for (int i = 0; i < k + 1; i++)
                myChart.Series[i].Points.Clear();

            for (int i = 0; i < k; i++)
                for (int j = 0; j < clusters[i].Size(); j++)
                    myChart.Series[i + 1].Points.AddXY(clusters[i].Points[j].X, clusters[i].Points[j].Y); 
        }
    }
}
