using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DBLib
{
    public class Validate
    {
        public static bool IsIP(string inputIP)
        {
            if (string.IsNullOrWhiteSpace(inputIP)) return false;

            var flg = true;
            var arr = inputIP.Split('.');

            if (arr.Length != 4) return false;

            for (var i = 0; i < arr.Length; i++)
            {
                int r;
                if (int.TryParse(arr[i], out r))
                {
                    if (i == 0 || i == 3)
                    {
                        if (!(r > 0 && r < 256))
                        {
                            flg = false;
                            break;
                        }
                    }
                    else
                    {
                        if (!(r >= 0 && r < 256))
                        {
                            flg = false;
                            break;
                        }
                    }
                }
                else
                {
                    flg = false;
                    break;
                }
            }

            return flg;
        }

        public static bool IsIP(string inputIP, string startIP, string endIP)
        {
            if (!IsIP(inputIP)) return false;
            if (!IsIP(startIP)) throw new Exception("StartIP不是有效的IP.");
            if (!IsIP(endIP)) throw new Exception("EndIP不是有效的IP.");

            var flg = true;

            var inputArr = inputIP.Split('.');
            var startArr = startIP.Split('.');
            var endArr = endIP.Split('.');
            string strInput = null, strStart = null, strEnd = null;

            foreach (var item in inputArr)
            {
                var s = "000" + item;
                strInput += s.Substring(s.Length - 3);
            }
            foreach (var item in startArr)
            {
                var s = "000" + item;
                strStart += s.Substring(s.Length - 3);
            }
            foreach (var item in endArr)
            {
                var s = "000" + item;
                strEnd += s.Substring(s.Length - 3);
            }

            var input = Convert.ToInt64(strInput);
            var start = Convert.ToInt64(strStart);
            var end = Convert.ToInt64(strEnd);

            if (input > end || input < start)
                flg = false;

            return flg;
        }
    }
}
