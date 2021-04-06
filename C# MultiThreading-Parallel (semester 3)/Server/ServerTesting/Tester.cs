using ServiceReference;
using System;
using System.Linq;
using System.Threading;

namespace ServerTesting
{
    internal class Tester : IServerContractCallback
    {
        private AutoResetEvent waitHandler = new AutoResetEvent(false);

        private readonly byte[] originalBytes;

        private byte[] resultBytes;

        private volatile bool isRunning;

        private volatile bool isComplete;

        private DateTime startTime;

        private DateTime endTime;

        public Tester(byte[] bytes)
        {
            originalBytes = bytes;
            resultBytes = null;
            isComplete = false;
            isRunning = false;
            startTime = DateTime.MinValue;
            endTime = DateTime.MinValue;
            Start();
        }

        private void Start()
        {
            
            startTime = DateTime.Now;
            try
            {
                ServerContractClientBase client = new ServerContractClientBase(new System.ServiceModel.InstanceContext(this));
                client.ApplyFilter(originalBytes, client.GetListOfFilters().Last());
                isRunning = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public void ReturnImage(byte[] bytes)
        {
            resultBytes = bytes;
            endTime = DateTime.Now;
            isComplete = true;
            isRunning = false;
            waitHandler.Set();
        }

        public void ReturnProgress(int progress)
        {
            return; //emulation of smth
        }

        public int GetTime()
        {
            if (isComplete || isRunning)
            {
                waitHandler.WaitOne(60000); //max time of waiting - 60 seconds
                if (resultBytes == null)
                {
                    return -1;
                }
                return (int)(endTime - startTime).TotalMilliseconds;
            }
            return 0;
        }
    }
}
