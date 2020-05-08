using System;
using System.Collections.Generic;
using System.Net;

namespace ChatLibrary
{
    public class ChatManager
    {
        private readonly IController controller;

        public List<IPEndPoint> clientsHistory;

        public ChatManager(IController controller)
        {
            this.controller = controller;
            clientsHistory = new List<IPEndPoint>();
        }

        public void StartChating(Client client)
        {
            PrintInformation();
            client.StartWaiting();
            while (!client.IsLeave)
            {
                try
                {
                    string message = controller.GetMessage();

                    if (message == "")
                    {
                        throw new ArgumentException();
                    }

                    switch (message.Trim())
                    {

                        case "connect":
                            {
                                client.Connect();
                                clientsHistory = client.ConnectedClients;
                                break;
                            }

                        case "disconnect":
                            {
                                client.Disconnect();
                                break;
                            }
                        case "clients":
                            {
                                client.ShowClients();
                                break;
                            }
                        case "exit":
                            {
                                clientsHistory = client.ConnectedClients;
                                client.Exit();
                                client.LeaveChat();
                                //Environment.Exit(0);
                                break;
                            }
                        default:
                            {
                                client.SendMessage(message);
                                break;
                            }
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Error! You entered nothing, try again");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        private void PrintInformation()
        {
            Console.WriteLine("\n" +
                              "Connect  -  to connect to other clients\n" +
                              "Disconnect  -  to disconnect from this lobby\n" +
                              "Clients  -  to show who is in this lobby now\n" +
                              "Help  -  to show this help window\n" +
                              "Exit  -  to exit from chat\n");
        }       
    }
}
