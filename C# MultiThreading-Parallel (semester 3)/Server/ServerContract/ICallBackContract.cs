using System.ServiceModel;


namespace ServerContract
{
    public interface ICallBackContract
    {
        [OperationContract(IsOneWay = true)]
        void ReturnImage(byte[] bmp);

        [OperationContract(IsOneWay = true)]
        void ReturnProgress(int progress);

    }
}
