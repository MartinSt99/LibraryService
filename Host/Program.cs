using System;
using System.ServiceModel;
using Kundenservice;

namespace Host
{
    internal class Program

    {
        private static void Main(string[] args)
        {
            //var host = new ServiceHost(typeof(OrderBooks));
            var host = new ServiceHost(typeof(AktienInfo));
            //var host = new ServiceHost(typeof(OrderService));
            host.Open();
            Console.WriteLine("Running");
            Console.ReadLine();
        }
    }
}