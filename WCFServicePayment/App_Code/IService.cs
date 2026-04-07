using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{
    [OperationContract]
    PaymentResponse MakePayment(PaymentRequest request);
}

[DataContract]
public class PaymentRequest
{
    [DataMember]
    public long AccountNo { get; set; }

    [DataMember]
    public int Amount { get; set; }

    [DataMember]
    public int BillId { get; set; }

    [DataMember]
    public int OrderId { get; set; }
}

[DataContract]
public class PaymentResponse
{
    [DataMember]
    public bool IsSuccess { get; set; }

    [DataMember]
    public string Message { get; set; }
}