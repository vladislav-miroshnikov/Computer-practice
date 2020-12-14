using System;
using System.ServiceModel;
using ServerContract;
using System.ServiceModel.Description;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ServiceFilter), new Uri("net.tcp://localhost:5567"));
            host.Description.Behaviors.Add(new ServiceMetadataBehavior());
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "/mex");
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            host.AddServiceEndpoint(typeof(IServerContract), binding, "/srv");
            host.Open();
            Console.WriteLine("Server is now running at net.tcp://localhost:5567, port 5567:");
            Console.WriteLine("Write command 'exit' to close host");
            while(Console.ReadLine() != "exit")
            {
                Console.WriteLine("It's not a exit command");
            }
            host.Close();
        }
    }
}
