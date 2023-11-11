using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Timers;

namespace miny
{
    public class Policko : Label
    {
        public bool BOMBA;
        int x;
        int y;

        public int Sousedi(Policko[,] pole)
        {
            int sousedi = 0;

            x = int.Parse(this.Name.Split('_')[0]);
            y = int.Parse(this.Name.Split('_')[1]);

            int startx =-1;
            int konecx = 2;
            int starty = -1;
            int konecy = 2;

            if (x == 0) { startx = 0; }
            else if(x == pole.GetLength(0)-1) { konecx = 1;}
            if (y == 0) { starty = 0; }
            else if (y == pole.GetLength(1)-1) { konecy = 1; }




            for (int i = startx; i < konecx; i++)
            {
                for (int j = starty; j < konecy; j++)
                {
                    if (pole[x + i, y + j].BOMBA == true && !(i == 0 && j == 0))
                    {
                        sousedi += 1;
                    }
                }
            }

            return sousedi;
        }
    }

    public partial class Form1 : Form
    {
        Policko[,] pole;
        Image vlajecka;
        Image bomba;
        Image konec;

        int width = 10;
        int height = 10;

        Label l;
        Label score;

        public Form1()
        {
            InitializeComponent();

            List<Policko> bomby = new List<Policko>();

            vlajecka = (Image)(new Bitmap(Image.FromFile(@"C:\\Users\\pisko\\source\\repos\\miny\\miny\\vlajecka.png"), new Size(this.Width / width, this.Height / height)));
            bomba = (Image)(new Bitmap(Image.FromFile(@"C:\Users\pisko\source\repos\miny\miny\bomba.png"), new Size(this.Width / width, this.Height / height)));
            konec = new Bitmap(Image.FromFile(@"C:\Users\pisko\source\repos\miny\miny\5.png"), new Size(Width, ClientSize.Height));
           
            pole = new Policko[width, height];

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    pole[i, j] = new Policko();
                    this.Controls.Add(pole[i, j]);

                    pole[i, j].Location = new Point(this.Width/width * i, this.ClientSize.Height / height * j);
                    pole[i, j].Name = i + "_" + j;
                    pole[i, j].Size = new Size(this.Width / width, this.ClientSize.Height / height);
                    pole[i, j].TabIndex = 0;
                    pole[i, j].Click += new EventHandler(Klikani);
                    pole[i, j].BorderStyle = BorderStyle.Fixed3D;
                    pole[i, j].Font = new Font(new FontFamily("Arial"), Math.Min(this.Width / width, this.ClientSize.Height / height));
                    pole[i, j].TextAlign = ContentAlignment.MiddleCenter;

                }

            }

            var random = new Random();

            for(int i = 0; i < (pole.GetLength(0)*pole.GetLength(1))/10; i++)
            {
                int x = random.Next(0, pole.GetLength(0));
                int y = random.Next(0, pole.GetLength(1));

                pole[x, y].BOMBA = true;
                bomby.Add(pole[x, y]);
            }

            Resize += new EventHandler (OnResize);

            l = new Label();
            l.Location = new System.Drawing.Point(0, 0);
            l.Size = new Size(this.Width, this.Height);

            score = new Label();
            score.Location = new Point(0, 0);
            score.Size = new Size(10, 20);
            score.Name = "score";
            score.TabIndex = 0;
            score.Text = bomby.Count().ToString();
            score.BorderStyle = BorderStyle.FixedSingle;
            score.BringToFront();


            InitializeComponent();



        }

        
        private void OnResize(object sender, EventArgs e)
        {
            Control control = sender as Control;
            this.Width = control.Size.Width;
            this.Height = control.Size.Height;
            l.Size = new Size(this.Width, this.Height);

            vlajecka = (Image)(new Bitmap(Image.FromFile(@"C:\\Users\\pisko\\source\\repos\\miny\\miny\\vlajecka.png"), new Size(this.Width / width, this.ClientSize.Height / height)));
            bomba = (Image)(new Bitmap(Image.FromFile(@"C:\Users\pisko\source\repos\miny\miny\bomba.png"), new Size(this.Width / width, this.ClientSize.Height / height)));

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {                
                    pole[i, j].Location = new Point(this.Width / width * i, this.ClientSize.Height / height * j);     
                    pole[i, j].Size = new Size(this.Width / width, this.ClientSize.Height / height);
                    pole[i, j].Font = new Font(new FontFamily("Papyrus"), Math.Min(this.Width / width, this.ClientSize.Height / height)/2);

                }

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Klikani(object sender, EventArgs e)
        {
            Policko sender2 = (Policko)sender;
            MouseEventArgs m = (MouseEventArgs)e;

            if (m.Button == MouseButtons.Left)
            {
                if (sender2.BOMBA == true)
                {
                    sender2.Image = bomba;
                    velkyTresk();

                }
                else
                {
                    sender2.Image = null;
                    sender2.Text = sender2.Sousedi(pole).ToString();
                }
            }
            else if(m.Button == MouseButtons.Right)
            {
                sender2.Image = vlajecka;
            }
        }
        private void velkyTresk()
        {
            int a = 0;


           

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2500;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            
            void timer_Tick(object sender, EventArgs e)
            {
                if(a == 1)
                {
                    Application.Exit();

                }

                l.BringToFront();
                l.Image = konec;
                timer.Start();

                a++;
            }
            



        }
    }
}