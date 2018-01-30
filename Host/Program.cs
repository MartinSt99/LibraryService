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
            var host2 = new ServiceHost(typeof(OrderBooks));
            //var host = new ServiceHost(typeof(OrderService));
            host.Open();
            host2.Open();
            Console.WriteLine("Running");
            Console.ReadLine();
        }
    }
}