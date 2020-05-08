using System.Collections.Generic;
using System.Net;

namespace ChatLibrary
{
    public interface IController
    {
        string GetMessage();
        int GetPortValue();
        IPEndPoint GetIPEndPoint();
        List<IPEndPoint> GetListOfIPs(string input);
    }
}
