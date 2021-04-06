using System.Collections.Generic;
using System.ServiceModel;

namespace ServerContract
{
    [ServiceContract(CallbackContract = typeof(ICallBackContract))]
    public interface IServerContract
    {
        [OperationContract]
        List<string> GetListOfFilters();

        [OperationContract(IsOneWay = true)]
        void ApplyFilter(byte[] bytes, string filterName);

    }
}
