using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace HsmBI
{
    public class collections

    {
        public enum ImageSizePref
        {
            None,
            Width,
            Height
        }
        public enum Months
        {
            January = 1,
            February =2,
            March = 3,
            April = 4,
            May=5,
            June =6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public enum Gender
        {
            Male, Female
        }
        public enum MaritalStatus
        {
            Single, Married
        }
        public enum Availability
        {
            Regular
        }
       
            public static Dictionary<int, string> GetGenders()
            {
                Dictionary<int, string> lst = new Dictionary<int, string>();
                lst.Add((int)Gender.Male, Gender.Male.ToString());
                lst.Add((int)Gender.Female, Gender.Female.ToString());
                return lst;
            }

            public static Dictionary<int, string> GetMaritalStatuses()
            {
                Dictionary<int, string> lst = new Dictionary<int, string>();
                lst.Add((int)MaritalStatus.Single, MaritalStatus.Single.ToString());
                lst.Add((int)MaritalStatus.Married, MaritalStatus.Married.ToString());
                return lst;
            }
        
    }

    public class ImageDimension
    {
        private int width;

        private int height;

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public ImageDimension(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    public class Shared
    {
        public Shared()
        {
        }

        public static string CreateTag(params string[] keys)
        {
            string text = string.Empty;
            try
            {
                string[] strArrays = keys;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string istr = strArrays[i];
                    if (istr != keys[0])
                    {
                        text = string.Concat(text, ",");
                    }
                    text = string.Concat(text, Shared.StripSymbols(Shared.StripHTML(istr)).Replace(" ", ","));
                }
            }
            catch
            {
            }
            return text;
        }

        public static Image CropImage(Bitmap source, int maxWidth, int maxHeight)
        {
            Bitmap bmp = new Bitmap(maxWidth, maxHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, new Rectangle(0, 0, maxWidth, maxHeight), GraphicsUnit.Pixel);
            return bmp;
        }

        public static string GenerateCode()
        {
            DateTime cd = DateTime.Now;
            int num = Convert.ToInt32(cd.ToString("yyMMdd")) + Convert.ToInt32(cd.ToString("HHmmss")) + Convert.ToInt32(cd.ToString("ffff"));
            return num.ToString();
        }

        public static string generateticket()
        {
            Guid guid = Guid.NewGuid();

            return string.Concat(guid.ToString().Substring(0,3), GenerateCode());
        }
        public static string GenerateFileName(string title, string ext)
        {
            string fname = "hsm";
            string str = Shared.StripSymbols(Shared.StripHTML(title));
            char[] chrArray = new char[] { ' ' };
            foreach (string t in str.Split(chrArray).Take<string>(5))
            {
                fname = string.Concat(fname, "-", t);
            }
            Guid guid = Guid.NewGuid();
            fname = string.Concat(fname, "-", string.Format("{0}.{1}", guid.ToString().Substring(0, 10), ext));
            return fname;
        }


        public static string GetUniqueKey(string txt)
        {
            string lower = Shared.StripHTML(Shared.StripSymbols(txt)).Replace(" ", "-").ToLower();
            return lower;
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, collections.ImageSizePref pref = 0)
        {
            double ratioX = (double)maxWidth / (double)image.Width;
            double ratioY = (double)maxHeight / (double)image.Height;
            double ratio = 1;
            switch (pref)
            {
                case collections.ImageSizePref.Width:
                    {
                        ratio = ratioX;
                        break;
                    }
                case collections.ImageSizePref.Height:
                    {
                        ratio = ratioY;
                        break;
                    }
                default:
                    {
                        ratio = Math.Max(ratioX, ratioY);
                        break;
                    }
            }
            int newWidth = (int)((double)image.Width * ratio);
            int newHeight = (int)((double)image.Height * ratio);
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static string StripHTML(string htmlString)
        {
            string str = Regex.Replace(htmlString, "<[^>]*>", string.Empty).Replace("&nbsp;", string.Empty).Trim();
            return str;
        }

        public static string StripSymbols(string xString)
        {
            return (new Regex("[;\\\\\\\\/:*?\"<>|&']")).Replace(xString, string.Empty);
        }
    }
    
}
