using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public class Client
    {
        private readonly IPEndPoint clientIpEndPoint;
        public List<IPEndPoint> ConnectedClients { get; private set; }

        private readonly Socket socket;

        private readonly IController clientController;
        public bool IsLeave { get; private set; }

        public Client(IController clientController)
        {
            this.clientController = clientController;
            ConnectedClients = new List<IPEndPoint>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientIpEndPoint = GetLocalIPEndPoint(clientController.GetPortValue());
            bool isIpCorrect = false;
            while (!isIpCorrect)
            {
                try
                {
                    socket.Bind(clientIpEndPoint);
                    isIpCorrect = true;
                }
                catch (SocketException)
                {
                    Console.WriteLine("This port is already used");
                    clientIpEndPoint = GetLocalIPEndPoint(clientController.GetPortValue());
                }
            }
            Console.WriteLine($"Your ip-name in network: {clientIpEndPoint}");
        }

        public void StartWaiting()
        {
            Task waitingTask = new Task(Wait);
            waitingTask.Start();
        }

        
        private void Wait()
        {
            try
            {
                while (true)
                {
                    string inputMessage = string.Empty;
                    int bytes = 0;
                    byte[] data = new byte[2048];
                    EndPoint tmp = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = socket.ReceiveFrom(data, ref tmp);
                        inputMessage += Encoding.Unicode.GetString(data, 0, bytes);
                    } while (socket.Available > 0);

                    IPEndPoint senderIp = tmp as IPEndPoint;

                    if (inputMessage[0] == '@')
                    {
                        inputMessage = InteractionProtocol.MessageProcessing(inputMessage);
                    }
                    else if (inputMessage[0] == '$')
                    {
                        inputMessage = inputMessage.Remove(0, 1);
                    }

                    if (inputMessage[0] != '*' && inputMessage[0] != '+' && inputMessage[0] != '-')
                    {
                        Console.WriteLine($"[{senderIp.ToString()}] : {inputMessage}");
                    }

                    else
                    {               
                        ProccessMessage(inputMessage);
                    }
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10054)
                {
                    Console.WriteLine("This address is not used by anyone");
                    StartWaiting();
                }
                else
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void ProccessMessage(string inputMessage)
        {
            if (inputMessage[0] == '*')
            {
                foreach (IPEndPoint ip in clientController.GetListOfIPs(inputMessage))
                {
                    if (!ConnectedClients.Contains(ip))
                    {
                        ConnectedClients.Add(ip);
                    }
                }

                Console.Write("Now these clients (with you) are in the lobby: ");
                Console.WriteLine();
                Console.WriteLine($"{clientIpEndPoint.ToString()}");
                foreach (IPEndPoint clientIP in ConnectedClients)
                {
                    Console.WriteLine($"{clientIP.ToString()}");
                    List<IPEndPoint> tmpList = new List<IPEndPoint>();
                    tmpList.AddRange(ConnectedClients);
                    tmpList.Remove(clientIP);
                    tmpList.Add(clientIpEndPoint);
                    string ipList = "$+";

                    foreach (IPEndPoint ip in tmpList)
                    {
                        ipList += ip.Address.ToString() + ":" + ip.Port.ToString() + ",";
                    }
                    ipList = ipList.Remove(ipList.Length - 1);
                    
                    byte[] ipdata = Encoding.Unicode.GetBytes(ipList);
                    socket.SendTo(ipdata, clientIP);
                }

                Console.WriteLine();
            }


            if (inputMessage[0] == '+')
            {
                Console.Write("Now these clients (with you) are in the lobby: ");
                Console.WriteLine();
                Console.WriteLine($"{clientIpEndPoint.ToString()}");
                foreach (IPEndPoint ip in clientController.GetListOfIPs(inputMessage))
                {
                    if (!ConnectedClients.Contains(ip))
                    {
                        ConnectedClients.Add(ip);
                    }
                    Console.WriteLine($"{ip.ToString()}");
                }
                Console.WriteLine();
            }

            if (inputMessage[0] == '-')
            {
                inputMessage = inputMessage.Remove(0, 1);
                int index = inputMessage.LastIndexOf(':');
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(inputMessage.Substring(0, index)), 
                    Convert.ToInt32(inputMessage.Substring(index + 1)));
                ConnectedClients.Remove(ip);
                Console.WriteLine("{0} disconnected", ip.ToString());
            }
        }

        public void Connect()
        {
            IPEndPoint ipToConnect = clientController.GetIPEndPoint();
            if (ipToConnect.Equals(clientIpEndPoint))
            {
                Console.WriteLine("This is your Ip, try again");
                return;
            }
            if (!ConnectedClients.Contains(ipToConnect))
            {
                SendIpsToClient(ipToConnect);
                ConnectedClients.Add(ipToConnect);
            }
            else
            {
                Console.WriteLine($"You have already connected to {ipToConnect}");
            }
        }

        public void SendIpsToClient(IPEndPoint ip)
        {
            string message = "$*" + clientIpEndPoint.Address.ToString() + ":" + clientIpEndPoint.Port.ToString() + ",";
            foreach (IPEndPoint temp in ConnectedClients)
            {
                message += temp.Address.ToString() + ":" + temp.Port.ToString() + ",";
            }

            message = message.Remove(message.Length - 1);
           
            byte[] data = Encoding.Unicode.GetBytes(message);

            socket.SendTo(data, ip);
        }

        public void SendMessage(string message)
        {
            message = "$" + message;
            byte[] data = Encoding.Unicode.GetBytes(message);

            foreach (IPEndPoint tmpClientIP in ConnectedClients)
            {
                socket.SendTo(data, tmpClientIP);
            }
        }

        
        public void Disconnect()
        {
            if (ConnectedClients.Count == 0)
            {
                Console.WriteLine("You are not connected to anyone");
                return;
            }
            string message = "$-" + clientIpEndPoint.ToString();

           

            byte[] data = Encoding.Unicode.GetBytes(message);

            foreach (var connectedClient in ConnectedClients)
            {
                socket.SendTo(data, connectedClient);
            }

            ConnectedClients.Clear();

        }

        public IPEndPoint GetLocalIPEndPoint(int port)
        {
            IPEndPoint iPEndPoint = null;
            string host = Dns.GetHostName();
            IPAddress[] ipList = Dns.GetHostEntry(host).AddressList;
            foreach (IPAddress ip in ipList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    iPEndPoint = new IPEndPoint(ip, port);
                }
            }
            return iPEndPoint;
        }

        public void ShowClients()
        {
            if (ConnectedClients.Count != 0)
            {
                Console.WriteLine("Clients list");
                int i = 1;
                foreach (IPEndPoint ip in ConnectedClients)
                {
                    Console.WriteLine($"{i} : {ip.ToString()}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("The list of connected clients is empty");
            }
            
        }

        public void Exit()
        {
            Disconnect();
            IsLeave = true;
        }

        public void LeaveChat()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
