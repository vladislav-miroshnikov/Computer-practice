using ChatLibrary;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatManager manager = new ChatManager(new ChatController());
            Client client = new Client(new ChatController());
            manager.StartChating(client);
        }
    }
}
