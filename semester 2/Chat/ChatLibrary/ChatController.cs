using System;
using System.Collections.Generic;
using System.Net;

namespace ChatLibrary
{
    public class ChatController : IController
    {
        public int GetPortValue()
        {
            int result = 0;
            bool isInitCorrect = false;
            while (!isInitCorrect)
            {
                try
                {
                    Console.Write("Input your port: ");
                    string input = Console.ReadLine().Trim();
                    if (input == "exit")
                    {
                        Environment.Exit(0);
                    }
                    int tmpPortVal = int.Parse(input);
                    if (tmpPortVal >= 1 && tmpPortVal <= 65535)
                    {
                        result = tmpPortVal;
                        isInitCorrect = true;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Incorrect input port value, try again!");
                }
            }

            return result;
        }

        public string GetMessage()
        {
            return Console.ReadLine();
        }

        public IPEndPoint GetIPEndPoint()
        {
            bool isCorrectInput = false;
            while (!isCorrectInput)
            {
                try
                {
                    Console.Write("Input IP:port ");
                    string input = Console.ReadLine().Trim();
                    if (input == "exit")
                    {
                        Environment.Exit(0);
                    }
                    IsCorrectFormatIPAndPort(input);
                    try
                    {
                        int index = input.LastIndexOf(':');
                        return new IPEndPoint(
                            IPAddress.Parse(input.Substring(0, index)),
                            Convert.ToInt32(input.Substring(index + 1)));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error format of IP");
                        throw new ArgumentException();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input IP & port in this format: X.X.X.X:Y");
                }

            }

            return new IPEndPoint(IPAddress.None, 0);
        }

        private void IsCorrectFormatIPAndPort(string ipAndPort)
        {
            try
            {
                int i = 0;
                for (int j = 0; j < 4; ++j)
                {
                    string temp = "";
                    while (ipAndPort[i] != '.' && ipAndPort[i] != ':')
                    {
                        temp += ipAndPort[i];
                        i++;
                    }

                    try
                    {
                        if (Convert.ToInt32(temp) < 0 || Convert.ToInt32(temp) > 255)
                        {
                            throw new FormatException();
                        }
                    }
                    catch (Exception)
                    {
                        throw new FormatException();
                    }

                    i++;
                }

                string tmpPort = "";
                while (i < ipAndPort.Length)
                {
                    tmpPort += ipAndPort[i];
                    i++;
                }

                try
                {
                    Convert.ToInt32(tmpPort);
                }
                catch (Exception)
                {
                    throw new FormatException();
                }
            }
            catch (Exception)
            {
                throw new FormatException();
            }
        }

        public List<IPEndPoint> GetListOfIPs(string input)
        {
            List<IPEndPoint> list = new List<IPEndPoint>();
            input = input.Remove(0, 1);
            string[] iPs = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tmp in iPs)
            {
                int ipAddressLength = tmp.LastIndexOf(':');
                list.Add(new IPEndPoint(IPAddress.Parse(tmp.Substring(0, ipAddressLength)), Convert.ToInt32(tmp.Substring(ipAddressLength + 1))));
            }
            return list;
        }
    }
}
