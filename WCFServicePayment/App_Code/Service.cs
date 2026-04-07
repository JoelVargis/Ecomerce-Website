using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LogicLayer;
// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.

public class Service : IService
{
    public PaymentResponse MakePayment(PaymentRequest request)
    {
        BankServiceReference.ServiceClient bankClient = null;

        try
        {
            bankClient = new BankServiceReference.ServiceClient();

            var bankResponse = bankClient.ProcessPayment(
                request.AccountNo,
                request.Amount
            );

            if (bankResponse.IsSuccess)
            {
                BillBLL billBll = new BillBLL();
                OrdersBLL bll = new OrdersBLL();
                billBll.MarkBillAsPaid(request.BillId);
                bll.MarkOrderAsOrdered(request.OrderId);

                return new PaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment successful"
                };
            }          
            return new PaymentResponse
            {
                IsSuccess = false,
                Message = bankResponse.Message
            };
        }
        catch
        {
            return new PaymentResponse
            {
                IsSuccess = false,
                Message = "Payment service error"
            };
        }
        finally
        {
            if (bankClient != null)
            {
                try { bankClient.Close(); }
                catch { bankClient.Abort(); }
            }
        }
    }
}