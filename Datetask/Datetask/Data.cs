using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datetask
{
    abstract class Data
    {
        public int Sec;
        public int Min;
        public int Hour;
        public int Day; 
        public int Month;
        public int Year;
        public abstract int Parse (string sr, ref DateTime d);
    }
    class DataAn : Data
    {
        public override int Parse (string sr, ref DateTime d)
        {
            string[] a; 
            try
            {
                a = sr.Split('.');
            }
            catch
            {
                return 1; 
            }
            if (a.Length == 6)
            {
                Day = Convert.ToInt32(a[0]);
                Month = Convert.ToInt32(a[1]);
                Year = Convert.ToInt32(a[2]);
                Hour = Convert.ToInt32(a[3]);
                Min = Convert.ToInt32(a[4]);
                Sec = Convert.ToInt32(a[5]);
                try
                {
                    d = new DateTime(Year, Month, Day, Hour, Min, Sec);
                }
                catch
                {
                    return 2;
                }
                return 3;
            }
            else if (a.Length ==3)
            {
                Day = Convert.ToInt32(a[0]);
                Month = Convert.ToInt32(a[1]);
                Year = Convert.ToInt32(a[2]);
                try
                {
                    d = new DateTime(Year, Month, Day);
                }
                catch
                {
                    return 2;
                }
                return 3;
            }
            else
            {
                return 1; 
            }
        }
    }
    class DataAm : Data
    {
        public override int Parse(string sr, ref DateTime d)
        {
            string[] a;
            try
            {
                a = sr.Split('.');
            }
            catch
            {
                return 1;
            }
            if (a.Length == 6)
            {
                Month = Convert.ToInt32(a[0]);
                Day = Convert.ToInt32(a[1]);
                Year = Convert.ToInt32(a[2]);
                Hour = Convert.ToInt32(a[3]);
                Min = Convert.ToInt32(a[4]);
                Sec = Convert.ToInt32(a[5]);
                try
                {
                    d = new DateTime(Year, Month, Day, Hour, Min, Sec);
                }
                catch
                {
                    return 2;
                }
                return 3;
            }
            else if (a.Length == 3)
            {
                Month = Convert.ToInt32(a[0]);
                Day = Convert.ToInt32(a[1]);
                Year = Convert.ToInt32(a[2]);
                try
                {
                    d = new DateTime(Year, Month, Day);
                }
                catch
                {
                    return 2;
                }
                return 3;
            }
            else
            {
                return 1;
            }
        }
    }
}
