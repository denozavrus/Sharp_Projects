using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datetask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                textBox1.Enabled = true;
            }
            else if (checkBox2.Checked == false)
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                textBox1.Enabled = true;
            }
            else if (checkBox1.Checked == false)
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox4.Checked = false;
                textBox2.Enabled = true;
            }
            else if (checkBox4.Checked == false)
            {
                textBox2.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox3.Checked = false;
                textBox2.Enabled = true;
            }
            else if (checkBox3.Checked == false)
            {
                textBox2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(@"C:\Users\Денис\Desktop\FileLab.txt", true);  
            int a;
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime(); 
            if (checkBox1.Checked == true)
            {
                Data data1 = new DataAn();
                a = data1.Parse(textBox1.Text, ref d1);
                switch (a)
                {
                    case 1:
                        {
                            label3.Text = "Неверный формат ввода даты. Пожалуйста, введите данные через точку";
                            sw.WriteLine("Ошибка ввода даты");
                            return;
                        }
                    case 2:
                        {
                            label3.Text = "Неправильная дата. Проверьте правильность даты и выбранного формата";
                            sw.WriteLine("Ввод неверной даты");
                            return;
                        }
                }
            }
            if (checkBox2.Checked == true)
            {
                Data data1 = new DataAm();
                a = data1.Parse(textBox1.Text, ref d1);
                switch (a)
                {
                    case 1:
                        {
                            label3.Text = "Неверный формат ввода даты. Пожалуйста, введите данные через точку";
                            sw.WriteLine("Ошибка ввода даты");
                            return;
                        }
                    case 2:
                        {
                            label3.Text = "Неправильная дата. Проверьте правильность даты и выбранного формата";
                            sw.WriteLine("Ввод неверной даты");
                            return; 
                        }
                }
            }
            if (checkBox3.Checked == true)
            {
                Data data2 = new DataAn();
                a = data2.Parse(textBox2.Text, ref d2);
                switch (a)
                {
                    case 1:
                        {
                            label3.Text = "Неверный формат ввода даты. Пожалуйста, введите данные через точку";
                            sw.WriteLine("Ошибка ввода даты");
                            return;
                        }
                    case 2:
                        {
                            label3.Text = "Неправильная дата. Проверьте правильность даты и выбранного формата";
                            sw.WriteLine("Ввод неверной даты");
                            return;
                        }
                }
            }
            if (checkBox4.Checked == true)
            {
                Data data2 = new DataAm();
                a = data2.Parse(textBox2.Text, ref d2);
                switch (a)
                {
                    case 1:
                        {
                            label3.Text = "Неверный формат ввода даты. Пожалуйста, введите данные через точку";
                            sw.WriteLine("Ошибка ввода даты");
                            return;
                        }
                    case 2:
                        {
                            label3.Text = "Неправильная дата. Проверьте правильность даты и выбранного формата";
                            sw.WriteLine("Ввод неверной даты");
                            return;
                        }
                }
            }
            sw.WriteLine(textBox1.Text, " - Первая дата");
            sw.WriteLine(textBox2.Text, " - Вторая дата");
            TimeSpan timeSpan = d1.Subtract(d2);
            textBox3.Text = Convert.ToString(timeSpan.Seconds);
            textBox4.Text = Convert.ToString(timeSpan.Minutes);
            textBox5.Text = Convert.ToString(timeSpan.Hours);
            int month, m = 0;
            int year;
            int days = timeSpan.Days; 
            month = d2.Month;
            year = d2.Year; 
            for (int i = d2.Month + d2.Year*12; i < d1.Month + d1.Year*12; i++ )
            {
                if (month == 13)
                {
                    month = 1;
                    year = year + 1;
                }
                if ((days - DateTime.DaysInMonth(year, month)) < 0)
                {
                    m = 1; 
                    break;
                }
                days = days - DateTime.DaysInMonth(year, month);
                month++; 
            }
            textBox7.Text = Convert.ToString(Math.Floor(Math.Abs(days) / 7.0));
            textBox6.Text = Convert.ToString(days % 7);
            textBox8.Text = Convert.ToString(Math.Abs(d2.Month + d2.Year * 12 - (d1.Month + d1.Year * 12)) - m);
            sw.WriteLine("Временной промежуток был успешно посчитан: " + textBox3.Text + " -количество секунд " + textBox4.Text + " -количество минут " + textBox5.Text + " -количество часов ");
            sw.WriteLine(textBox6.Text + " -количество дней " + textBox7.Text + " -количество недель " + textBox8.Text + " -количество месяцев ");
            sw.WriteLine("Результат посчитан. Ожидание ввода следующей даты");
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(@"C:\Users\Денис\Desktop\FileLab.txt", false);
            sw.Close();
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            textBox7.Text = null;
            textBox8.Text = null;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\Денис\Desktop\FileLab.txt");
        }
    }
}
