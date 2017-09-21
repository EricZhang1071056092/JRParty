using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JRPartyData;

namespace JRPartyService.Methods
{
    public static class CommonMethod
    {
        private static JRPartyDataContext ds = new JRPartyDataContext();
        /*-------------身份证计算年龄---------------*/
        public static int IDCard2Age(string IDCard)
        {
            string Sub_str = IDCard.Substring(6, 8).Insert(4, "-").Insert(7, "-");   //提取出生年份
            TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(Sub_str));
            return (ts.Days / 365);
        }

        /*-------------计算党龄---------------*/
        public static int joinTime2partyAge(string joinTime)
        {
            TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(joinTime));
            return (ts.Days / 365);
        }

        /*--------------计算下属区域----------*/
        public static List<string> getSubDistrict(string districtID)
        {
            List<string> districts = new List<string>();
            List<string> dis1 = new List<string>();
            List<string> dis2 = new List<string>();
            districts.Add(districtID);
            dis2.Add(districtID);
            try
            {
                SYS_District[] x = null;
                SYS_District[] y = null;

                do
                {
                    dis1.Clear();
                    for (int i = 0; i < dis2.Count(); i++)
                    {
                        x = (from a in ds.SYS_District
                             where a.attachTo == dis2[i]
                             select a).ToArray();
                        for (int j = 0; j < x.Length; j++)
                        {
                            if (districts.Contains(x[j].id) == false)
                            {
                                districts.Add(x[j].id);
                                dis2.Add(x[j].id);
                            }
                        }
                        dis2.Remove(dis2[i]);
                    }
                    for (int i = 0; i < dis2.Count(); i++)
                    {
                        y = (from a in ds.SYS_District
                             where a.attachTo == dis2[i]
                             select a).ToArray();
                        for (int j = 0; j < y.Length; j++)
                        {
                            if (dis1.Contains(y[j].id) == false)
                            {
                                dis1.Add(y[j].id);
                            }
                        }
                    }
                } while (dis1.ToArray().Length != 0);
                return districts;
            }
            catch (Exception)
            {
                return districts;
            }
        }

        /*--------------经纬度计算两点距离----------*/
        public static double latitudelongitude2Length(double x1, double y1, double x2, double y2)
        {
            try
            {
                var R = 6371000;//地球半径
                double Length = R * Math.Acos(Math.Cos(y1) * Math.Cos(y2) * Math.Cos(x1 - x2) + Math.Sin(y1) * Math.Sin(y2));
                return Length;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /*--------------浏览器半斜距----------*/
        public static double calculateSlant(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) / 2;
        }
        /*--------------计算选择半径-------------*/
        public static double calculateRadius(int explorX, int explorY, int zoomLevel)
        {
            var explorSlant = calculateSlant(explorX, explorY);
            int scale = 0;
            switch (zoomLevel)
            {
                case 2:
                    scale = 5000;
                    break;
                case 3:
                    scale = 5000;
                    break;
                case 4:
                    scale = 3000;
                    break;
                case 5:
                    scale = 1000;
                    break;
                case 6:
                    scale = 500;
                    break;
                case 7:
                    scale = 300;
                    break;
                case 8:
                    scale = 200;
                    break;
                case 9:
                    scale = 100;
                    break;
                case 10:
                    scale = 50;
                    break;
                case 11:
                    scale = 20;
                    break;
                default:
                    scale = 0;
                    break;
            }
            return scale * explorSlant * 0.013/100000;
        }
        /*--------------计算平面半径-------------*/
        public static double calculate2DRadius(int explorX, int explorY, int zoomLevel)
        {
            var explorSlant = calculateSlant(explorX, explorY);
            int scale = 0;
            switch (zoomLevel)
            {
                case 2:
                    scale = 5000;
                    break;
                case 3:
                    scale = 5000;
                    break;
                case 4:
                    scale = 3000;
                    break;
                case 5:
                    scale = 1000;
                    break;
                case 6:
                    scale = 500;
                    break;
                case 7:
                    scale = 300;
                    break;
                case 8:
                    scale = 200;
                    break;
                case 9:
                    scale = 100;
                    break;
                case 10:
                    scale = 50;
                    break;
                case 11:
                    scale = 20;
                    break;
                default:
                    scale = 0;
                    break;
            }
            return scale * explorSlant * 0.016;
        }


        /*--------------基姆拉尔森计算公式计算日期变星期-------------*/
        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1 || m == 2)
            {
                m += 12;
                y--;         
                //把一月和二月看成是上一年的十三月和十四月，例：如果是2004-1-10则换算成：2003-13-10来代入公式计算。
            }
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
            string weekstr = "";
            switch (week)
            {
                case 0: weekstr = "星期一"; break;
                case 1: weekstr = "星期二"; break;
                case 2: weekstr = "星期三"; break;
                case 3: weekstr = "星期四"; break;
                case 4: weekstr = "星期五"; break;
                case 5: weekstr = "星期六"; break;
                case 6: weekstr = "星期日"; break;
            }
            return weekstr;
        }
    }
}
