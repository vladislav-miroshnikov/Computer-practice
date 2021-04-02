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
            Console.WriteLine("Welcome to app <ChatManager>, in order to get into the network, enter the port");
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
                              "Write the name of one of the commands below\n" +
                              "Also text that is not recognized as any of the following commands is regarded as a message and will be sent to all clients (if any):\n" +
                              "\n-Connect  -  connect to another chat (you can also connect to another version of the chat)\n" +
                              "-Disconnect  -  to disconnect from chat\n" +
                              "-Clients  -  to show connected clients\n" +
                              "-Exit  -  to exit from chat\n");
        }       
    }
}
