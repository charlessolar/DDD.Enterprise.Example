using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace ServiceStack
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppHost().Init().Start("http://*:8088/");
            "ServiceStack SelfHost listening at http://localhost:8088 ".Print();
            Process.Start("http://localhost:8088/");

            Console.ReadLine();
        }
    }
}
