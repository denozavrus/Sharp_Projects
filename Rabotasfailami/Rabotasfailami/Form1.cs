using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
using System.Windows.Forms;

namespace Rabotasfailami
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static int max=0; 
        class cveta
        {
            public int red;
            public int green;
            public int blue;
            public int znach;
            public cveta (int Red, int Green, int Blue, int Znach)
            {
                red = Red;
                blue = Blue;
                green = Green;
                znach = Znach; 
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("C:/Users/Денис/Desktop/FileLab.txt");
            Random rnd = new Random();
            try
            {
                for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {
                    cveta a = new cveta(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(-100, 100));
                    sw.Write($"{a.red}:{a.green}:{a.blue}:{a.znach}:");
                    if (Math.Abs(a.znach) > max)
                    {
                        max = Math.Abs(a.znach);
                    }
                }
            }
            catch
            {
                label1.Text = "Пожалуйста введите число столбцов";
            }
            sw.Close(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("C: /Users/Денис/Desktop/FileLab.txt");
            Graphics g = this.CreateGraphics();
            string[] f;
            string d = sr.ReadToEnd();
            d = d.Trim(':');
            f = d.Split(':');
            int dlina = 700 / ((Convert.ToInt32(textBox1.Text)*2)-1);
            int c = 0;
            double t = 185.0 / max;
            for (int i = 0; i < Convert.ToInt32(textBox1.Text)*4; i =i+4)
            {
                cveta a = new cveta(Convert.ToInt32(f[i]), Convert.ToInt32(f[i+1]), Convert.ToInt32(f[i+2]), Convert.ToInt32(f[i+3]));
                SolidBrush brush1 = new SolidBrush(Color.FromArgb(a.red, a.green, a.blue)); 
                if (a.znach>0)
                    {
                    g.FillRectangle(brush1, 10 + 2 * c * dlina, Convert.ToInt64(215 - (t * a.znach)), dlina, Convert.ToInt64(t * a.znach));
                    Label text = new Label();
                    text.Location = new Point(10 + 2 * c * dlina + (dlina / 2)-10, Convert.ToInt32(215 - (t * a.znach)-20));
                    text.Text = Convert.ToString(a.znach);
                    text.Width = 30;
                    text.Height = 12;
                    this.Controls.Add(text);
                }
                else
                {
                    g.FillRectangle(brush1, 10 + 2 * c * dlina, 215, dlina, Convert.ToInt64(t * Math.Abs(a.znach)));
                    Label text = new Label();
                    text.Location = new Point(10 + 2 * c * dlina + (dlina/2)-10, Convert.ToInt32(215 + Math.Abs( t * a.znach)+15));
                    text.Text = Convert.ToString(a.znach);
                    text.Width = 30;
                    text.Height = 12;
                    this.Controls.Add(text);
                }
                c = c + 1;
            }
            sr.Close(); 
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.DrawLine(new Pen(Color.Black, 2.0f), 5, 30, 5, 400);
            g.DrawLine(new Pen(Color.Black, 2.0f), 5, 215, 705, 215); 
        }
    }
}
