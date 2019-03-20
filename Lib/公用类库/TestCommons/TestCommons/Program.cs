using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WHC.Framework.Commons;
using System.Diagnostics;

namespace TestCommons
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //参数字符串：first -o outfile.txt --compile second \errors=errors.txt third fourth --test = "the value" fifth

            CommandArgs objArgs = CommandLine.Parse(args);
            
            //键值参数列表，得到输出
            //o:outfile.txt
            //compile:second
            //errors:errors.txt
            //test:the value
            foreach (string str in objArgs.ArgPairs.Keys)
            {
                Debug.WriteLine(string.Format("{0}:{1}", str, objArgs.ArgPairs[str]));
            }


            //非键值参数列表：得到first third fourth fifth 共四个字符串
            foreach (string str in objArgs.Params)
            {
                Debug.WriteLine(str);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
