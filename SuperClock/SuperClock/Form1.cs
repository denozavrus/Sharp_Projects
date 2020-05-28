using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SuperClock1
{
    public partial class Form1 : Form
    {
       Timer timer; 
       private void Execute (Timer timer, TimeCounters counters, IClocks clocks)
        {
            timer.Stop();
            counters.Hours = 0;
            counters.Minuts = 0;
            counters.Seconds = 0;
            clocks.HoursInDay = 24;
            clocks.MinutsInHour = 60;
            clocks.SecondsInMinute = 60;
            clocks.date = new DateTime(0, 0, 0, 0, 0, 0); 
        }

        public void TimerTick(SuperClock superClock36, Dictionary<int, string> months)
        {
            superClock36.DateCheck(superClock36);
            label6.Text = superClock36.counters.Hours.ToString();
            label7.Text = superClock36.counters.Minuts.ToString();
            label8.Text = superClock36.counters.Seconds.ToString();
            label3.Text = superClock36.date.Day.ToString();
            label4.Text = months[(superClock36.date.Month)].ToString();
            label5.Text = superClock36.date.Year.ToString();
            superClock36.counters.Seconds += 1; 
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double koef = 1;
            if (textBox1.Text != null)
            {
                try { koef = 1 / Convert.ToDouble(textBox1.Text); }
                catch
                {
                    MessageBox.Show("Пожалуйста укажите число в коэффициенте ускорялки (дробные числа указывайте через запятую)");
                }
            }
            Dictionary<int, string> months = new Dictionary<int, string>(12);
            months.Add(1, "Января");
            months.Add(2, "Февраля");
            months.Add(3, "Марта");
            months.Add(4, "Апреля");
            months.Add(5, "Мая");
            months.Add(6, "Июня");
            months.Add(7, "Июля");
            months.Add(8, "Августа");
            months.Add(9, "Сентября");
            months.Add(10, "Октября");
            months.Add(11, "Ноября");
            months.Add(12, "Декабря");
            TimeCounters counters = new TimeCounters(dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, dateTimePicker2.Value.Second);
            SuperClock superClock36 = new SuperClock(60, 60, 36, dateTimePicker1.Value,counters);
            timer = new Timer();
            timer.Interval = Convert.ToInt32(1000 * koef);
            timer.Enabled = true;
            timer.Tick += (x, y) => { TimerTick(superClock36, months); };
            timer.Start();
            button1.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            timer.Stop();
            button1.Enabled = true; 
        }
    }
    public class TimeCounters
    {
        public int Hours { get; set; }
        public int Minuts { get; set; }
        public int Seconds { get; set; }

        public TimeCounters (int x, int y, int z)
        {
            this.Hours = x;
            this.Minuts = y;
            this.Seconds = z; 
        }
    }

    public interface IClocks
    {
        int HoursInDay { get; set; }
        int MinutsInHour { get; set; }
        int SecondsInMinute { get; set; }
        void DateCheck(IClocks clocks);
        DateTime date { get; set; }
    }

    public class SuperClock : IClocks
    {
        public TimeCounters counters;
        public SuperClock(int x, int y, int z, DateTime date, TimeCounters counters)
        {
            this.SecondsInMinute = x;
            this.MinutsInHour = y;
            this.HoursInDay = z;
            this.counters = counters; 
            this.date = date;
        }
        public void DateCheck(IClocks superClock)
        {
            if (counters.Hours == HoursInDay)
            {
                this.date = this.date.AddDays(1);
                counters.Hours = 0;
            }

            if (counters.Minuts == MinutsInHour)
            {
                counters.Hours++;
                counters.Minuts = 0;
            }

            if (counters.Seconds == SecondsInMinute)
            {
                counters.Minuts++;
                counters.Seconds = 0;
            }
        }

        public int HoursInDay { get; set; }
        public int MinutsInHour { get; set; }
        public int SecondsInMinute { get; set; }
        public DateTime date { get; set; }
    }
}


