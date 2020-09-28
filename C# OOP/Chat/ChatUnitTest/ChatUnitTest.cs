using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatLibrary;
using Moq;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatUnitTest
{
    [TestClass]
    public class ChatUnitTest
    {
        [TestMethod]
        public void ChatTest()
        {

            ChatManager[] chatManagers = new ChatManager[2];
            Client[] clients = new Client[2];
            Mock<IController>[] clientMocks = new Mock<IController>[2];
            Mock<IController>[] chatMocks = new Mock<IController>[2];

            for (int i = 0; i < 2; i++)
            {
                clientMocks[i] = new Mock<IController>();
                chatMocks[i] = new Mock<IController>();
                clientMocks[i].Setup(a => a.GetPortValue()).Returns(i + 1);
                clients[i] = new Client(clientMocks[i].Object);
            }

            IPEndPoint iPEndPointFirst = null;
            IPEndPoint iPEndPointSecond = null;

            foreach (var iPAdress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (iPAdress.AddressFamily == AddressFamily.InterNetwork)
                {
                    iPEndPointFirst = new IPEndPoint(iPAdress, 1);
                    iPEndPointSecond = new IPEndPoint(iPAdress, 2);
                    break;
                }
            }

            chatMocks[0].Setup(r => r.GetMessage()).Returns("connect");
            clientMocks[0].Setup(e => e.GetIPEndPoint()).Returns(clients[0].GetLocalIPEndPoint(2));

            clientMocks[0].Setup(s => s.GetListOfIPs(It.IsAny<string>())).Returns(new List<IPEndPoint> { iPEndPointSecond });
            clientMocks[1].Setup(q => q.GetListOfIPs(It.IsAny<string>())).Returns(new List<IPEndPoint> { iPEndPointFirst });

            chatMocks[1].Setup(w => w.GetMessage()).Returns(() =>
            {
                Thread.Sleep(50);
                return ("exit");
            });

            chatManagers[0] = new ChatManager(chatMocks[0].Object);
            Thread thread = new Thread(() => chatManagers[0].StartChating(clients[0]));
            thread.Start();

            chatManagers[1] = new ChatManager(chatMocks[1].Object);
            chatManagers[1].StartChating(clients[1]);

            //thread.Abort();
            //Check that clients are really connected
            Assert.AreEqual(1, chatManagers[0].clientsHistory.Count);
            Assert.AreEqual(1, chatManagers[1].clientsHistory.Count);
            
        }
    }
}
