using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BankServiceDAL
{
    public class BankDAL
    {
        DBConnect db = new DBConnect();

        public bool DebitAccount(long accountNo,int amount,out string message)
        {
          
            message = "";

          
            string selectQuery = @"SELECT AccountBalance FROM UserAccounts WHERE AccountNo = @AccountNo";
               
            SqlParameter[] selectParams =
            {
                new SqlParameter("@AccountNo",accountNo),
              
            };

            string balanceStr = db.fun_executescalar(selectQuery, selectParams);

            if (balanceStr == "null")
            {
                message = "Invalid account number";
                return false;
            }

            int balance = Convert.ToInt32(balanceStr);

            
            if (balance < amount)
            {
                message = "Insufficient balance";
                return false;
            }

       
            string updateQuery = @"
                UPDATE UserAccounts
                SET AccountBalance = AccountBalance - @Amount
                WHERE AccountNo = @AccountNo";

            SqlParameter[] updateParams =
            {
                new SqlParameter("@Amount", amount),
                new SqlParameter("@AccountNo", accountNo)
            };

            int rows = db.fun_executenonquery(updateQuery, updateParams);

            if (rows <= 0)
            {
                message = "Debit failed";
                return false;
            }
         
            message = "Payment approved by bank";
            return true;
        }
    }
}
