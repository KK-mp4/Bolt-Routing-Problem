namespace Bolts
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public List<Station> Stations = new List<Station>();

        Bitmap Map;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            LoadList();
        }

        private void Prepare()
        {
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Length";
            dataGridView1.Columns[4].Name = "Bolt length";
            dataGridView1.Columns[5].Name = "Travel time";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"..\..\..\Bolt_List.json";

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                try
                {
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    int x = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                    int z = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                    //double length = Math.Round(Math.Sqrt((Math.Pow(x - (-105), 2) + Math.Pow(z - (-7), 2))), 1);

                    Station newStation = new Station(name, x, z);
                    Stations.Add(newStation);
                }
                catch
                {
                    MessageBox.Show("Wrong input data", "Error");
                }
            }

            Stations.Sort((x, y) => x.Name.CompareTo(y.Name));

            File.WriteAllText(path, JsonConvert.SerializeObject(Stations, Formatting.Indented));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadList();
        }

        public void LoadList()
        {
            string path = @"..\..\..\Bolt_List.json";
            if (!File.Exists(path))
            {
                return;
            }

            Stations = JsonConvert.DeserializeObject<List<Station>>(File.ReadAllText(path));

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Stations;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        int x = -105;
                        int z = -7;
                        int distanceThreshold = 50;

                        MergeAtCoords(x, z, distanceThreshold);
                    }
                    break;

                case 1:
                    {
                        int distanceThreshold = 50;

                        MergeAtAverage(distanceThreshold);
                    }
                    break;

                case 2:
                    {
                        int distanceThreshold = 50;

                        Loop1(distanceThreshold);
                    }
                    break;

                case 2:
                    {
                        int distanceThreshold = 50;

                        NN(distanceThreshold);
                    }
                    break;

                default:
                    break;
            }
        }

        private void NN(int distanceThreshold)
        {
            List<Station> Stations2 = new List<Station>();

            for (int i = 0; i < Stations.Count; i++)
            {
                double length = Math.Round(Math.Sqrt((Math.Pow(Stations[i].X - x, 2) + Math.Pow(Stations[i].Z - z, 2))), 1);

                if (length > distanceThreshold)
                {
                    double boltLength = Math.Max(Math.Abs(Stations[i].X), Math.Abs(Stations[i].Z));
                    int travelTime = Convert.ToInt32(boltLength / 20);

                    Station newStation = new Station(Stations[i].Name, Stations[i].X, Stations[i].Z, length, boltLength, travelTime);
                    Stations2.Add(newStation);
                }
            }

            Stations2.Sort((x, y) => x.BoltLength.CompareTo(y.BoltLength));

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Stations2;

            PrepareBitmap(1024, 1024);

            Random rnd = new Random();

            double totalDIstance = 0;
            int totalTime = 0;

            for (int i = Stations2.Count - 1; i > 0; i--)
            {
                var color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
                DrawBolt(x, z, Stations2[i].X, Stations2[i].Z, Stations2[Stations2.Count - 1].BoltLength, color);
                totalDIstance += Stations2[i].Distance;
                totalTime += Stations2[i].TravelTime;
            }

            //int j = 24;
            //var color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
            //DrawBolt(x, z, Stations2[j].X, Stations2[j].Z, Stations2[Stations2.Count - 1].BoltLength, color);

            label1.Text = $"Total bolt distance: {(double)totalDIstance / 1000:F0} km";
            label2.Text = $"Total travel time: {(double)totalTime / 60:F0} min";
            label3.Text = $"Average travel time: {(double)totalTime / Stations2.Count:F0} sec";
        }

        private void MergeAtCoords(int x, int z, int distanceThreshold)
        {
            List<Station> Stations2 = new List<Station>();

            for (int i = 0; i < Stations.Count; i++)
            {
                double length = Math.Round(Math.Sqrt((Math.Pow(Stations[i].X - x, 2) + Math.Pow(Stations[i].Z - z, 2))), 1);

                if (length > distanceThreshold)
                {
                    double boltLength = Math.Max(Math.Abs(Stations[i].X), Math.Abs(Stations[i].Z));
                    int travelTime = Convert.ToInt32(boltLength / 20);

                    Station newStation = new Station(Stations[i].Name, Stations[i].X, Stations[i].Z, length, boltLength, travelTime);
                    Stations2.Add(newStation);
                }
            }

            Stations2.Sort((x, y) => x.BoltLength.CompareTo(y.BoltLength));

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Stations2;

            PrepareBitmap(1024, 1024);

            Random rnd = new Random();

            double totalDIstance = 0;
            int totalTime = 0;

            for (int i = Stations2.Count - 1; i > 0; i--)
            {
                var color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
                DrawBolt(x, z, Stations2[i].X, Stations2[i].Z, Stations2[Stations2.Count - 1].BoltLength, color);
                totalDIstance += Stations2[i].Distance;
                totalTime += Stations2[i].TravelTime;
            }

            //int j = 24;
            //var color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
            //DrawBolt(x, z, Stations2[j].X, Stations2[j].Z, Stations2[Stations2.Count - 1].BoltLength, color);

            label1.Text = $"Total bolt distance: {(double)totalDIstance / 1000:F0} km";
            label2.Text = $"Total travel time: {(double)totalTime / 60:F0} min";
            label3.Text = $"Average travel time: {(double)totalTime / Stations2.Count:F0} sec";
        }

        private void MergeAtAverage(int distanceThreshold)
        {
            List<Station> Stations2 = new List<Station>();

            double totalX = 0;
            double totalZ = 0;

            for (int i = 0; i < Stations.Count; i++)
            {
                totalX += Stations[i].X;
                totalZ += Stations[i].Z;
            }

            int averageX = Convert.ToInt32(totalX / Stations.Count);
            int averageZ = Convert.ToInt32(totalZ / Stations.Count);

            for (int i = 0; i < Stations.Count; i++)
            {
                double length = Math.Round(Math.Sqrt((Math.Pow(Stations[i].X - averageX, 2) + Math.Pow(Stations[i].Z - averageZ, 2))), 1);

                if (length > distanceThreshold)
                {
                    double boltLength = Math.Max(Math.Abs(Stations[i].X), Math.Abs(Stations[i].Z));
                    int travelTime = Convert.ToInt32(boltLength / 20);

                    Station newStation = new Station(Stations[i].Name, Stations[i].X, Stations[i].Z, length, boltLength, travelTime);
                    Stations2.Add(newStation);
                }
            }

            Stations2.Sort((x, y) => x.BoltLength.CompareTo(y.BoltLength));

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Stations2;

            PrepareBitmap(1024, 1024);

            Random rnd = new Random();

            double totalDIstance = 0;
            int totalTime = 0;

            for (int i = Stations2.Count - 1; i > 0; i--)
            {
                var color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
                DrawBolt(averageX, averageZ, Stations2[i].X, Stations2[i].Z, Stations2[Stations2.Count - 1].BoltLength, color);
                totalDIstance += Stations2[i].Distance;
                totalTime += Stations2[i].TravelTime;
            }

            label1.Text = $"Total bolt distance: {(double)totalDIstance / 1000:F0} km";
            label2.Text = $"Total travel time: {(double)totalTime / 60:F0} min";
            label3.Text = $"Average travel time: {(double)totalTime / Stations2.Count:F0} sec";
        }

        private void Loop1(int distanceThreshold)
        {
            List<Station> Stations2 = new List<Station>();

            for (int i = 0; i < Stations.Count; i++)
            {
                double length = Math.Round(Math.Sqrt((Math.Pow(Stations[i].X - 0, 2) + Math.Pow(Stations[i].Z - 0, 2))), 1);

                if (length > distanceThreshold)
                {
                    double boltLength = Math.Max(Math.Abs(Stations[i].X), Math.Abs(Stations[i].Z));
                    int travelTime = Convert.ToInt32(boltLength / 20);

                    Station newStation = new Station(Stations[i].Name, Stations[i].X, Stations[i].Z, length, boltLength, travelTime);
                    Stations2.Add(newStation);
                }
            }

            Stations2.Sort((x, y) => x.BoltLength.CompareTo(y.BoltLength));

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Stations2;

            PrepareBitmap(1024, 1024);

            Random rnd = new Random();

            double totalDIstance = 0;
            int totalTime = 0;

            for (int i = Stations2.Count - 1; i > 1; i--)
            {
                Color color = Color.FromArgb(rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
                DrawBolt(Stations2[i - 1].X, Stations2[i - 1].Z, Stations2[i].X, Stations2[i].Z, Stations2[Stations2.Count - 1].BoltLength, color);
                totalDIstance += Math.Sqrt((Math.Pow(Stations[i-1].X - Stations[i].X, 2) + Math.Pow(Stations[i-1].Z - Stations[i].X, 2)));
                totalTime += Convert.ToInt32((Math.Sqrt((Math.Pow(Stations[i - 1].X - Stations[i].X, 2) + Math.Pow(Stations[i - 1].Z - Stations[i].X, 2)))) / 20);
            }

            DrawBolt(Stations2[Stations2.Count - 1].X, Stations2[Stations2.Count - 1].Z, Stations2[0].X, Stations2[0].Z, Stations2[Stations2.Count - 1].BoltLength, Color.White);
            totalDIstance += Math.Sqrt((Math.Pow(Stations[Stations2.Count - 1].X - Stations[0].X, 2) + Math.Pow(Stations[Stations2.Count - 1].Z - Stations[0].X, 2)));
            totalTime += Convert.ToInt32((Math.Sqrt((Math.Pow(Stations[Stations2.Count - 1].X - Stations[0].X, 2) + Math.Pow(Stations[Stations2.Count - 1].Z - Stations[0].X, 2)))) / 20);

            label1.Text = $"Total bolt distance: {(double)totalDIstance / 1000:F0} km";
            label2.Text = $"Total travel time: {(double)totalTime / 60:F0} min";
            label3.Text = $"Average travel time: {(double)totalTime / Stations2.Count:F0} sec";
        }

        private void DrawBolt(int x0, int z0, int x, int z, double maxDistance, Color color)
        {
            double ratioX = (2 * Math.Abs(maxDistance)) / (Map.Width);
            double ratioZ = (2 * Math.Abs(maxDistance)) / (Map.Height);

            int imgCoordsX0 = Convert.ToInt32(((double)x0 / ratioX) + ((double)Map.Width / 2));
            int imgCoordsZ0 = Convert.ToInt32(((double)z0 / ratioZ) + ((double)Map.Height / 2));

            int imgCoordsX = Convert.ToInt32(((double)x / ratioX) + ((double)Map.Width / 2));
            int imgCoordsZ = Convert.ToInt32(((double)z / ratioZ) + ((double)Map.Height / 2));


            int param_1 = imgCoordsX - imgCoordsX0;
            int param_2 = imgCoordsZ - imgCoordsZ0;

            Bitmap newMap = new Bitmap(Map);

            if (param_1 < param_2)
            {
                if(imgCoordsX > imgCoordsX0)
                {
                    while (imgCoordsX >= imgCoordsX0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsX--;

                        if (imgCoordsZ > imgCoordsZ0)
                            imgCoordsZ--;
                        else
                            imgCoordsZ++;
                    }
                }
                else
                {
                    while (imgCoordsX <= imgCoordsX0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsX++;

                        if (imgCoordsZ > imgCoordsZ0)
                            imgCoordsZ--;
                        else
                            imgCoordsZ++;
                    }
                }

                if (imgCoordsZ > imgCoordsZ0)
                {
                    while (imgCoordsZ >= imgCoordsZ0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsZ--;
                    }
                }
                else
                {
                    while (imgCoordsZ <= imgCoordsZ0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsZ++;
                    }
                }
            }
            else
            {
                if (imgCoordsZ > imgCoordsZ0)
                {
                    while (imgCoordsZ >= imgCoordsZ0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsZ--;

                        if (imgCoordsX > imgCoordsX0)
                            imgCoordsX--;
                        else
                            imgCoordsX++;
                    }
                }
                else
                {
                    while (imgCoordsZ <= imgCoordsZ0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsZ++;

                        if (imgCoordsX > imgCoordsX0)
                            imgCoordsX--;
                        else
                            imgCoordsX++;
                    }
                }

                if (imgCoordsX > imgCoordsX0)
                {
                    while (imgCoordsX >= imgCoordsX0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsX--;
                    }
                }
                else
                {
                    while (imgCoordsX <= imgCoordsX0)
                    {
                        newMap.SetPixel(imgCoordsX, imgCoordsZ, color);
                        imgCoordsX++;
                    }
                }
            }

            Map.Dispose();
            Map = new Bitmap(newMap);
            newMap.Dispose();
            pictureBox1.Image = Map;
        }

        private void PrepareBitmap(int width, int height)
        {
            Map = new Bitmap(width, height);

            RectangleF rectf = new RectangleF(0, 0, Map.Width, Map.Height);
            using (Graphics g2 = Graphics.FromImage(Map))
            {
                g2.FillRectangle(Brushes.Black, rectf);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Clipboard.SetImage(pictureBox1.Image);
            }
        }
    }
}
