using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BankServiceDAL;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    public BankResponse ProcessPayment(long accountNo, int amount)
    {
        BankDAL dal = new BankDAL();

        string Msg;

        bool success = dal.DebitAccount(accountNo, amount, out Msg);


        return new BankResponse
        {
            IsSuccess = success,
            Message = Msg
        };
    }

}
